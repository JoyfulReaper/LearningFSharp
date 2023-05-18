// Currying: Automatic function chaining
// F# function accept exactly one input and have exactly one output

// Add accpets an integer and retuns a function that accpets an interger and returns an integer
// val add: a: int -> b:int -> int
let add a b = a + b

// Re-write to more closely resemble the compiled code:
let add2 a = fun b -> (+) a b



// Partial Application
// Creating new functions from existing ones by supplying some of the arguments
let add10 = add 10;

printfn "%i" (add10 5)