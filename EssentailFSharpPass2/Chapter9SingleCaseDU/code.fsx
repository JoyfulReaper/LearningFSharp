type RegisteredCustomer = {
    Id : string
}

type UnregisteredCustomer = {
    Id : string
}

type ValidationError =
    | InputOutOfRange of string
    
type Spend = Spend of decimal
    with
        member this.Value = this |> fun (Spend value) -> value
        static member Create input =
            if input >= 0.0M && input <= 1000.0M then
                Ok (Spend input)
            else
                Error (InputOutOfRange "You can only spend between 0 and 1000")

type Customer =
    | Eligible of RegisteredCustomer
    | Registered of RegisteredCustomer
    | Guest of UnregisteredCustomer
    with
        member this.CalculateDiscountPercentage(spend:Spend) =
            match this with
            | Eligible _ ->
                if spend.Value >= 100.0M then 0.1M
                else 0.0M
            | _ -> 0.0M

// Using Type Aliases
type Spend' = decimal
type Total' = decimal

// Customer -> Spend -> Total
let calculateTotal (customer:Customer) (spend:Spend') : Total' = 
    let discount = 
        match customer with
        | Eligible _ when spend >= 100.0M -> spend * 0.1M
        | _ -> 0.0M
    spend - discount

//// Alternative - Using type alias for function signature ////

type CalculateTotal = Customer -> Spend' -> Total'

let calculateTotal' : CalculateTotal =
    fun customer spend ->
        let discount = 
            match customer with
            | Eligible _ when spend >= 100.0M -> spend * 0.1M
            | _ -> 0.0M
        spend - discount

//// Using Single Case Discrimanated Unions ////


let calculateTotalDU (customer:Customer) (spend:Spend) : Total' =
    let (Spend value) = spend // Deconstruct a Discriminated Union
    let discount = 
        match customer with
        | Eligible _ when value >= 100.0M -> value * 0.1M
        | _ -> 0.0M
    value - discount

let calculateTotalDU' (customer:Customer) (spend: Spend) = // Deconstucting the DU in in the function parameter
    spend.Value * (1.0M - customer.CalculateDiscountPercentage spend)

let john = Eligible { Id = "John" }
let mary = Eligible { Id = "Mary" }
let richard = Registered { Id = "Richard" }
let sarah = Guest { Id = "Sarah" }

// 'a -> 'a -> bool
let isEqualTo expected actual =
    actual = expected

let assertEqual customer spent expected =
    Spend.Create spent
    |> Result.map (fun spend -> calculateTotalDU' customer spend)
    |> isEqualTo (Ok expected)

let assertJohn = assertEqual john 100.0M 90.0M
let assertMary = assertEqual mary 99.0M 99.0M
let assertRichard = assertEqual richard 100.0M 100.0M
let assertSarah = assertEqual sarah 100.0M 100.0M


///////////////////////////////

// The problem with type abbreviations
// If the underlying type matches then anything can be used for input, not just the limited range of values we want
// Nothing stops you from supplying an invalid value or wrong value to a parameter
type Latitude = decimal
type Longitude = decimal

type GpsCoordinate = { Latitude : Latitude; Longitude : Longitude }

// Latitude -90 to 90
// Longitude -180 to 180
let badGps : GpsCoordinate = { Latitude = 1000M; Longitude = -345M }

let latitude = 46M
let longitude = 15M

// Swap latitude and longitude
let badGps2 : GpsCoordinate = { Latitude = longitude; Longitude = latitude }