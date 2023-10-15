// Reccommended Approach

type CustomerId = CustomerId of string

type RegisteredCustomer = {
    Id : CustomerId
}

type UnregisteredCustomer = {
    Id : CustomerId
}

type ValidationError =
    | InputOutOfRange of string
    
type Spend = private Spend of decimal
module Spend = 
    let value input = input |> fun (Spend value) -> value
    
    let create input =
        if input >= 0.0M && input <= 1000.0M then
            Ok (Spend input)
        else
            Error (InputOutOfRange "Spend must be between 0 and 1000")

type Total = decimal
type DiscountPercentage = decimal

type Customer =
    | Eligible of RegisteredCustomer
    | Registered of RegisteredCustomer
    | Guest of UnregisteredCustomer

module Customer =
    let calculateDiscountPercentage spend customer : DiscountPercentage =
        match customer with
        | Eligible _ -> if Spend.value spend >= 100M then 0.1M else 0.0M
        | _ -> 0.0M


    let calculateTotal customer spend : Total = // Deconstucting the DU in in the function parameter
        customer
        |> calculateDiscountPercentage spend
        |> fun discountPercentage -> Spend.value spend * (1.0M - discountPercentage)

let john = Eligible { Id = CustomerId "John" }
let mary = Eligible { Id = CustomerId "Mary" }
let richard = Registered { Id = CustomerId "Richard" }
let sarah = Guest { Id = CustomerId "Sarah" }

// 'a -> 'a -> bool
let isEqualTo expected actual =
    actual = expected

let assertEqual customer spent expected =
    Spend.create spent
    |> Result.map (fun spend -> Customer.calculateTotal customer spend)
    |> isEqualTo (Ok expected)

let assertJohn = assertEqual john 100.0M 90.0M
let assertMary = assertEqual mary 99.0M 99.0M
let assertRichard = assertEqual richard 100.0M 100.0M
let assertSarah = assertEqual sarah 100.0M 100.0M
