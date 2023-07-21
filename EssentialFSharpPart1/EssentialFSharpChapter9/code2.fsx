type CustomerId = CustomerId of string

type RegisteredCustomer = {
    Id : string
}

type UnregisteredCustomer =  {
    Id : string
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
            Error (InputOutOfRange "You can only spend between 0 and 1000")

// Type Abbreviations
type Total = decimal
type DiscountPercentage = decimal

type Customer =
    | Eligible of RegisteredCustomer
    | Registered of RegisteredCustomer
    | Guest of UnregisteredCustomer

module Customer =
    // Spend -> Customer -> DiscountPercentage
    let calculateDiscountPercentage spend customer : DiscountPercentage = // Pass the instance of the defining type as the last parameter
        match customer with
        | Eligible _ -> if Spend.value spend >= 100.0M then 0.1M else 0.0M
        | _ -> 0.0M

    // Customer -> Spend -> Total
    let calculateTotal customer spend : Total =
        let discountPercentage = customer |> calculateDiscountPercentage spend
        Spend.value spend * (1.0M - discountPercentage)

    // Customer -> Spend -> Total
    let calculateTotal' customer spend : Total =
        customer
        |> calculateDiscountPercentage spend
        |> fun discountPercentage -> Spend.value spend * (1.0M - discountPercentage)


let john = Eligible { Id = "John" }
let mary = Eligible { Id = "Mary" }
let richard = Registered { Id = "Richard" }
let sarah = Guest { Id = "Sarah" }

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
let assertSarah2 = assertEqual sarah 1001M 1001M

let test = Spend.create 10000000M