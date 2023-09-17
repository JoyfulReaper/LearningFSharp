open System
open System.IO
open System.Text.RegularExpressions
open FsToolkit.ErrorHandling.ValidationCE

type Customer = {
    CustomerId : string
    Email : string
    IsEligible : string
    IsRegistered : string
    DateRegistered : string
    Discount : string
}

type ValidatedCustomer = {
    CustomerId : string
    Email : string option
    IsEligible : bool
    IsRegistered: bool
    DateRegistered : DateTime option
    Discount : decimal option
}

type ValidationError = 
    | MissingData of name: string
    | InvalidData of name: string * value: string

type FilReader = string -> Result<string seq, exn>

let readFile : FilReader =
    fun path ->
        try
            seq {
                use reader = new StreamReader(File.OpenRead(path))
                while not reader.EndOfStream do
                    reader.ReadLine()
            }
            |> Ok
        with
        | ex -> Error ex

let parseLine (line:string) : Customer option =
    match line.Split('|') with
    | [| customerId; email; eligigle; registered; dateRegistered; discount |] ->
        Some {
            CustomerId = customerId
            Email = email
            IsEligible = eligigle
            IsRegistered = registered
            DateRegistered = dateRegistered
            Discount = discount
        }
    | _ -> None

let (|ParseRegex|_|) regex str =
    let m = Regex(regex).Match(str)
    if m.Success then Some (List.tail [for x in m.Groups -> x.Value])
    else None

let (|IsValidEmail|_|) input =
    match input with
    | ParseRegex ".*?@(.*)" [ _ ] -> Some input
    | _ -> None

let (|IsEmptyString|_|) (input:string) =
    if input.Trim() = "" then Some() else None

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

// string -> Result<string, ValidationError>
let validateCustomerId customerId =
    if customerId <> "" then Ok customerId
    else Error (MissingData "CustomerId")

// string -> Result<string option, ValidationError>
let validateEmail email =
    if email <> "" then
        match email with
        | IsValidEmail _ -> Ok (Some email)
        | _ -> Error (InvalidData ("Email", email))
    else
        Ok None

// string -> Result<bool, ValidationError>
let validateIsEligible (isEligigle:string) =
    match isEligigle with
    | IsBoolean b -> Ok b
    | _ -> Error (InvalidData ("IsElibible", isEligigle))

// string -> Result<bool, ValidationError>
let validateIsRegistered (isRegistered:string) =
    match isRegistered with
    | IsBoolean b -> Ok b
    | _ -> Error (InvalidData ("IsRegistered", isRegistered))

// string -> Result<DateTime option, ValidationError>
let validateDateRegistered (dateRegistered:string) = 
    match dateRegistered with
    | IsEmptyString -> Ok None
    | IsValidDate dt -> Ok (Some dt)
    | _ -> Error (InvalidData ("DateRegistered", dateRegistered))

// string -> Result<decimal option, ValidationError>
let validateDiscount discount =
    match discount with
    | IsEmptyString -> Ok None
    | IsDecimal value -> Ok (Some value)
    | _ -> Error (InvalidData ("Discount", discount))

// Result<'a, ValidationError> -> ValidationError list
let getError input =
    match input with
    | Ok _ -> []
    | Error ex -> [ex]

// Result<'a, ValidationError> -> 'a
let getValue input =
    match input with
    | Ok value -> value
    | Error _ -> failwith "Oop, yoiu shouldn't have got here!"

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
    let customerId = input.CustomerId |> validateCustomerId
    let email = input.Email |> validateEmail
    let isEligible = input.IsEligible |> validateIsEligible
    let isRegistered = input.IsRegistered |> validateIsRegistered
    let dateRegistered = input.DateRegistered |> validateDateRegistered
    let discount = input.Discount |> validateDiscount
    let errors = 
        [ 
            customerId |> getError;
            email |> getError;
            isEligible |> getError;
            isRegistered |> getError;
            dateRegistered |> getError;
            discount |> getError
        ] 
        |> List.concat
    match errors with
    | [] -> Ok (create (customerId |> getValue) (email |> getValue) (isEligible |> getValue) (isRegistered |> getValue) (dateRegistered |> getValue) (discount|> getValue))
    | _ -> Error errors


let validate' (input:Customer) : Result<ValidatedCustomer, ValidationError list> =
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

        return create customerId email isEligible isRegistered dateRegistered discount // called only if there are no errors
    }


(*
let validate' (input:Customer) : Result<ValidatedCustomer, ValidationError list> =
    // Alternate -- pretend each validate function returns a list of validation errors
    let customerId = input.CustomerId |> validateCustomerId
    let email = input.Email |> validateEmail 
    let isEligible = input.IsEligible |> validateIsEligible
    let isRegistered = input.IsRegistered |> validateDateRegistered
    let dateRegistered = input.IsRegistered |> validateIsRegistered
    let discount = input.Discount |> validateDiscount

    create customerId email isEligible isRegistered dateRegistered discount
*)

let parse (data: string seq) =
    data
    |> Seq.skip 1
    |> Seq.map parseLine
    |> Seq.choose id
    |> Seq.map validate'

let output data =
    data
    |> Seq.iter (fun x -> printfn "%A" x)

let import (fileReader:FilReader) path =
    match path |> fileReader with
    | Ok data ->
        data 
        |> parse 
        |> output
    | Error ex ->
        printfn "Error: %A" ex.Message


[<EntryPoint>]
let main argv =
    Path.Combine(__SOURCE_DIRECTORY__, "resources", "customers.csv")
    |> import readFile
    0