open System

let tryParseDateTime (input:string) =
    let (success,value) = DateTime.TryParse input
    if success then Some value else None

let tryParseDateTime' (input:string) =
    match DateTime.TryParse input with
    | true, result -> Some result
    | _ -> None

let isDate = tryParseDateTime "2015-01-01"
let isNotDate = tryParseDateTime "not a date"

type PersonName = {
    FirstName: string
    MiddleName: string option
    LastName: string
}

let person = { FirstName = "Ian"; MiddleName = None; LastName = "Griffiths" }
let person2 = { person with MiddleName = Some "Alistair" }

////////////////////////

let nullObj:string = null
let nullPri = Nullable<int>()

// Converting between nulls and options
let fromNullObj = Option.ofObj nullObj
let fromNullPri = Option.ofNullable nullPri

let toNullObj = Option.toObj fromNullObj
let toNullPri = Option.toNullable fromNullPri

let resultPM input =
    match input with
    | Some value -> value
    | None -> "------"

let resultDV = Option.defaultValue "------" fromNullObj
let x = Some "test" |> resultPM
let y = None |> resultPM

let resultFP = fromNullObj |> Option.defaultValue "------"

let setUnknownAsDefault = Option.defaultValue "????"

let result = setUnknownAsDefault fromNullObj