namespace ComputationExpression

module AsyncResultDemo =
    open System
    open FsToolkit.ErrorHandling

    type AuthError =
        | UserBannedOrSuspended

    type TokenError =
        | BadThingHappened of string

    type LoginError =
        | InvalidUser
        | InvalidPwd
        | Unauthorized of AuthError
        | TokenErr of TokenError

    type AuthToken = AuthToken of Guid

    type UserStatus =
    | Active
    | Suspended
    | Banned

    type User = {
        Name : string
        Password : string
        Status : UserStatus
    }

    [<Literal>]
    let ValidPassword = "password"
    [<Literal>]
    let ValidUser = "isvalid"
    [<Literal>]
    let SuspendedUser = "issuspended"
    [<Literal>]
    let BannedUser = "isbanned"
    [<Literal>]
    let BadLuckUser = "hasbadluck"
    [<Literal>]
    let AuthErrorMessage = "Earth's core stopped spinning"

    // string -> Async<User option>
    let tryGetUser username =
        async {
            let user = { Name = username; Password = ValidPassword; Status = Active }
            return
                match username with
                | ValidUser -> Some user
                | SuspendedUser -> Some { user with Status = Suspended}
                | BannedUser -> Some { user with Status = Banned}
                | BadLuckUser -> Some user
                | _ -> None
        }

    // string -> user -> bool
    let isPwdValid password user =
        password = user.Password

    // User -> Async<Result<unit, AuthError>>
    let authorize user =
        async {
            return
                match user.Status with
                | Active -> Ok()
                | _ -> UserBannedOrSuspended |> Error
        }
        
    // User -> Result<AuthToken, TokenError>
    let createAuthToken user =
        try
            if user.Name = BadLuckUser then failwith AuthErrorMessage
            else Guid.NewGuid() |> AuthToken |> Ok
        with
        | ex -> ex.Message |> BadThingHappened |> Error

    let login username password : Async<Result<AuthToken, LoginError>> =
        asyncResult {
            let! user = username |> tryGetUser |> AsyncResult.requireSome InvalidUser
            do! user |> isPwdValid password |> Result.requireTrue InvalidPwd
            do! user |> authorize |> AsyncResult.mapError Unauthorized // MapError used to convert the specific errors to LoginError types
            return! user |> createAuthToken |> Result.mapError TokenErr
        }

module AsyncResultDemoTests =

    open AsyncResultDemo

    let [<Literal>] BadPassword = "notpassword"
    let [<Literal>] NotValidUser = "notvalid"

    let isOk (input:Result<_,_>) : bool =
        match input with
        | Ok _ -> true
        | _ -> false
    
    let matchError (error:LoginError) (input:Result<_,LoginError>) =
        match input with
        | Error ex -> ex = error
        | _ -> false  

    let runWithValidPassword (username:string) = 
        login username ValidPassword |> Async.RunSynchronously
    
    let success =
        let result = runWithValidPassword ValidUser
        result |> isOk 

    let badPassword =
        let result = login ValidUser BadPassword |> Async.RunSynchronously
        result |> matchError InvalidPwd

    let invalidUser =
        runWithValidPassword NotValidUser
        |> matchError InvalidUser

    let isSuspended =
        runWithValidPassword SuspendedUser
        |> matchError (UserBannedOrSuspended |> Unauthorized)

    let isBanned =
        let result = runWithValidPassword BannedUser
        result |> matchError (UserBannedOrSuspended |> Unauthorized)

    let hasBadLuck =
        let result = runWithValidPassword BadLuckUser
        result |> matchError (AuthErrorMessage |> BadThingHappened |> TokenErr)