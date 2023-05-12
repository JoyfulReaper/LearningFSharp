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


// Indexed Propteries
type Sentence(initial: string) =
    let mutable words = initial.Split(' ')
    let mutable text = initial
    member x.Item
        with get i = words.[i]
        and set i v =
            words[i] <- v
            text <- System.String.Join(" ", words)
    member x.Chars with get(i) = text.[i] // Any property can be an indexed property (Unlike C#)

let s = Sentence "Don't forget to drink your Ovaltine"
printfn "%s" s.[1] // Access with dot notation
s.[1] <- "remember"
printfn "%s" s.[1]

printfn "%c" (s.Chars(0)) //Dot notation can only be used default indexed properties; access like a method

// Setting properties as part of the constructor call
type Person5() =
    member val Name = "" with get, set

let p = Person5(Name = "Dave")