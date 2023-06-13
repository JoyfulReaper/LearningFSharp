// Async and Parallel

open System
open System.Threading.Tasks
open System.Collections.Concurrent
open System.Threading.Tasks

Parallel.For(0, 100, printfn "%i") |> ignore 

// With locking - Negate benefits of parallelizing
Parallel.For(0, 100, fun n-> lock Console.Out (fun () -> printfn "%i" n)) |> ignore

// This function compositon elminates the need for locking pg 232
Parallel.For(0, 100, (sprintf "%i") >> Console.WriteLine) |> ignore

// Short circuting
let shortCircuitExample shortCircut =
    let bag = ConcurrentBag<_>()
    Parallel.For(0, 999999, fun i s -> if i < 10000 then bag.Add i else shortCircut s) |> ignore
    (bag, bag.Count)

shortCircuitExample (fun s -> s.Stop()) |> printfn "%A"
shortCircuitExample (fun s -> s.Break()) |> printfn "%A"

// Cancelling
let parallelForWithCancellation (wait: int) =
    use tokenSource = new System.Threading.CancellationTokenSource(wait)

    try 
        Parallel.For(
        0,
        Int32.MaxValue,
        ParallelOptions(CancellationToken = tokenSource.Token),
        fun (i : int) -> Console.WriteLine i) |> ignore
    with
    | :? OperationCanceledException -> printfn "Cancelled!"
    | ex -> printfn "%O" ex

parallelForWithCancellation 1

