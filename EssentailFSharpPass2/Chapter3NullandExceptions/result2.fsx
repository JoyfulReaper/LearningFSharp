open System

type Customer = {
    Id : int
    IsVip : bool
    Credit : decimal
}

// Customer -> Result<(Customer * decimal),exn>
let getPurchases customer = 
    try
        // Imagine this function is fetching data from a Database
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

// Customer -> Result<Customer,exn>
let increaseCreditIfVip customer = 
    try
        // Imagine this function could cause an exception
        let increase =  
            if customer.IsVip then 100.0M else 50.0M
        Ok { customer with Credit = customer.Credit + increase }
    with
    | ex -> Error ex


/////////// MAP ////////////
// (Customer * decimal -> Customer) -> Result<Customer * decimal, exn> -> Result<Customer,exn>
let map (tryPromoteToVip:Customer * decimal -> Customer) (result : Result<Customer * decimal, exn>) : Result<Customer, exn> =
    match result with
    | Ok x -> Ok (tryPromoteToVip x)
    | Error ex -> Error ex

// ('a -> 'b) -> Result<'a, 'c> -> Result<'b 'c>
let map' (f: 'a -> 'b) (result: Result<'a, 'c>) : Result<'b, 'c> =
    match result with
    | Ok x -> Ok <| f x
    | Error ex -> Error ex

// ('a -> 'b) -> Result<'a, 'c> -> Result<'b 'c>
let map'' f result =
    match result with
    | Ok x -> Ok (f x)
    | Error ex -> Error ex


/////////// BIND ////////////
let bind (increaseCreditIfVip:Customer -> Result<Customer, exn>) (result:Result<Customer, exn>) : Result<Customer, exn> =
    match result with
    | Ok x -> increaseCreditIfVip x
    | Error ex -> Error ex

let bind' (f:'a -> Result<'b, 'c>) (result:Result<'a, 'c>) : Result<'b, 'c> =
    match result with
    | Ok x -> f x
    | Error ex -> Error ex

let bind'' f result =
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

let upgradeCustomer' customer = // Simpified version
    customer
    |> getPurchases
    |> function
        | Ok x -> Ok (tryPromoteToVip x)
        | Error ex -> Error ex
    |> function
        | Ok x -> increaseCreditIfVip x
        | Error ex -> Error ex


// Using Map and Bind
let upgradeCustomer'' customer =
    customer
    |> getPurchases
    |> map'' tryPromoteToVip
    |> bind'' increaseCreditIfVip


// Using Map and Bind Procedurally
let upgradeCustomerProcedural customer =
    let purchasedResult = getPurchases customer
    let promotedResult = map tryPromoteToVip purchasedResult
    let increaseResult = bind increaseCreditIfVip promotedResult
    increaseResult

let customerVIP = { Id = 1; IsVip = true; Credit = 0.0M }
let customerSTD = { Id = 2; IsVip = false; Credit = 100.0M }

let assertVIP = 
    upgradeCustomer'' customerVIP = Ok {Id = 1; IsVip = true; Credit = 100.0M }
let assertSTDtoVIP = 
    upgradeCustomer'' customerSTD = Ok {Id = 2; IsVip = true; Credit = 200.0M }
let assertSTD = 
    upgradeCustomer'' { customerSTD with Id = 3; Credit = 50.0M } = Ok {Id = 3; IsVip = false; Credit = 100.0M }


//////////////////////////////////////////////////////
// string -> DateTime option
let tryParseDateTime (input:string) =
    let success, value = DateTime.TryParse input
    if success then Some value else None

let getResult =
    try
        Ok "Hello"
    with
    | ex -> Error ex

let getResult' =
    try
        Ok "12-25-2015"
    with
    | ex -> Error ex

let parsedDT = getResult |> map'' tryParseDateTime
let parsedDT' = getResult' |> map'' tryParseDateTime