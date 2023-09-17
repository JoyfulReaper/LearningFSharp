// Making Eligibility a domain concept

type RegisteredCustomer =
    {
        Id: string
    }

type UnregisteredCustomer =
    {
        Id: string
    }

type Customer =
    | Eligible of RegisteredCustomer
    | Registered of RegisteredCustomer
    | Guest of UnregisteredCustomer


// Customer -> decimal -> decimal
let calculateTotal customer spend =
    let discount =
        match customer with
        | Eligible _ when spend >= 100M -> spend * 0.1M
        | _ -> 0.0M
    spend - discount

// 'a -> 'a -> bool
let areEqual expected actual =
    actual = expected

let john = Eligible { Id = "John"; }
let mary = Eligible { Id = "Mary";}
let richard = Registered { Id = "Richard";}
let sarah = Guest { Id = "Sarah" } 

let assertJohn = (calculateTotal john 100.0M = 90.0M)
let assertMary = (calculateTotal mary 99.0M = 99.0M)
let assertRichard = (calculateTotal richard 100.0M = 100.0M)
let assertSarah = (calculateTotal sarah 100.0M = 100.0M)
