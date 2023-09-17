open System

type Customer = {
    Id : int
    IsVip : bool
    Credit : decimal
}

// Customer -> (Customer * decimal)
let getPurchases customer =
    let purchases = if customer.Id % 2 = 0 then 120M else 80M
    (customer, purchases)

// (Customer * decimal) -> Customer
let tryPromoteToVip purchases =
    let (customer, amount) = purchases
    if amount > 100M then { customer with IsVip = true}
    else customer

// Customer -> Customer
let increaseCreditIfVip customer =
    let increase = if customer.IsVip then 100M else 50M
    { customer with Credit = customer.Credit + increase}




// Ways the F# supports function composition:

// Composition Operator
// Customer -> Customer
let upgradeCustomerComposed =
    getPurchases >> tryPromoteToVip >> increaseCreditIfVip

// Nested
// Customer -> Customer
let upgradeCustomerNested customer =
    increaseCreditIfVip(tryPromoteToVip(getPurchases customer))

// Procedural
// Customer -> Customer
let upgradeCustomerProcedural customer =
    let customerWithPurchases = getPurchases customer
    let promotedCustomer = tryPromoteToVip customerWithPurchases
    let increasedCreditCustomer = increaseCreditIfVip promotedCustomer
    increasedCreditCustomer

// Forward Pipe Operator
let upgradeCustomerPiped customer = // Customer -> Customer
    customer
    |> getPurchases
    |> tryPromoteToVip
    |> increaseCreditIfVip

let customerVIP = { Id = 1; IsVip = true; Credit = 0.0M }
let customerSTD = { Id = 2; IsVip = false; Credit = 100.0M }

let assertVIP =
    upgradeCustomerComposed customerVIP = {Id = 1; IsVip = true; Credit = 100.0M }
let assertSTDtoVIP =
    upgradeCustomerComposed customerSTD = {Id = 2; IsVip = true; Credit = 200.0M }
let assertSTD =
    upgradeCustomerComposed { customerSTD with Id = 3; Credit = 50.0M } = {Id = 3; IsVip = false; Credit = 100.0M }

// UNIT
let now () = DateTime.UtcNow

// Anonymous Functions
let add x y = x + y
let sum = add 1 4

// Written using a lambda
let add' = fun x y -> x + y
let sum' = add' 1 4

// ('a -> 'b -> 'c) -> 'a -> 'b -> 'c
let apply f x y = f x y

let sum'' = apply add 1 4
let sum''' = (fun x y -> x + y) 1 4

// Creates a new instance of the Random class each call
let rnd () =
    let rand = Random()
    rand.Next(100)

// Re-uses the same instance of the Random class
let rnd' () =
    let rand = Random()
    fun () -> rand.Next(100)

List.init 50 (fun _ -> rnd())

// Partial Application
type LogLevel =
    | Error
    | Warning
    | Info

// LogLevel -> string -> unit
let log (level : LogLevel) message =
    // with string interpolation (also supports format specifiers)
    // printfn $"[%A{level}]: %s{message}"
    printfn "[%A]: %s" level message

// string -> unit
let logError = log Error

let e1 = log Error "Curried function"
let e2 = logError "Partially Applied function"