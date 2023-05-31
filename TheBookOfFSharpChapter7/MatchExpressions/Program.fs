open System

// Pattern Matching
// Each result expression must be the same type
// Patterns should be listed from most to least specific

let testOption opt =
    match opt with
    | Some(v) -> printfn "Some %i" v
    | None -> printfn "None"

// Guard Clauses: Additional criteria that must be met to satisfy a case
// Identical pattern, but different guard clauses
let testNumber value =
    match value with
    | v when v < 0 -> printfn "%i is negative" v
    | v when v > 0 -> printfn "%i is positive" v
    | _ -> printfn "Zero"

// Combining guards with boolean operators
let testNumber2 value =
    match value with
    | v when v > 0 && v % 2 = 0 -> printfn "%i is positive and even" v
    | v -> printfn "%i is zero, negative, or odd" v

// Pattern matching functions
let testOption2 = 
    function
    | Some(v) -> printfn "Some %i" v
    | None -> printfn "None"

// filter all none values from a list of optional integers using a pattern matching function
let result = 
    [Some 10; Some 4; None; Some 0; Some 7]
    |> List.filter (function | Some(_) -> true | None -> false)

// Variable Patterns
// Matches any value and binds that value to a name
// anything other than 0,2,3, will be bound to n and converted to a string
// Identifer should be lowercase to distinguish from Identifer pattern
let numberToString =
    function
    | 0 -> "zero"
    | 1 -> "one"
    | 2 -> "two"
    | 3 -> "three"
    | n -> sprintf "%O" n


// Wildcard Patterns
// '_' works like Variable pattern, excepts discards value instead of binding it
let numberToString2 =
    function
    | 0 -> "zero"
    | 1 -> "one"
    | 2 -> "two"
    | 3 -> "three"
    | _ -> "unknown"

// Identifer Patterns
// When a pattern consists of more than a single character and begins with an uppercase character, the compiler attempts to resolve it as a name. This is called an Identifier Pattern.

// Matching Union Cases
type Shape =
    | Circle of radius:float
    | Rectangle of width:float * height:float
    | Triangle of side1:float * side2:float * side3:float

let getPerimeter =
    function
    | Circle(r) -> 2.0 * Math.PI * r
    | Rectangle(w, h) -> 2.0 * (w + h)
    | Triangle(s1, s2, s3) -> s1 + s2 + s3

let c1 = Circle(2.0)
let p1 = getPerimeter c1
printfn "Perimeter of circle is %f" p1

// Matching Literals
[<Literal>]
let Zero = 0
[<Literal>]
let One = 1
[<Literal>]
let Two = 2
[<Literal>]
let Three = 3

let numberToString3 =
    function
    | Zero -> "zero"
    | One -> "one"
    | Two -> "two"
    | Three -> "three"
    | _ -> "unknown"

// Matching Nulls
let matchString =
    function
    | null
    | "" -> None
    | v -> Some(v.ToString())

let t = null |> matchString |> printfn "%A"

// Matching Tuples
let locatePoint p =
    match p with
    | (0,0) -> sprintf "%A is at the origin" p
    | (_, 0) -> sprintf "%A is on the x-axis" p
    | (0, _) -> sprintf "%A is on the y-axis" p
    | (x, y) -> sprintf "Point (%i, %i)" x y

// Matching Records
type Name = { First: string; Middle: string option; Last: string }

let formatName =
    function
    | {First = f; Middle = Some(m); Last = l} -> sprintf "%s %s %s" l f m
    | {First = f; Middle = None; Last = l} -> sprintf "%s %s" l f

// Compiler can often automatically infer the type of a record pattern
// If not it can be specifed: Name.Middle = Some(_) -> true
let hasMiddleName =
    function
    | { Middle = Some(_) } -> true
    | { Middle = None } -> false

// Matching Collections
// Array Pattern - Match array with specific number of elements
let getLength =
    function 
    | null -> 0
    | [| |] -> 0
    | [| _ |] -> 1
    | [| _; _ |] -> 2
    | [| _; _; _ |] -> 3
    | a -> a |> Array.length

// List Pattern - Match list with specific number of elements
let getLength2 =
    function
    | [ ] -> 0
    | [_] -> 1
    | [_; _] -> 2
    | [_; _; _] -> 3
    | l -> l |> List.length

// Cons Pattern - Separates a list's head from its tail. Allows recursively match against a list with an arbitrary number of elements.
let getLengthCon n =
    let rec len c l =
        match l with
        | [] -> c
        | _ ::t -> len (c + 1) t
    len 0 n

// Matching by type
// Type-Annotated Pattern: Force each case to match against the same data type
let startsWithUpperCase =
    function
    | (s: string) when s.Length > 0 && System.Char.IsUpper(s.[0]) -> true
    | _ -> false

// Dynamic Type-Test Pattern: Satisfied when the matched instance is a particular type
type RgbColor = { R: int; G: int; B: int }
type CmykColor = { C: int; M: int; Y: int; K: int }
type HslColor = { H: int; S: int; L: int }

let detectColorSpace (cs : obj) =
    match cs with 
    | :? RgbColor -> "RGB"
    | :? CmykColor -> "CMYK"
    | :? HslColor -> "HSL"
    | _ -> failwith "Unrecognized"

// As Patterns:
// Allows you to match against a value and bind it to a name in a single step
let x, y as point = (10, 20)

let locatePoint2 =
    function
    | (0, 0) as p -> sprintf "%A is at the origin" p
    | (_, 0) as p -> sprintf "%A is on the x-axis" p
    | (0, _) as p -> sprintf "%A is on the y-axis" p
    | (x, y) -> sprintf "Point (%i, %i)" x y

// Conjunction Patterns / And Patterns
// Useful for extracting values when another pattern is matched
let locatePoint3 =
    function
    | (0, 0) as p -> sprintf "%A is at the origin" p
    | (x, y) & (_, 0) -> sprintf "(%i, %i) is on the x-axis" x y
    | (x, y) & (0, _) -> sprintf "(%i, %i) is on the y-axis" x y
    | (x, y) -> sprintf "Point (%i, %i)" x y

// Disjunctive Patterns / Or Patterns
// Multiple patterns should run the same code
let locatePoint4 =
    function
    | (0,0) as p -> sprintf "%A is at the origin" p
    | (0, _) | (_, 0) as p -> sprintf "%A is on an axis" p
    | p -> sprintf "Point %A" p


// Active Patterns
// Active patterns are functions that return a tuple of a Boolean and a value. The Boolean indicates whether the pattern matched, and the value is the result of the pattern match.

// Active recognizer pattern
let (|Fizz|Buzz|FizzBuzz|Other|) n =
    match (n % 3, n % 5) with
    | 0, 0 -> FizzBuzz
    | 0, _ -> Fizz
    | _, 0 -> Buzz
    | _ -> Other n

// Pattern matching with active patterns
let fizzBuzz =
   function
    | Fizz -> "Fizz"
    | Buzz -> "Buzz"
    | FizzBuzz -> "FizzBuzz"
    | Other n -> n.ToString()

seq { 1..100 }
|> Seq.map fizzBuzz
|> Seq.iter (printfn "%s")

// Partial Active Patterns
let (|Fizz|_|)n = if n % 3 = 0 then Some() else None
let (|Buzz|_|)n = if n % 5 = 0 then Some() else None

let fizzBuzzPartial =
    function
    | Fizz & Buzz -> "FizzBuzz"
    | Fizz -> "Fizz"
    | Buzz -> "Buzz"
    | n -> n.ToString()

seq { 1..100 }
|> Seq.map fizzBuzzPartial
|> Seq.iter (printfn "%s")

// Parameterized Active Patterns
let (|DivisibleBy|_|) d n = if n % d = 0 then Some DivisibleBy else None

let fizzBuzzParameterized =
    function
    | DivisibleBy 3 & DivisibleBy 5 -> "FizzBuzz"
    | DivisibleBy 3 -> "Fizz"
    | DivisibleBy 5 -> "Buzz"
    | n -> n.ToString()

seq { 1..100 }
|> Seq.map fizzBuzzParameterized
|> Seq.iter (printfn "%s")