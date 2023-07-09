/////// Functions /////////////

type Customer = {
    Id: int
    IsVip: bool
    Credit: decimal
}

// Customer -> (Customer * decimal)
let getPurchases customer =
    let purchases = if customer.Id % 2 = 0 then 120M else 80M
    (customer, purchases) // Parenthese are optional

// (Customer * decimal) -> Customer
let tryPromoteToVip purchases =
    let (customer, amount) = purchases
    if amount > 100M then { customer with IsVip = true}
    else customer

// Customer -> Customer
let increaseCreditIfVip customer =
    let increase = if customer.IsVip then 100M else 50M
    { customer with Credit = customer.Credit + increase }

// Composition operator
let upgradeCustomerComposed = // Customer -> Customer
    getPurchases >> tryPromoteToVip >> increaseCreditIfVip

// Nested
let upgradeCustomerNested customer = // Customer -> Customer
    increaseCreditIfVip(tryPromoteToVip(getPurchases customer))

// Procedural
let upgradeCustomerProcedural customer = // Customer -> Customer
    let customerWithPurchases = getPurchases customer
    let promotedCustomer = tryPromoteToVip customerWithPurchases
    let increasedCreditCustomer = increaseCreditIfVip promotedCustomer
    increasedCreditCustomer

// Forward pipe operator
let upgradeCustomerPiped customer = // Customer -> Customer
    customer
    |> getPurchases
    |> tryPromoteToVip
    |> increaseCreditIfVip
    
let customerVIP = { Id = 1; IsVip = true; Credit = 0.0M }
let customerSTD = { Id = 2; IsVip = false; Credit = 100M }

let assertVIP = 
    upgradeCustomerComposed customerVIP = { Id = 1; IsVip = true; Credit = 100M; }

let assertSTDtoVIP =
    upgradeCustomerComposed customerSTD = { Id = 2; IsVip = true; Credit = 200M }

let assertSTD =
    upgradeCustomerComposed { customerSTD with Id = 3; Credit = 50M;} = {Id = 3; IsVip = false; Credit = 100M }

///////////Unit/////////////

open System

// unit -> System.DateTime
let now () = DateTime.UtcNow

// 'a -> unit
let log msg =
    sprintf "Pretend this was logged" |> ignore
    ()

//////////Anonymous Functions/////////////

let add x y = x + y

// Anonymous function:
let addLambda = fun x y -> x + y

// Generic Function that can take any function with two input parameters
// ('a -> 'b -> 'c) -> 'a -> 'b -> 'c
// first argument to apply (f) is a function that takes arguments 'a and 'b and returns 'c ('a -> 'b -> 'C) - Type signature of function that apply expects
// second argument to apply (x) is of type 'a, first argument passed to function f
// third argument to apply (y) is of type 'b, second argument passed to f
let apply f x y = f x y

let sum = apply add 1 4
let sumLambda = apply (fun x y -> x + y) 1 4

//////////////////////

// Creates a new instance of the random class every time its called
let rnd () =
    let rand = Random()
    rand.Next(100)

let random = List.init 50 (fun _ -> rnd())

//Uses scoping rules to re-use the same instance of the Randome class each time the rnd2 function is called
// rand is creted once and captured in the closure of the rnd2 function
let rnd2 =
    // Created once during the initialization of `rnd2`
    //rnd2 function is returned as a closure that captures the rand instance
    let rand = Random() 
    fun () -> rand.Next(100)

let random2 = List.init 50 (fun _ -> rnd2 () )

/////////Multiple Parameters//////////

// Last Chapter:
// Customer -> decimal -> decimal
// let calculateTotal customer spend = ...

// Takes a Customer as input and returns a function with the signature (decimal -> decimal) as output
// Customer -> (decimal -> decimal)
// let calculateTotal customer =
//  fun spend -> (...)

// The ability to automatically change single input/single output functions together is called Currying
// Allows writing functions that appear to have multiple input arguments, but are actually a chain of one input/one ouput functions.

//////Partial Application//////

// Customer -> decimal -> decimal
// let calculateTotal customer spend = ...

type Customer2 =
    | Registered of Id: string * IsEligible:bool  // Looks like a tuple, but is not. Allows lables
    | Guest of Id: string

let calculateTotal customer spend =
    let discount =
        match customer with
        | Registered (IsEligible = true) when spend >= 100M -> spend * 0.1M
        | _ -> 0.0M
    spend - discount

// Decimal -> Decimal
let john = Registered ("John", true)
let partial = calculateTotal john

// Decimal
let complete = partial 100.0M

//////////////// Forward Pipe Operator ////////////////

let complete2 = 100.0M |> calculateTotal john

// 'a -> 'a -> bool
let isEqualTo expected actual =
    expected = actual

let assertJohn = isEqualTo 90.0M (calculateTotal john 100.0M)

// 'a -> ('a -> 'b) -> 'b
// let (|>) v f = f v

// isEqualTo is partially applied as part of the forward pipe
// passing the result of calculateTotal function to the right hand side
let assertJohn2 = calculateTotal john 100.0M |> isEqualTo 90.0M
///////////////// Result of calculateTotal is 'a -> isEqual is ('a -> 'b)


// decimal -> (decimal -> bool) -> bool
// pipe forward operator in prefix form
let assertJohn3 = (|>) (calculateTotal john 100.0M) (isEqualTo 90.0M)

// decimal -> (decimal -> bool) -> bool
// pipe forward operator in infix form
let assertJohn4 = (calculateTotal john 100.0M) |> (isEqualTo 90.0M)

// decimal -> (decimal -> bool) -> bool
let assertJohn5 = calculateTotal john 100.0M |> (isEqualTo 90.0M)


/////////// Partial Application Part 2///////////////////

type LogLevel =
    | Error
    | Warning
    | Info

// LogLevel -> String -> unit
let log2 (level : LogLevel) message =
    printfn "[%A]: %s" level message
    ()

// String interpolation can specify format specifiers or not:
(*
// LogLevel -> string -> unit
let log (level:LogLevel) message =
    printfn $"[%A{level}]: %s{message}"

// LogLevel -> 'a -> unit
let log (level:LogLevel) message =
    printfn $"[{level}]: {message}"
*)


let logError = log2 Error // string -> unit

log2 Error "Curried function"
logError "Partially Applied Function"


let calculateTotal2 customer =
    fun spend ->
        let discount =
            match customer with
            | Registered (IsEligible = true) when spend >= 100M -> spend * 0.1M
            | _ -> 0.0M
        spend - discount