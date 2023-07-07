module part3 =
    type RegisteredCustomer =
        { Id: string }

    type UnregisteredCustomer =
        { Id: string }

    // Discriminated Union
    type Customer =
        | Eligible of RegisteredCustomer
        | Registered of RegisteredCustomer
        | Guest of UnregisteredCustomer

    let calculateTotal customer spend = // Additional Simplification
        let discount =
            match customer with
            | Eligible _ when spend >= 100.0M -> spend * 0.1M
            | _ -> 0.0M
        spend - discount

    let john = Eligible { Id = "John" }
    let mary = Eligible { Id = "Mary" }
    let richard = Registered { Id = "Richard" }
    let sarah = Guest { Id = "Sarah" }

    let areEqual expected actual = // Generic function
        actual = expected

    let assertJohn = (calculateTotal john 100.0M = 90.0M) // Paraentheses are not required here
    let assertMary = (calculateTotal mary 99.0M = 99.0M)
    let assertRichard = (calculateTotal richard 100.0M = 100.0M)
    let assertSarah = (calculateTotal sarah 100.0M = 100.0M)
