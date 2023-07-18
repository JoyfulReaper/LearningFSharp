// Partial Active Patterns
// Return an option (only subset of possible input values will produce an output)
// Useful for validation and parsing

open System

// Parsing a DateTime using a partial active pattern: (|...|_|)

// string -> DateTime option
let (|ValidDate|_|) (input:string) =
    match DateTime.TryParse(input) with
    | true, value -> Some value
    | fale, _ -> None

// string -> DateTime option
let (|ValidDate2|_|) (input:string) =
    let success, value = DateTime.TryParse(input)
    if success then Some value else None

// string -> DateTime option
let parse input =
    match input with
    | ValidDate dt -> printfn "%A" dt
    | _ -> printfn $"'%s{input}' is not a valid date"

let isDate = parse "2019-12-20"
let isNotDate = parse "Hello"


let (|IsValidDate|_|) (input:string) =
    let success, _ = DateTime.TryParse input
    if success then Some () else None

let isValidDate input =
    match input with
    | IsValidDate -> true
    | _  -> false

// Parameterized Partial Active Patterns

let calculate i =
    if i % 3 = 0 && i % 5 = 0 then "FizzBuzz"
    elif i % 3 = 0 then "Fizz"
    elif i % 5 = 0 then "Buzz"
    else i |> string

[1..15] |> List.map calculate |> List.iter (printfn "%s")


let calculate2 i =
    match(i % 3, i % 5) with
    | (0,0) -> "FizzBuzz"
    | (0, _) -> "Fizz"
    | (_,0) -> "Buzz"
    | _ -> i |> string


// Paramatezied Partial Active Pattern
let (|IsDivisibleBy|_|) divisor n =
    if n % divisor = 0 then Some() else None

let calculate3 i = 
    match i with
    | IsDivisibleBy 3 & IsDivisibleBy 5 -> "FizzBuzz"
    | IsDivisibleBy 3 -> "Fizz"
    | IsDivisibleBy 5 -> "Buzz"
    | _ -> i |> string

let mapping = [(3, "Fizz"); (5, "Buzz");(7, "Bazz")]

let calculate4 mapping n =
    mapping
    |> List.map (fun (divisor, result) -> if n % divisor = 0 then result else "")
    |> List.reduce (+) // (+) is a shortcut for (fun acc v -> acc + v)
    |> fun input -> if input = "" then string n else input

[1..15] |> List.map (calculate4 mapping) |> List.iter (printfn "%s")


//////////////

let isLeapYear year =
    year % 400 = 0 || (year % 4 = 0 && year % 100 <> 0)

let (|NotDivisibleBy|_|) divisor n =
    if n% divisor <> 0 then Some () else None

let isLeapYear2 year =
    match year with
    | IsDivisibleBy 400 -> true
    | IsDivisibleBy 4 & NotDivisibleBy 100 -> true
    | _ -> false

let isDivisibleBy divisor year =
    year % divisor = 0

let notDivisibleBy divisor year =
    not (year |> isDivisibleBy divisor)

let isLeapYear3 year =
    year |> isDivisibleBy 400 || (year |> isDivisibleBy 4 && year |> notDivisibleBy 100)

let isLeapYear4 input =
    match input with
    | year when year |> isDivisibleBy 400 -> true
    | year when year |> isDivisibleBy 4 && year |> notDivisibleBy 100 -> true
    | _ -> false

// Multi-Case Active Patterns:
type Rank = Ace|Two|Three|Four|Five|Six|Seven|Eight|Nine|Ten|Jack|Queen|King
type Suit = Hearts|Clubs|Diamonds|Spades
type Card = Rank * Suit

let (|Red|Black|) (card: Card) =
    match card with
    | (_, Diamonds) | (_, Hearts) -> Red
    | (_, Clubs) | (_, Spades) -> Black

let describeColour card =
    match card with
    | Red -> "Red"
    | Black -> "Black"
    |> printfn "The card is %s"

describeColour (Two, Hearts)

// Single Case Active Patterns
let (|CharacterCount|) (input:string) =
    input.Length

let (|ContainsANumber|) (input:string) =
    input
    |> Seq.filter Char.IsDigit
    |> Seq.length > 0

let (|IsValidPassword|) input =
    match input with
    | CharacterCount len when len < 8 -> (false, "Password must be at least 8 characters.")
    | ContainsANumber false -> (false, "Password must contain at least 1 digit.")
    | _ -> (true, "")

let setPassword input =
    match input with
    | IsValidPassword (true, _) as pwd -> Ok pwd
    | IsValidPassword (false, failureReason) -> Error $"Password not set: %s{failureReason}"

let badPassword = setPassword "password"
let goodPassword = setPassword "passw0rd"

// Practical Example

type Score = int * int

// Score * Score -> option<unit>
let (|CorrectScore|_|) (expected: Score, actual: Score) =
    if expected = actual then Some() else None

let (|Draw|HomeWin|AwayWin|) (score:Score) =
    match score with
    | (h, a) when h = a -> Draw
    | (h, a) when h > a -> HomeWin
    | _ -> AwayWin

let (|CorrectResult|_|) (expected:Score, actual: Score) =
    match (expected, actual) with
    | (Draw, Draw) -> Some ()
    | (HomeWin, HomeWin) -> Some()
    | (AwayWin, AwayWin) -> Some()
    | _ -> None

let goalsScore (expected:Score) (actual:Score) =
    let (h, a) = expected
    let (h', a') = actual
    let home = [h; h'] |> List.min
    let away = [a; a'] |> List.min
    (home * 15) + (away * 20)

let goalScore' (expected:Score) (actual:Score) =
    let home = [ fst expected; fst actual] |> List.min
    let away = [ snd expected; snd actual] |> List.min
    (home * 15) + (away * 20)

let resultScore (expected:Score)(actual:Score) =
    match (expected, actual) with
    | CorrectScore -> 400
    | CorrectResult -> 100
    | _ -> 0

let calculatePoints (expected:Score)(actual:Score)=
    let pointsForCorrectScore =
        match (expected, actual) with
        | CorrectScore -> 300
        | _ -> 0
    let pointsForCorrctResult =
        match (expected, actual) with
        | CorrectResult -> 100
        | _ -> 0
    let pointsForGoals = goalScore' expected actual
    pointsForCorrectScore + pointsForCorrctResult + pointsForGoals

let calculatePoints' (exptected:Score) (actual:Score) =
    let pointsForResult = resultScore exptected actual
    let pointsForGoals = goalScore' exptected actual
    pointsForResult + pointsForGoals

let calculatePoints'' (expected:Score) (actual:Score) =
    [ resultScore; goalScore']
    |> List.sumBy (fun f -> f expected actual)