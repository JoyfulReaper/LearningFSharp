﻿open System
open System.IO
open System.Text.RegularExpressions
open FsToolkit.ErrorHandling.ValidationCE

type Customer = {
    CustomerId : string
    Email : string
    IsEligible: string
    IsRegistered: string
    DateRegistered: string
    Discount: string
}

type ValidatedCustomer = {
    CustomerId: string
    Email: string option
    IsEligible: bool
    IsRegistered: bool
    DateRegistered: DateTime option
    Discount: decimal option
}

type ValidationError =
    | MissingData of name: string
    | InvalidData of name: string * value: string

type DataReader = string -> Result<string seq, exn>

let readFile : DataReader =
    fun path ->
        try
            File.ReadLines(path)
            |> Ok
        with
        | ex -> Error ex

let parseLine (line:string) : Customer option =
    match line.Split('|') with
    | [|customerId; email; eligible; registered; dateRegistered; discount|] ->
        Some {
            CustomerId = customerId
            Email = email
            IsEligible = eligible
            IsRegistered = registered
            DateRegistered = dateRegistered
            Discount = discount 
        }
    | _ -> None

// Partial Active Patterns for parsing string data
let (|ParseRegex|_|) regex str =
    let m = Regex(regex).Match(str)
    if m.Success then Some (List.tail [for x in m.Groups -> x.Value])
    else None
    
let (|IsValidEmail|_|) input =
    match input with
    | ParseRegex ".*?@(.*)" [_] -> Some input
    | _ -> None
    
let (|IsEmptyString|_|) (input:string) =
    if input.Trim() = "" then Some () else None

let (|IsDecimal|_|) (input:string) =
    let success, value = Decimal.TryParse input
    if success then Some value else None
    
let (|IsBoolean|_|) (input:string) =
    match input with
    | "1" -> Some true
    | "0" -> Some false
    | _ -> None
    
let (|IsValidDate|_|) (input:string) =
    let (success, value) = input |> DateTime.TryParse
    if success then Some value else None
    
let validateCustomerId customerId =
    if customerId <> "" then Ok customerId
    else Error (MissingData "CustomerId")
    
let validateEmail email =
    if email <> "" then
        match email with
        | IsValidEmail _ -> Ok (Some email)
        | _ -> Error (InvalidData ("Email", email))
    else
        Ok None

let validateIsEligible (isEligible:string) =
    match isEligible with
    | IsBoolean b -> Ok b
    | _ -> Error (InvalidData ("IsEligible", isEligible))

let validateIsRegistered (isRegistered:string) =
    match isRegistered with
    | IsBoolean b -> Ok b
    | _ -> Error (InvalidData ("IsRegistered", isRegistered))
    
let validateDateRegistered (dateRegistered:string) =
    match dateRegistered with
    | IsEmptyString -> Ok None
    | IsValidDate dt -> Ok (Some dt)
    | _ -> Error (InvalidData ("DateRegistered", dateRegistered))

let validateDiscount discount =
    match discount with
    | IsEmptyString -> Ok None
    | IsDecimal value -> Ok (Some value)
    | _ -> Error (InvalidData ("Discount", discount))

let getError input =
    match input with
    | Ok _ -> []
    | Error ex -> [ex]
    
let getValue input =
    match input with
    | Ok v -> v
    | _ -> failwith "Oops, you shouldn't have got here!"

let create customerId email isEligible isRegistered dateRegistered discount =
    {
        CustomerId = customerId
        Email = email
        IsEligible = isEligible
        IsRegistered = isRegistered
        DateRegistered = dateRegistered
        Discount = discount
    }
    
let validate (input:Customer) : Result<ValidatedCustomer, ValidationError list> =
    (* If we had used let rather than and, the code would have only returned the first error found and then
    would not have the other lines as it would be on the Error track. The style before 'and' was introduced
    is referred to as 'monadic', as opposed to the 'applicative' style we are now using where the use of and
    forces the code to run all of the validation, even if there are validation errors reported. The final
    function to create a ValidatedCustomer is only called if there are no errors. It ends up doing exactly
    what we did initially but much more elegantly! *)
    validation {
        let! customerId =
            input.CustomerId
            |> validateCustomerId
            |> Result.mapError (fun ex -> [ex])
        and! email =
            input.Email
            |> validateEmail
            |> Result.mapError (fun ex -> [ex])
        and! isEligible =
            input.IsEligible
            |> validateIsEligible
            |> Result.mapError (fun ex -> [ex])
        and! isRegistered =
            input.IsRegistered
            |> validateIsRegistered
            |> Result.mapError (fun ex -> [ex])
        and! dateRegistered =
            input.DateRegistered
            |> validateDateRegistered
            |> Result.mapError (fun ex -> [ex])
        and! discount =
            input.Discount
            |> validateDiscount
            |> Result.mapError List.singleton
        
        return create customerId email isEligible isRegistered dateRegistered discount   
    }

let parse (data:string seq) =
    data
    |> Seq.skip 1
    |> Seq.map parseLine
    |> Seq.choose id
    |> Seq.map validate

let output data =
    data
    |> Seq.iter (printfn "%A")

let import (dataReader:DataReader) path =
    match path |> dataReader with
    | Ok data -> data |> parse |> output
    | Error ex -> printfn "Error: %A" ex.Message    
    
[<EntryPoint>]
let main argv =
    Path.Combine(__SOURCE_DIRECTORY__, "resources", "customers.csv")
    |> import readFile
    0