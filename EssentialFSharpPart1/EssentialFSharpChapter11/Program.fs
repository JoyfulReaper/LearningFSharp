// naive implementation
let rec fact n =
    match n with
    | 1 -> 1
    | n -> n * fact (n - 1)

// Tail Call Optimisation - Using an accumulator
let factTail n =
    let rec loop n acc =
        match n with
        | 1 -> acc
        | _ -> loop (n-1) (acc * n)
    loop n 1

// Navie implementation
let rec fib (n:int64) =
    match n with 
    | 0L -> 0L
    | 1L -> 1L
    | s -> fib (s - 1L) + fib (s - 2L)

let test = fib 50L

// Tail call Optimisation - Using an accumulator
let fibTail (n:int64) =
    let rec loop n (a,b) =
        match n with
        | 0L -> a
        | 1L -> b
        | n -> loop (n-1L)(b, a+b)
    loop n (0L, 1L)

let test2 = fibTail 50L

let mappings = [(3, "Fizz");(5, "Buzz")]

let fizzBuzz initialMapping n =
    let rec loop mapping acc =
        match mapping with
        | [] -> if acc = "" then string n else acc
        | head::tail ->
            let value =
                head |> (fun (div, msg) -> if n % div = 0 then msg else "")
            loop tail (acc + value)
    loop initialMapping ""

[1 .. 105]
|> List.map (fizzBuzz mappings)
|> List.iter (printfn "%s")

let fizzBuzzFold n =
    [(3, "Fizz"); (5, "Buzz")]
    |> List.fold (fun acc (div, msg) ->
        if n % div = 0 then acc + msg else acc) ""
    |> fun s -> if s = "" then string n else s

[1..105]
|> List.iter (fizzBuzzFold >> printfn "%s")

let fizzBuzzFold' n =
    [(3, "Fizz"); (5, "Buzz")]
    |> List.fold (fun acc (div, msg) ->
        match (if n % div = 0 then msg else "") with
        | "" -> acc
        | s -> if acc = string n then s else acc + s) (string n) 

[1..105]
|> List.iter (fizzBuzzFold' >> printfn "%s")

let rec qsort input =
    match input with
    | [] -> []
    | head::tail ->
        let smaller, larger = List.partition (fun n -> head >= n) tail
        List.concat [qsort smaller; [head]; qsort larger]

[5;9;5;2;7;9;1;1;3;5] |> qsort |> printfn "%A"


    