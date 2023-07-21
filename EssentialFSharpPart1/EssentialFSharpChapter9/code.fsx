type RegisteredCustomer = {
    Id : string
}

type UnregisteredCustomer =  {
    Id : string
}

type ValidationError =
    | InputOutOfRange of string

type Spend = private Spend of decimal
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
                if spend.Value >= 100.0M then 0.1M else 0.0M
            | _ -> 0.0M


// Type Abbreviations
//type Spend = decimal
type Total = decimal

type CalcuateTotal = Customer -> Spend -> Total

// Customer -> Spend -> Total
let calculateTotal (customer: Customer) (spend: Spend) : Total =
    let (Spend value) = spend
    let discount =
        match customer with
        | Eligible _ when value >= 100.0M -> value * 0.1M
        | _ -> 0.0M
    value - discount

// Customer -> Spend -> Total
let calculateTotal' : CalcuateTotal =
    fun customer spend ->
        let (Spend value) = spend
        let discount =
            match customer with
            | Eligible _ when value >= 100.0M -> value * 0.1M
            | _ -> 0.0M
        value - discount

let calculateTotal'' customer (Spend spend) =
    let discount =
        match customer with
        | Eligible _ when spend >= 100.0M -> spend * 0.1M
        | _ -> 0.0M
    spend - discount

// Customer -> Spend -> decimal
let calculateTotal''' (customer:Customer) (spend:Spend) =
    let discount =
        match customer with
        | Eligible _ when spend.Value >= 100.0M -> spend.Value * 0.1M
        | _ -> 0.0M
    spend.Value - discount

let calculateTotal'''' (customer:Customer) (spend:Spend) =
    spend.Value * (1.0M - customer.CalculateDiscountPercentage spend)

let john = Eligible { Id = "John" }
let mary = Eligible { Id = "Mary" }
let richard = Registered { Id = "Richard" }
let sarah = Guest { Id = "Sarah" }

// 'a -> 'a -> bool
let isEqualTo expected actual =
    actual = expected

let assertEqual customer spent expected =
    printfn "Spent: %A" spent
    Spend.Create spent
    |> Result.map (fun spend -> calculateTotal''' customer spend)
    |> isEqualTo (Ok expected)

let assertJohn = assertEqual john 100.0M 90.0M
let assertMary = assertEqual mary 99.0M 99.0M
let assertRichard = assertEqual richard 100.0M 100.0M
let assertSarah = assertEqual sarah 100.0M 100.0M
let assertSarah2 = assertEqual sarah 1001M 1001M

let test = Spend.Create 10000000M