// Static Initializer
type ClassWithStaticCtor() =
    static let mutable staticField = 0
    static do
        printfn "Invoking static initilizer"
        staticField <- 10
    do printfn "Static Field Value: %i" staticField

// Static fields
module Logger =
    let private log l c m = printfn "%-5s [%s] %s" l c m
    let LogInfo = log "INFO"
    let LogError = log "Error"

    type MyService() =
        static let logCategory = "MyService"
        member x.DoSomething() =
            LogInfo logCategory "Doing Something"
        member x.DoSomethingElse() =
            LogError logCategory "Doing Something Else"

    let svc = MyService()
    svc.DoSomething()
    svc.DoSomethingElse()

// Static Properties
// Readonly static property
type Processor() =
    static let mutable itemsProcessed = 0
    static member ItemsProcessed = itemsProcessed
    member x.Process() =
        itemsProcessed <- itemsProcessed + 1
        printfn "Processing..."
   
while Processor.ItemsProcessed < 5 do (Processor()).Process()


// Static Members 
// see page 90
// static member CreateReader(filename) = ...

// Mutually Recursive Types
open System.Collections.Generic
type Book() =
    let pages = List<Page>()
    member x.Pages with get() = pages.AsReadOnly()
    member x.AddPage (pageNumber : int, page : Page) =
        if page.Owner = Some(x) then failwith "Page is already part of a book"
        pages.Insert(pageNumber - 1, page)
and Page (content : string) =
    let mutable owner : Book option = None
    member x.Content = content
    member x.Owner with get() = owner
    member internal x.Owner with set(v) = owner <- v
    override x.ToString() = content
