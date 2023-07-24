open System

type ILogger =
    abstract member Info : string -> unit
    abstract member Error : string -> unit

type Logger() =
    interface ILogger with
        member _.Info(msg) = printfn "Info: %s" msg
        member _.Error(msg) = printfn "Error: %s" msg


// Object Expression - Creates annonymous type
let logger = {
    new ILogger with
        member _.Info(msg) = printfn "Info: %s" msg
        member _.Error(msg) = printfn "Error: %s" msg
}

let doSomethingElse (logger:ILogger) input =
    logger.Info $"Processing {input} at {DateTime.UtcNow.ToString()}"
    ()

doSomethingElse logger "MyData"

type MyClass(logger:ILogger) =
    let mutable count = 0

    member _.DoSomething input =
        logger.Info $"Processing {input} at {DateTime.UtcNow.ToString()}"
        count <- count + 1

    member _.Count = count

let myClass = MyClass(logger)
[1..10] |> List.iter myClass.DoSomething
printfn "%i" myClass.Count