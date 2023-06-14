// Async work flows

open System
open System.IO
open System.Net

type StreamReader with
    member x.AsyncReadToEnd () =
        async { do! Async.SwitchToNewThread()
                let content = x.ReadToEnd()
                do! Async.SwitchToThreadPool()
                return content }

let getPage (uri : Uri) =
    async {
        let req = WebRequest.Create uri
        use! response = req.AsyncGetResponse()
        use stream = response.GetResponseStream()
        use reader = new StreamReader(stream)
        return! reader.AsyncReadToEnd() 
    }

async {
    let! content = Uri "http://nostarch.com" |> getPage
    content.Substring(0, 50) |> printfn "%s"
} |> Async.Start


// StartWithContinuations
Async.StartWithContinuations(
    getPage(Uri "http://nostarch.com"),
    (fun c -> c.Substring(0, 50) |> printfn "%s..."),
    (printfn "Exception: %O"),
    (fun _ -> printfn "Cancelled")
)

// Running multiple workflows in parallel and waiting for the result
open System.Text.RegularExpressions;;

[| getPage(Uri "http://nostarch.com")
   getPage(Uri "http://kgivler.com")
   getPage(Uri "http://google.com") |]
|> Async.Parallel
|> Async.RunSynchronously
|> Seq.iter (fun c -> let sample = c.Substring(0, 500)
                      Regex.Replace(sample, @"[\r\n]| {2,}", "")
                      |> printfn "%s...");;

// Async.CancelDefaultToken() - Cancelling async workflow using the default token