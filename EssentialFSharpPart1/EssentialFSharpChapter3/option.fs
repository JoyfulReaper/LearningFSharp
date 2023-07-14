namespace Option
////// Null Handling //////////
module Options =
    open System

    (*If you are wondering how DateTime.TryParse returns a tuple, the answer is quite simple; the F#
    language does not support out parameters, so the clever folks behind F# made it so that during
    interop, out parameters are added to the function output, which generally results in a tuple.*)
    // string -> Option<DateTime>
    let tryParseDateTime (input : string) =
        let (success, value) = DateTime.TryParse input
        if success then Some value else None

    let tryParseDateTime2 (input : string) =
        match DateTime.TryParse input with
        | true, result -> Some result
        | false, _ -> None

    let isDate = tryParseDateTime "2019-08-01"
    let isNotDate = tryParseDateTime "Hello"

    type PersonName = {
        FirstName : string
        MiddleName : Option<string> // Can also just use string option
        LastName : string
    }

    let person = { FirstName = "Ian"; MiddleName = None; LastName = "Russell" }
    let person2 = { person with MiddleName = Some "????" }

    /// Interop with .NET

    // Reference Type
    let nullObj:string = null

    // Nullable type
    let nullPri = Nullable<int>()

    let fromNullObj = Option.ofObj nullObj
    let fromNullPri = Option.ofNullable nullPri

    // Default values - pg 42
    let resultPM input =
        match input with
        | Some value -> value
        | None -> "------"

    let resultDV = Option.defaultValue "------" fromNullObj

    let resultFP = fromNullObj |> Option.defaultValue "------"

    let resultFPA =
        fromNullObj
        |> Option.defaultValue "------"

    // (string option -> string) - Using Partial Application
    let setUnknownAsDefault = Option.defaultValue "????"
    let result = fromNullObj |> setUnknownAsDefault