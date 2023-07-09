module Part2

// Handling Exceptions:

open System

// decimal -> decimal -> decimal
let tryDivide (x: decimal) (y: decimal) =
    try
        x / y
    with
    | :? DivideByZeroException as ex -> raise ex

// decimal -> decimal -> Result<decimal,exn>
let tryDivide2 (x: decimal)(y: decimal) =
    try
        Ok (x/y)
    with
    | :? DivideByZeroException as ex -> Error ex

let badDivide = tryDivide2 1M 0M
let goodDivide = tryDivide2 1M 1M

/////// Function Composition with Result

type Customer = {
    Id: int
    IsVip: bool
    Credit: decimal
}

// Customer -> Result<(Customer * decimal)>, exn>
let getPurchases customer =
    try
        // Imagine this function is fetching data from a database
        let purchases =
            if customer.Id % 2 = 0 then (customer, 120M)
            else (customer, 80M)
        Ok purchases
    with
    | ex -> Error ex

// Customer * decimal -> Customer
let tryPromoteToVip purchases =
    let customer, amount = purchases
    if amount > 100M then { customer with IsVip = true }
    else customer

// Customer -> Result<Customer, exn>
// One track function: Doesn't output a result type and will only execute on the "OK track"
let increaseCreditIfVip customer =
    try
        // Imagine this function could cause an exception
        let increase =
            if customer.IsVip then 100.0M else 50.0M
        Ok { customer with Credit = customer.Credit + increase}
    with
    | ex -> Error ex

// Higher order function because it takes tryPromoteToVip function as input parameter
// (Customer * decimal -> Customer) -> Result<Customer * decimal, exn> -> Result<Customer, exn>
let map1 (tryPromoteToVip:Customer * decimal -> Customer) (result:Result<Customer * decimal, exn>) : Result<Customer, exn> =
    match result with
    | Ok x -> Ok (tryPromoteToVip x)
    | Error ex -> Error ex

// ('a -> 'b) -> Result<'a,'c> -> Result<'b,'c>
let map2 (f: 'a -> 'b) (result:Result<'a,'c>) : Result<'b, 'c> =
    match result with
    | Ok x -> Ok (f x)
    | Error ex -> Error ex

// ('a -> 'b) -> Result<'a,'c> -> Result<'b,'c>
let map f result =
    match result with
    | Ok x -> Ok (f x)
    | Error ex -> Error ex

// (Customer -> Result<Customer,exn>) -> Result<Customer, exn> -> Result<Customer, exn>
let bind1 (increaseCreditIfVip: Customer -> Result<Customer, exn>) (result:Result<Customer, exn>) : Result<Customer,exn> =
    match result with
    | Ok x -> increaseCreditIfVip x
    | Error ex -> Error ex

// ('a -> Result<'b, 'c>) -> Result<'a,'c> -> Result<'b, 'c>
let bind2 (f: 'a -> Result<'b,'c>) (result:Result<'a,'c>) : Result<'b, 'c> =
    match result with
    | Ok x -> f x
    | Error ex -> Error ex

// ('a -> Result<'b, 'c>) -> Result<'a,'c> -> Result<'b, 'c>
let bind f result =
    match result with
    | Ok x -> f x
    | Error ex -> Error ex

let upgradeCustomer customer =
    customer
    |> getPurchases
    |> fun result ->
        match result with
        | Ok x -> Ok(tryPromoteToVip x)
        | Error ex -> Error ex
    |> fun result ->
        match result with
        | Ok x -> increaseCreditIfVip x
        | Error ex -> Error ex

// Can be simplified slightly with function keyword:
let upgradeCustomer2 customer =
    customer
    |> getPurchases
    |> function
        | Ok x -> Ok (tryPromoteToVip x)
        | Error ex -> Error ex
    |> function
        | Ok x -> increaseCreditIfVip x
        | Error ex -> Error ex

let upgradeCustomer3 customer =
    customer
    |> getPurchases
    |> map tryPromoteToVip // Map Takes a function (tryPromoteToVip) that operates on the inner value (Customer * decimal), but does not accept or return the monad itself, and wraps the output in a new instance of the same monadic type
    |> bind increaseCreditIfVip // Bind Takes a function (increaseCreditIfVip) that returns a monadic value. That function operates on the inner value of the agument monad, which itself returns a new monad.

let upgradeCustomerProcedural customer =
    let purchasedResult = getPurchases customer
    let promotedResult = map tryPromoteToVip purchasedResult
    let increaseResult = bind increaseCreditIfVip promotedResult
    increaseResult

let upgradeCustomerFinal customer =
    customer
    |> getPurchases
    |> Result.map tryPromoteToVip
    |> Result.bind increaseCreditIfVip

let customerVIP = { Id = 1; IsVip = true; Credit = 0.0M }
let customerSTD = { Id = 2; IsVip = false; Credit = 100.0M }

let assertVIP =
    upgradeCustomerFinal customerVIP = Ok { Id = 1; IsVip = true; Credit = 100.0M }

let assertSTDtoVIP =
    upgradeCustomerFinal customerVIP = Ok { Id = 2; IsVip = true; Credit = 200.0M }

let assertSTD =
    upgradeCustomerFinal { customerSTD with Id = 3; Credit = 50.0M } = Ok { Id = 3; IsVip = false; Credit = 100.0M }

//////////////////////////

// string -> DateTime option
let tryParseDateTime (input : string) =
    let (success, value) = DateTime.TryParse input
    if success then Some value else None

// Result<string, exn>
let getResult =
    try
        Ok "Hello"
    with
    |ex -> Error ex

let parsedDT = getResult |> map tryParseDateTime

////////////////////////////