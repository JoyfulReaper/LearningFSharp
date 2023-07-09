module Part3

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

// Map takes a function that does not accept or return a Result and wraps it's output in a Result
// ('a -> 'b) -> Result<'a,'c> -> Result<'b,'c>
let map f result =
    match result with
    | Ok x -> Ok (f x)
    | Error ex -> Error ex

// ('a -> Result<'b, 'c>) -> Result<'a,'c> -> Result<'b, 'c>
// Bind takes a function that returns a result, unwraps the value in the result argument, and then calls the provided function with that argument
// Bind enables you to chain computations that depend on the outcome of previous computations. It handles the propagation of success or failure through the monadic context.
let bind f result =
    match result with
    | Ok x -> f x
    | Error ex -> Error ex

let upgradeCustomer customer =
    customer
    |> getPurchases
    |> map tryPromoteToVip
    |> bind increaseCreditIfVip

let customerVIP = { Id = 1; IsVip = true; Credit = 0.0M }
let customerSTD = { Id = 2; IsVip = false; Credit = 100.0M }

let assertVIP =
    upgradeCustomer customerVIP = Ok { Id = 1; IsVip = true; Credit = 100.0M }

let assertSTDtoVIP =
    upgradeCustomer customerVIP = Ok { Id = 2; IsVip = true; Credit = 2000.0M }

let assertSTD =
    upgradeCustomer { customerSTD with Id = 3; Credit = 50.0M } = Ok { Id = 3; IsVip = false; Credit = 100.0M }
