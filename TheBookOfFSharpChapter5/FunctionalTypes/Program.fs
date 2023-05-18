open System
// Functional Types
// Tuples, records and discriminated unions are typically assoicated with functional programming

// Tuples: Expressed as coma delimited list, sometimes enclosed in parentheses
let point1 = 10.0, 10.0
let point2 = (20.0, 20.0)
let point : float * float = 0.0, 0.0 // Signature for a tuple includes the type of each value separated by an asterisk

// Extracting values:
let slope p1 p2 =
    let x1 = fst p1
    let y1 = snd p1
    let x2 = fst p2
    let y2 = snd p2
    (y1 - y2) / (x1 - x2)

slope (13.0, 8.0) (1.0, 2.0) |> printfn "%f"


// Tuple Pattern: Specify an identifier for each value in a tuple by separating the identifiers with commas
let slopeTuplePattern p1 p2 =
    let x1, y1 = p1
    let x2, y2 = p2
    (y1 - y2) / (x1 - x2)

slopeTuplePattern (13.0, 8.0) (1.0, 2.0) |> printfn "%f"

// Patterns can be included in the signiture
let slopeSig (x1, y1) (x2, y2) = (y1 - y2) / (x1 - x2)
slopeSig (13.0, 8.0) (1.0, 2.0) |> printfn "%f"

//Tuples are considered equal when the corresponding component values in each instance are the same
printfn "%b" <| ((1,2) = (1,2))
printfn "%b" <| ((1,2) = (2,1))

// Out parameters: F# compiler converts the return value and out parameter to a pair (Tuple with 2 values)
let r, v = System.Int32.TryParse "10"
printfn "%b %i" r v




// Records: Group values in a single immutable construct with value equality and custom functionality
type color = { R: byte; G: byte; B : byte }
             member x.ToHexString() = sprintf "#%02X%02X%02X" x.R x.G x.B // Records can have additional members
             static member Red = { R = 255uy; G = 0uy; B = 0uy } // Member can be static
             static member Blue = { R = 0uy; G = 0uy; B = 255uy }
             static member Green = { R = 0uy; G = 255uy; B = 0uy }
             static member (+) (l: color, r: color) =
                { R = Math.Min(255uy, l.R + r.R)
                  G = Math.Min(255uy, l.G + r.G)
                  B = Math.Min(255uy, l.B + r.B) }
        

// Semicolons can be omitted when each pair is on a seperate line:
type rgbColor2 = 
    { R: byte
      G: byte
      B: byte }

// Creating a record
let red = { R = 255uy; G = 0uy; B = 0uy }

// Extracting/Accessing values
let rbColorToHex  (c: rgbColor2) =
    sprintf "#%02X%02X%02X" red.R red.G red.B

printfn "%s" <| rbColorToHex red

// Avoiding naming conflicts - two records with the same definitions (color and rgbColor2)
let blue = { color.R = 0uy; G = 0uy; B = 255uy }
printf "%s" <| blue.ToHexString()

// Copy and update record expression
let red2 = { R = 255uy; G = 0uy; B = 0uy }
let yellow = { red2 with G = 255uy }

// Mutable records
type mutRgbColor =
    { mutable R : byte
      mutable G : byte
      mutable B : byte }

let mutColor = { mutRgbColor.R = 255uy; G = 255uy; B = 255uy }
mutColor.G <- 100uy