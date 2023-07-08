module part1

// Tuple (AND type)
type Customer = string * bool * bool

let fred = ("Fred", true, true)
let frank : Customer = ("Frank", true, true) // Explicitly stating the type

// Decomposing a tuple
let (id, isEligible, isRegistered) = fred


// Record (AND type)
type CustomerRecord = { Id : string; IsEligible : bool; IsRegistered : bool }

let sam = { Id = "Sam"; IsEligible = true; IsRegistered = true }

let samantha : CustomerRecord = { Id = "Samantha"
                                  IsEligible = true 
                                  IsRegistered = true }

// Input parameters are in curried form (if function signiture has more then on arrow, you have curried parameters)
let calculateTotal (customer: CustomerRecord) (spend: decimal) : decimal =
    let discount =
        if customer.IsEligible && spend >= 100M
        then (spend * 0.1M) else 0.0M
    let total = spend - discount
    total

// Input parameters are in tupled form
let calculateTotal2 (customer: CustomerRecord, spend: decimal) : decimal =
    let discount =
        if customer.IsEligible && spend >= 100M
        then (spend * 0.1M) else 0.0M
    let total = spend - discount
    total

// Compiler type inference: Types not required in signiture
let calculateTotal3 customer spend =
    let discount =
        if customer.IsEligible && spend >= 100M
        then (spend * 0.1M) else 0.0M
    spend - discount

let john = { Id = "John"; IsEligible = true; IsRegistered = true}
let mary = { Id = "Mary"; IsEligible = true; IsRegistered = true}
let richard = { Id = "Richard"; IsEligible = false; IsRegistered = true}
let sarah = { Id = "Sarah"; IsEligible = false; IsRegistered = false}

let areEqual expected actual = // Generic function
    actual = expected

let assertJohn = (calculateTotal3 john 100.0M = 90.0M) // Paraentheses are not required here
let assertMary = (calculateTotal3 mary 99.0M = 99.0M)
let assertRichard = (calculateTotal3 richard 100.0M = 100.0M)
let assertSarah = (calculateTotal3 sarah 100.0M = 100.0M)

let assertJohn2 = areEqual 90.0M (calculateTotal3 john 90.0M)
let assertMary2 = areEqual 99.0M (calculateTotal3 mary 99.0M)
let assertRichard2 = areEqual 100.0M (calculateTotal3 richard 100.0M)
let assertSarah2 = areEqual 100.0M (calculateTotal3 sarah 100.0M)

let mutable myInt = 0
myInt = 1 // Equality Check
myInt <- 1 // Assignment
myInt = 1 // Equality Check