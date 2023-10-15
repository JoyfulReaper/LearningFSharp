open System

type ILogger =
    abstract member Info : string -> unit
    abstract member Error : string -> unit
    
 // Creating a class that implements the interface
type Logger() =
    interface ILogger with
        member _.Info(msg) = printfn "INFO: %s" msg
        member _.Error(msg) = printfn "ERROR: %s" msg
        
// Using an object expression to create an instance of the interface
let logger = 
    { new ILogger with
        member _.Info(msg) = printfn "INFO: %s" msg
        member _.Error(msg) = printfn "ERROR: %s" msg }
    
let doSomethingElse  (logger:ILogger) input =
    logger.Info $"Processing {input} at {DateTime.UtcNow.ToString()}"
    ()
    
doSomethingElse logger "Hello World"
    
type MyClass(logger: ILogger) =
    let mutable count = 0
    
    member _.DoSomething input =
        logger.Info $"Processing {input} at {DateTime.UtcNow.ToString()}"
        count <- count + 1
        ()
        
    member _.Count = count
    
let myClass = MyClass(logger)
[1..10] |> List.iter myClass.DoSomething
printfn "%i" myClass.Count