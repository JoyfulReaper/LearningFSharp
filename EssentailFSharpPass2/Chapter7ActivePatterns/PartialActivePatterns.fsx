open System

// Parsing a date with a function
let parse (input:string) =
    match DateTime.TryParse(input) with
    | true, value -> Some value
    | false, _ -> None
    
let isDate = parse "2019-12-20"
let isNotDate = parse "Hello"


// Active Patterns
// Partial Active Patterns are Useful for validation and parsing

// Partial Active Pattern - Logic could also have been written using an if expression
let (|ValidDate|_|) (input:string) =
    match DateTime.TryParse(input) with
    | true, value -> Some value
    | false, _ -> None
    
    
let parse' input =
    match input with
    | ValidDate dt -> printfn "%A" dt // The parsed date "dt" is accessable because it is being returned from the active pattern
    | _ -> printfn $"'%s{input}' is not a valid date"
    
parse' "2019-12-20"
parse' "Hello"

// Parsed date is not returned from the active pattern
let (|IsValidDate|_|) (input:string) = 
    let success, _ = DateTime.TryParse input
    if success then Some () else None
    
let isValidDate input =
    match input with
    | IsValidDate -> true
    | _ -> false
    
    
    
    
// Parameterized Partial Active Patterns

// Fizz Buzz Pattern Matching 1
let calculate1 i =
    if i % 3 = 0 && i % 5 = 0 then "FizzBuzz"
    elif i % 3 = 0 then "Fizz"
    elif i % 5 = 0 then "Buzz"
    else i |> string
    
[1..15] |> List.map calculate1

// Fizz Buzz Pattern Matching 2
let calculate2 i =
    match (i % 3, i % 5) with
    | (0,0) -> "FizzBuzz"
    | (0, _) -> "Fizz"
    | (_, 0) -> "Buzz"
    | _ -> i |> string
    
// Fizz Buzz Pattern Matching 3
let calculate3 i =
    match (i % 3 = 0, i % 5 = 0) with
    | (true, true) -> "FizzBuzz"
    | (true, _) -> "Fizz"
    | (_, true) -> "Buzz"
    | _ -> i |> string
    
// Using a parameterized  partial active pattern
let(|IsDivisibleBy|_|) divisor n =
    if n % divisor = 0 then Some () else None
    
let calulateAP1 i =
    match i with
    | IsDivisibleBy 3 & IsDivisibleBy 5 -> "FizzBuzz"
    | IsDivisibleBy 3 -> "Fizz"
    | IsDivisibleBy 5 -> "Buzz"
    | _ -> i |> string
    
let calculate i =
    match i with
    | IsDivisibleBy 3 & IsDivisibleBy 5 & IsDivisibleBy 7 -> "FizzBuzzBazz"
    | IsDivisibleBy 3 & IsDivisibleBy 5 -> "FizzBuzz"
    | IsDivisibleBy 3 & IsDivisibleBy 7 -> "FizzBazz"
    | IsDivisibleBy 5 & IsDivisibleBy 7 -> "BuzzBazz"
    | IsDivisibleBy 3 -> "Fizz"
    | IsDivisibleBy 5 -> "Buzz"
    | IsDivisibleBy 7 -> "Bazz"
    | _ -> i |> string
    
[1..30] |> List.map calculate

// Isaac Abraham - FizzBuzz with Parameterized Active Patterns
let (|IsDivisibleBy2|_|) divisors n =
    if divisors |> List.forall (fun div -> n % div = 0)
    then Some ()
    else None
    
let calculate4 i =
    match i with
    | IsDivisibleBy2 [3;5;7] -> "FizzBuzzBazz"
    | IsDivisibleBy2 [3;5] -> "FizzBuzz"
    | IsDivisibleBy2 [3;7] -> "FizzBazz"
    | IsDivisibleBy2 [5;7] -> "BuzzBazz"
    | IsDivisibleBy2 [3] -> "Fizz"
    | IsDivisibleBy2 [5] -> "Buzz"
    | IsDivisibleBy2 [7] -> "Bazz"
    | _ -> i |> string
    
[1..30] |> List.map calculate4

// Another Approach
let calculate5 n =
    [(3, "Fizz"); (5, "Buzz"); (7, "Bazz")]
    |> List.map (fun(divisor, result) -> if n % divisor = 0 then result else "")
    |> List.reduce (+)
    |> fun input -> if input = "" then string n else input
    
[1..30] |> List.map calculate5

// With this approach could also pass the mapping
let calculate6 mapping n =
    mapping
    |> List.map (fun (divisor, result) -> if n % divisor = 0 then result else "")
    |> List.reduce (+)
    |> fun input -> if input = "" then string n else input
    
[1..30] |> List.map (calculate6 [(3, "Fizz"); (5, "Buzz");])


/////// Leap Year
let isLeapYear year =
    year % 400 = 0 || (year % 4 = 0 && year % 100 <> 0)
    
[2000;2001;2020] |> List.map isLeapYear

///// Using a parameterized active pattern
let (|IsDivisibleBy3|_|) divisor n =
    if n % divisor = 0 then Some () else None
    
let (|NotDivisibleBy|_|) divisor n =
    if n % divisor <> 0 then Some () else None
    
let isLeapYearAP year =
    match year with
    | IsDivisibleBy3 400 -> true
    | NotDivisibleBy 4 & NotDivisibleBy 100 -> true // There is logic for & and | but not NOT
    | _ -> false
    
 // Also could have used helper functions
 
let isDivisibleBy divisor year =
    year % divisor = 0
    
let notDivisibleBy divisor year =
    not (year |> isDivisibleBy divisor)
    
let isLeapYear' year =
    year |> isDivisibleBy 400 || (year |> isDivisibleBy 4 && year |> notDivisibleBy 100)
    
// Match expression with guard clause
let isLeapYear'' year =
    match year with
    | year when year |> isDivisibleBy 400 -> true
    | year when year |> isDivisibleBy 4 && year |> notDivisibleBy 100 -> true
    | _ -> false
    
[2000;2001;2020] |> List.map isLeapYear''

// Allows removing the notDivisibleBy function
let isLeapYear''' year =
    match year with
    | year when year |> isDivisibleBy 400 -> true
    | year when year |> isDivisibleBy 4 && year |> isDivisibleBy 100 |> not -> true
    | _ -> false
    
[2000;2001;2020] |> List.map isLeapYear'''



// Multi-Case Active Patterns

type Rank = Ace|Two|Three|Four|Five|Six|Seven|Eight|Nine|Ten|Jack|Queen|King
type Suit = Clubs|Diamonds|Hearts|Spades
type Card = Rank * Suit

let (|Red|Black|) (card:Card) =
    match card with
    | (_, Diamonds) | (_, Hearts) -> Red
    | (_, Clubs) | (_, Spades) -> Black
    
let describeColor card =
    match card with
    | Red -> "Red"
    | Black -> "Black"
    |> printfn "The card is %s"
    
describeColor (Two, Hearts)

// Single Case Active Patterns

let (|CharacterCount|) (input:string) =
    input.Length
    
let (|ContainsANumber|) (input:string) =
    input
    |> Seq.filter Char.IsDigit
    |> Seq.length > 0
    
let (|IsValidPassword|) input =
    match input with
    | CharacterCount len when len < 8 -> (false, "Password must be at least 8 characters")
    | ContainsANumber false -> (false, "Password must contain a number")
    | _ -> (true, "")
    
let setPassword input =
    match input with
    | IsValidPassword (true, _) as pwd -> Ok pwd
    | IsValidPassword (false, msg) -> Error $"Password not set: %s{msg}"
    
let badPassword = setPassword "password"
let goodPassword = setPassword "passw0rd"