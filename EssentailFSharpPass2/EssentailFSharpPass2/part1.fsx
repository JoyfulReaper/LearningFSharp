// Simple but naive solution

type CustomerTuple = string * bool * bool
let fred = ("Fred", true, true)

let (id, isEligible, isRegistered) = fred

type Customer = { Id: string; IsEligible: bool; IsRegistered: bool}

//// Customer -> decimal -> decimal
//let calculateTotal (customer : Customer) (spend : decimal) : decimal =
//    let discount =
//        if customer.IsEligible && spend >= 100.0M
//        then (spend * 0.1M) else 0.0M
//    let total = spend - discount
//    total

// Customer -> decimal -> decimal
let calculateTotal customer spend =
    let discount =
        if customer.IsEligible && spend >= 100.0M then spend * 0.1M
        else 0.0M
    spend - discount

// 'a -> 'a -> bool
let areEqual expected actual =
    actual = expected

let john = {Id = "John"; IsEligible = true; IsRegistered = true}
let mary = {Id = "Mary"; IsEligible = true; IsRegistered = true}
let richard = {Id = "Richard"; IsEligible = false; IsRegistered = true}
let sarah = {Id = "Sarah"; IsEligible = false; IsRegistered = false}

let assertJohn = (calculateTotal john 100.0M = 90.0M)
let assertMary = (calculateTotal mary 99.0M = 99.0M)
let assertRichard = (calculateTotal richard 100.0M = 100.0M)
let assertSarah = (calculateTotal sarah 100.0M = 100.0M)

let assertJohn2 = areEqual 90M (calculateTotal john 100.0M)
let assertMary2 = areEqual 99M (calculateTotal mary 99.0M)
let assertRichard2 = areEqual 100M (calculateTotal richard 100.0M)
let assertSarah2 = areEqual 100M (calculateTotal sarah 100.0M)

