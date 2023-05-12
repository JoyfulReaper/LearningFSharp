// Fields
// Mutable let binding used to define backing field for Name property
type Person() =
    let mutable name : string = ""
    member x.Name
        with get() = name
        and set(v) = name <- v

// Expicit Fields (Public by default)
// If the class doesn't have a primary constructor or for more control
// Create an explicit field with val keyword
// In classes with a primary constructor use the DefaultValue attribute
// to ensure the values are appropriately initialized
type Person2() =
    [<DefaultValue>] val mutable n : string
    [<DefaultValue>] val mutable private a : int
    member x.Name
        with get() = x.n
        and set(v) = x.n <- v


//Explicit Properties
type Person3() = 
    let mutable name = ""
    member x.Name
        with get() = name
        and set(value) = name <- value

// Alternate Proptery Syntax
// access modifiers can be added after with or and
type Person3a() =
    let mutable name = ""
    member x.Name with get() = name
    member x.Name with set(value) = name <- value

// Implicit properties
// Must appear before other member definitions
// defined via member val keyword
// must be initialized to a default value
type Person4() =
    member val Name = "" with get, set


// Read only implicit property
type Person4a(name) =
    member val Name = name

