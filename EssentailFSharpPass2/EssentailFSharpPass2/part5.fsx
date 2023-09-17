// Alternative Approach 2/2

type Customer =
    | Registered of Id : string * IsEligible : bool
    | Guest of Id : string

// Active Pattern
let (|IsEligible|_|) customer =
    match customer with
    | Registered (IsEligible = true) -> Some ()
    | _ -> None

// Customer -> decimal -> decimal
let calculateTotal customer spend =
    let discount =
        match customer with
        | Registered (IsEligible = true) when spend >= 100M ->
            spend * 0.1M
        | _ -> 0.0M
    spend - discount

// Customer -> decimal -> decimal - If the Id property is needed it can be extracted when applying IsEligible = true filter
let calculateTotal' customer spend =
    let discount =
        match customer with
        | Registered (Id = id; IsEligible = true) when spend >= 100.0M -> spend * 0.1M
        | _ -> 0.0M
    spend - discount


// Customer -> decimal -> decimal
let calculateTotalActivePattern customer spend =
    let discount =
        match customer with
        | IsEligible when spend >= 100M ->
            spend * 0.1M
        | _ -> 0.0M
    spend - discount

let john = Registered (Id = "John", IsEligible = true)
let mary = Registered (Id = "Mary", IsEligible = true)
let richard = Registered (Id = "Richard", IsEligible = false)
let sarah = Guest (Id = "Sarah")

let assertJohn = (calculateTotal john 100.0M = 90.0M)
let assertMary = (calculateTotal mary 99.0M = 99.0M)
let assertRichard = (calculateTotal richard 100.0M = 100.0M)
let assertSarah = (calculateTotal sarah 100.0M = 100.0M)
