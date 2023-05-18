// Recursive Functions
// Methods in a type are implicitly recursive
// let bound functions need the rec keyword

let rec factorial v =
    match v with | 1L -> 1L
                 | _ -> v * factorial (v - 1L)


// Tail-Call recursion (The code in the book seems wrong, this seems correct per the stacktraces...)
let tcFactorial v =
    let rec fact c p =
        match c with
        | 0L -> p
        | _ -> fact (c - 1L) (c * p)
    fact v 1L

printfn "%i" <| factorial 5
printfn "%i" <| tcFactorial 5


// Mutually Recursive Functions
// When two or more functions call each other recursivly 
let fibonacci n =
    let rec f = function
                | 1 -> 1
                | n -> g (n - 1)
    and g = function
            | 1 -> 0
            | n -> g (n - 1) + f (n - 1)
    f n + g n

let fib = fibonacci 6
printfn "%i" fib


// Lambda Expression
// begins with fun keyword, omitting an identifier, then uses the '->' token instead of '='
// Example of an inline lambda expression im,mediately evaluated
(fun degreesF -> (degreesF - 32.0) * (5.0 / 9.0)) 212.0 |> printfn "%f"


// Closures
let createCounter() =
    let count = ref 0
    (fun () -> count.Value <- count.Value + 1
               count.Value)

let increment = createCounter()
for i in [1..10] do printfn "%i" <| increment()


let increment2 times incrementFunc =
    let rec inc2 times =
        match times with
            | 0 -> ()
            | _ ->
                printfn "%i" <| incrementFunc()
                inc2 (times - 1)
    inc2 times

increment2 3 increment