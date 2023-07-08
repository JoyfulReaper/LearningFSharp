﻿// Alternative Two

module part5

type Customer =
    | Registered of Id: string * IsEligible:bool  // Looks like a tuple, but is not. Allows lables
    | Guest of Id: string


let calculateTotal customer spend =
    let discount =
        match customer with
        | Registered (id, isEligible) when isEligible && spend >= 100.0M -> spend * 0.1M
        | _ -> 0.0M
    spend - discount

let calculateTotal2 customer spend =
    let discount =
        match customer with
        | Registered (IsEligible = true) when spend >= 100M -> spend * 0.1M
        | _ -> 0.0M
    spend - discount

let john = Registered (Id = "John", IsEligible = true)
let mary = Registered ( Id = "Mary", IsEligible = true)
let richard = Registered ( Id = "Richard", IsEligible = true)
let sarah = Guest ( Id = "Sarah" )

let areEqual expected actual = // Generic function
    actual = expected

let assertJohn = (calculateTotal john 100.0M = 90.0M) // Paraentheses are not required here
let assertMary = (calculateTotal mary 99.0M = 99.0M)
let assertRichard = (calculateTotal richard 100.0M = 100.0M)
let assertSarah = (calculateTotal sarah 100.0M = 100.0M)
