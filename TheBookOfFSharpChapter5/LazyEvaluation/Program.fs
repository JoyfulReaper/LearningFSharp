// Lazy Evaluation: Defer evaluation until the result is actually needed
//  By default F# uses eager evaluation, meaning an expression will be evaluated immediately

let lazyOperation = lazy (printfn "evaluating lazy expression"
                          System.Threading.Thread.Sleep(1000)
                          42)

lazyOperation.Force() |> printfn "Result: %i"