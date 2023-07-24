type IFizzBuzz =
    abstract member Calculate : int -> string

type FizzBuzz(mapping) =
    let calculate n =
         mapping
        |> List.map (fun (v,s) -> if n % v = 0 then s else "")
        |> List.reduce (+)
        |> fun s -> if s = "" then string n else s

    interface IFizzBuzz with
        member __.Calculate(value) = calculate value
       

let fizzBuzz = FizzBuzz([(3, "Fizz");(5, "Buzz")])
let fifteen = (fizzBuzz :> IFizzBuzz).Calculate(15)

let doFizzBuzz mapping range =
    let fizzBuzz = FizzBuzz(mapping) :> IFizzBuzz // Upcast to IFizzBuzz
    range
    |> List.map fizzBuzz.Calculate

let doFizzBuzz' mapping range =
    let fizzBuzz = FizzBuzz(mapping)
    range
    |> List.map (fun n -> (fizzBuzz :> IFizzBuzz).Calculate(n)) // Interface function

let output = doFizzBuzz [(3, "Fizz");(5, "Buzz")] [1..15]

