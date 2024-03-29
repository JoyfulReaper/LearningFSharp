namespace ComputationExpressions

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
        | Banned
        | Suspended
        
    type User =
        {
            Name: string
            Password: string
            Status: UserStatus
        }
        
    let [<Literal>] ValidPassword = "password"
    let [<Literal>] ValidUser = "user"
    let [<Literal>] SuspendedUser = "issuspended"
    let [<Literal>] BannedUser = "isbanned"
    let [<Literal>] BadLuckUser = "hasbadluck"
    let [<Literal>] AuthErrorMessage = "Earth's core stopped spinning"
    
    let tryGetUser username =
        async {
            let user = { Name = username; Password = ValidPassword; Status = Active }
            return
                match username with
                | ValidUser -> Some user
                | SuspendedUser -> Some { user with Status = Suspended }
                | BannedUser -> Some { user with Status = Banned }
                | BadLuckUser -> Some user
                | _ -> None
        }
        
    let isPwdValid password user =
        password = user.Password
        
    let authorize user =
        async {
            return
                match user.Status with
                | Active -> Ok ()
                | _ -> UserBannedOrSuspended |> Error
        }
        
    let createAuthToken user =
        try
            if user.Name = BadLuckUser then failwith AuthErrorMessage
            else Guid.NewGuid() |> AuthToken |> Ok
        with
        | ex -> BadThingHappened ex.Message |> Error
        
    let login username password =
        asyncResult {
            let! user = username |> tryGetUser |> AsyncResult.requireSome InvalidUser
            do! user |> isPwdValid password |> Result.requireTrue InvalidPwd
            do! user |> authorize |> AsyncResult.mapError Unauthorized
            return! user |> createAuthToken |> Result.mapError TokenErr
        }