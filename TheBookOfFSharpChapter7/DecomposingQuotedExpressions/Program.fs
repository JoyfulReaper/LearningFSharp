// Decomposing Quoted Expressions - pg 194

open System.Text
open Microsoft.FSharp.Quotations.Patterns
open Microsoft.FSharp.Quotations.DerivedPatterns
open Microsoft.FSharp.Quotations.ExprShape

let rec showSyntax =
    function
    | Int32 v ->
        sprintf "%i" v
    | Value (v, _) ->
       sprintf "%s" (v.ToString())
    | SpecificCall <@@ (+) @@> (_, _, exprs) ->
        let left = showSyntax exprs.Head
        let right = showSyntax exprs.Tail.Head
        sprintf "%s + %s" left right
    | SpecificCall <@@ (-) @@> (_, _, exprs) ->
        let left = showSyntax exprs.Head
        let right = showSyntax exprs.Tail.Head
        sprintf "%s - %s" left right
    | Call (opt, mi, exprs) ->
        let owner = match opt with
                    | Some expr -> showSyntax expr
                    | None -> sprintf "%s" mi.DeclaringType.Name
        if exprs.IsEmpty then
            sprintf "%s.%s" owner mi.Name
        else
            let sb = StringBuilder(showSyntax exprs.Head)
            exprs.Tail
            |> List.iter (fun expr ->
                            sb
                              .Append(",")
                              .Append(showSyntax expr) |> ignore)
            sprintf "%s.%s (%s)" owner mi.Name (sb.ToString())
    | ShapeVar var ->
        sprintf "%A" var
    | ShapeLambda (p, body) ->
        sprintf "fun %s -> %s" p.Name (showSyntax body)
    | ShapeCombination (o, exprs) ->
        let sb = StringBuilder()
        exprs |> List.iter (fun expr -> sb.Append(showSyntax expr) |> ignore)
        sb.ToString()

showSyntax <@ fun x y -> x + y @> |> printfn "%s"


// Subsituting Reflection page 197
module Validation =
    open System
    open Microsoft.FSharp.Quotations
    open Microsoft.FSharp.Quotations.Patterns

    type Test<'e> = | Test of ('e -> (string * string) option)

    let private add (quote : Expr<'x>) message args validate (xs: Test<'e> list) =
        let propName, eval =
            match quote with
            | PropertyGet (_, p, _) -> p.Name, fun x-> p.GetValue(x, [||])
            | Value (_, ty) when ty = typeof<'e> -> "x", box
            | _ -> failwith "Unsupported Expression"
        let test entity =
            let value = eval entity
            if validate (unbox value) then None
            else Some (propName, String.Format(message, Array.ofList (value :: args)))
        Test(test) :: xs

    let notNull quote =
        let validator = (fun v -> v <> null)
        add quote "Is a required field" [] validator

    let notEmpty quote =
        add quote "Cannot be empty" [] (String.IsNullOrWhiteSpace >> not)

    let between quote min max =
        let validator = (fun v -> v >= min && v <= max)
        add quote "Must be at least {2} and greater than {1}" [min; max] validator

    let createValidator (f : 'e -> Test<'e> list -> Test<'e> list) =
        let entries = f Unchecked.defaultof<_> []
        fun entity -> List.choose (fun (Test test) -> test entity) entries

open Validation
type TesType = { ObjectValue : obj
                 StringValue : string
                 IntValue : int }

let validate = 
    createValidator <| fun x -> notNull <@ x.ObjectValue @>
                                >> notEmpty <@ x.StringValue @>
                                >> between <@ x.IntValue @> 1 100

let test1 = { ObjectValue = obj(); StringValue = "Sample"; IntValue = 35 }
let test2 = { ObjectValue = null; StringValue = ""; IntValue = 1000 }

validate test1 |> printfn "%A"
validate test2 |> printfn "%A"