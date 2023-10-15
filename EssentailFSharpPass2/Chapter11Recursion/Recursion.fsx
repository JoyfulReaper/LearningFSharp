let rec fact n =
    match n with
    | 1 -> 1
    | n -> n * fact (n - 1)
    
let fact' n =
    let rec loop n acc =
        match n with
        | 1 -> acc
        | _ -> loop (n - 1) (acc * n)
    loop n 1
    
    
// Naive, slow implementation
let rec fib (n:int64) =
    match n with
    | 0L -> 0L
    | 1L -> 1L
    | s -> fib (s - 1L) + fib (s - 2L)
    
// Tail recursive, fast implementation
let fib' n =
    let rec loop n (a,b) =
        match n with
        | 0L -> a
        | 1L -> b
        | n -> loop (n - 1L) (b, a+b)
    loop n (0L, 1L)
    
// Solving FizzBuzz using recursion
let mapping = [(3, "Fizz"); (5, "Buzz")]

let fizzBuzz initialMapping n =
    let rec loop mapping acc =
        match mapping with
        | [] -> if acc = "" then string n else acc
        | head::tail ->
            let value =
                head |> (fun (div, msg) -> if n % div = 0 then msg else "")
            loop tail (acc + value)
    loop initialMapping ""
    
[ 1 .. 105 ]
|> List.map (fizzBuzz mapping)
|> List.iter (printfn "%s")

// Solving FizzBuzz using List.fold
let fizzBuzz' n =
    [(3, "Fizz"); (5, "Buzz")]
    |> List.fold (fun acc (div, msg) ->
        if n % div = 0 then acc + msg else acc) ""
    |> fun s -> if s = "" then string n else s
    
[1..105]
|> List.iter (fizzBuzz' >> printfn "%s")


// Quick Sort using Recursion
let rec qsort input =
    match input with
    | [] -> []
    | head::tail ->
        let smaller, larger = List.partition (fun n -> head >= n) tail
        List.concat [qsort smaller; [head]; qsort larger]

[5;9;5;2;7;9;1;1;3;5] |> qsort |> printfn "%A"
