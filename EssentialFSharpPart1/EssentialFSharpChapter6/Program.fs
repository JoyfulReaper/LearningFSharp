open System
open System.IO

type Customer = {
    CustomerId : string
    Email : string
    IsEligible : string
    IsRegistered : string
    DateRegistered : string
    Discount : string
}

type DataReader = string -> Result<string seq, exn>

(* Bug because seq is lazy evaluated
let readFile path = 
    try
        seq {  //Sequence Expression
            use reader = new StreamReader(File.OpenRead(path))
            while not reader.EndOfStream do
                reader.ReadLine()
        }
        |> Ok
    with
    | ex -> Error ex
*)

// string -> Result<seq<string>, exn>
let readFile : DataReader =
    fun path ->
        try
            File.ReadLines(path)
            |> Ok
        with
        | ex -> Error ex

// string -> Customer option
let parseLine (line:string) : Customer option =
    match line.Split('|') with
    | [| customerId; email; eligigle; registered; dateRegisitered; discount |] ->
        Some {
            CustomerId = customerId
            Email = email
            IsEligible = eligigle
            IsRegistered = registered
            DateRegistered = dateRegisitered
            Discount = discount
        }
    | _ -> None

let parse (data:string seq) =
    data
    |> Seq.skip 1 // Ignore Header Row
    |> Seq.map parseLine
    |> Seq.choose id // Ignore None and unwrap Some

let output data =
    data
    |> Seq.iter(fun x -> printfn "%A" x)

let import (dataReader:DataReader) path =
    match path |> dataReader with
    | Ok data -> data |> parse |> output
    | Error ex -> printfn "Error: %A" ex.Message

let importWithFileReader = import readFile

let fakeDataReader : DataReader =
    fun _ ->
        seq {
            "CustomerId|Email|Eligible|Registered|DateRegistered|Discount"
            "Frank|john@test.com|1|1|2015-01-23|0.1"
            "Mary|mary@test.com|1|1|2018-12-12|0.1"
            "Richard|richard@nottest.com|0|1|2016-03-23|0.0"
            "Sarah||0|0||"
        }
        |> Ok

[<EntryPoint>]
let main argv =
    Path.Combine(__SOURCE_DIRECTORY__, "resources", "customers.csv")
    |> importWithFileReader
    //|> import fakeDataReader
    0