open System.IO
open System
open System.Diagnostics

try
    use file = File.OpenText "somefile.txt"
    file.ReadToEnd() |> printfn "%s"
with
| :? FileNotFoundException as ex -> printfn "%s was not found" ex.FileName
| :? PathTooLongException
| :? ArgumentNullException
| :? ArgumentException -> printfn "Invalid filename"
| _ -> printf "Error Loading File"



// Reraise() and common pattern where the try block returns 
// Some<_> and each exception case returns None

let fileContents = 
    try
        use file = File.OpenText "somefile.txt"
        Some <| file.ReadToEnd()
    with
        | :? FileNotFoundException as ex ->
            printfn "%s was not found" ex.FileName
            None
        | _ -> 
            printfn "Error Loading File"
            reraise()


// Raising Exceptions
let filename = "x"
if not (File.Exists filename) then
    raise <| FileNotFoundException("filename was null or empty")


// Try Finally
let fileContents2 = 
    try
        use file = File.OpenText "somefile.txt"
        Some <| file.ReadToEnd
    finally
        printfn "Cleaning up..."
        

//Failwith (raise Microsoft.FSharp.Core.FailureException)
if not (File.Exists filename) then
    failwith "File not found"

// With F# string formatting
if not (String.IsNullOrEmpty filename) then
    failwithf "File %s not found" filename

// Invalid arg
if not (String.IsNullOrEmpty filename) then
    invalidArg "filename" (sprintf "%s is not a valid filename" filename)

// Custom Exceptions
type MyException(message, category) =
    inherit exn(message)
    member x.Category = category
    override x.ToString() = sprintf "[%s] %s" x.Category x.Message

exception RetryAttemptFailed of string * int
exception RetryCountExceeded of string