open System

let createDisposable name = 
    printfn "Creating: %s" name
    {
        new IDisposable with
        member x.Dispose() =
            printfn "disposing: %s" name
    }


let testDisposable() =
    use root = createDisposable "outer"
    for i in [1..2] do
        use nested = createDisposable (sprintf "inner %i" i)
        printfn "completing iteration %i" i
    printfn "leaving function"

let approximatelyEqual(x: float) (y: float) (threshold: float) =
    Math.Abs(x - y) <=  Math.Abs(threshold)

[<EntryPoint>]
let main argv = 
    testDisposable()
    let result = approximatelyEqual 0.33333 (1.0 / 3.0) 0.001
    printfn "%b" result
    0