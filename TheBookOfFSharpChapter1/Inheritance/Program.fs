// Inheritance
type BaseType() =
    member x.SayHello name =
        printfn "Hello, %s" name

type DerivedType() =
    inherit BaseType()

let dt = DerivedType()
dt.SayHello "Fred"


// Practical Inheritance Example:
type WorkItem(summary : string, desc : string) =
    member val Summary = summary
    member val Description = desc
    override x.ToString() = sprintf "%s" x.Summary // Overriding members

type Defect(summary, desc, severity : int) =
    inherit WorkItem(summary, desc)
    member val Severity = severity
    override x.ToString() = sprintf "%s (%i)" (base.ToString()) x.Severity // Access the base class implemtation with base keyword

type Enhancement(summary, desc, requestedBy : string) =
    inherit WorkItem(summary, desc)
    member val requestedBy = requestedBy

// Casting
// Upcasting (derived type to base type) with the static cast operator :>
let w = Defect("Incompatiblity Detected", "Delete", 1) :> WorkItem

// Downcasting (base type to derived type) with the dynamic cast operator :?>
let d = w :?> Defect

printfn "%s" (w.ToString())