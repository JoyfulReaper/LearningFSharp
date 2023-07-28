open ComputationExpression.OptionDemo
open ComputationExpression.AsyncDemo
open ComputationExpression.AsyncResultDemoTests
open System.IO

(*
[<EntryPoint>]
let main argv =
    calculate 8 0 |> printfn "calculate 8 0 = %A" // None
    calculate 8 2 |> printfn "calculate 8 2 = %A" // Some 16
    0
*)

(*
[<EntryPoint>]
let main argv =
    Path.Combine(__SOURCE_DIRECTORY__, "resources", "customers.csv")
    |> getFileInformation
    |> Async.RunSynchronously // Force async code to run, should only be done at application entry point
    |> printfn "%A"
    0
*)

[<EntryPoint>]
let main argv =
    printfn "Success: %b" success
    printfn "BadPassword: %b" badPassword
    printfn "InvalidUser: %b" invalidUser
    printfn "IsSuspended: %b" isSuspended
    printfn "IsBanned: %b" isBanned
    printfn "HasBadLuck: %b" hasBadLuck
    0