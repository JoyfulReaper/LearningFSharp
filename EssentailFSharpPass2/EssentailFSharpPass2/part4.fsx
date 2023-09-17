// Alternate Approach 1/2 - Using a DU as a type in a property of a record type

type CustomerType =
    | Registered of IsEligible : bool
    | Guest

type Customer = { Id : string; Type : CustomerType}


// Customer -> decimal -> decimal
let calculateTotal customer spend =
    let discount =
        match customer.Type with
        | Registered (IsEligible = true) when spend >= 100M ->  // Extract IsRegistered to isRegistered
            spend * 0.1M
        | _ -> 0.0M
    spend - discount


let john = { Id = "John"; Type = Registered (IsEligible = true) }
let mary = { Id = "Mary"; Type = Registered (IsEligible = true) }
let richard = { Id = "Richard"; Type = Registered (IsEligible = false) }
let sarah = { Id = "Sarah"; Type = Guest }

let assertJohn = calculateTotal john 100.0M = 90.0M
let assertMary = calculateTotal mary 99.0M = 99.0M
let assertRichard = calculateTotal richard 100.0M = 100.0M
let assertSarah = calculateTotal sarah 100.0M = 100.0M