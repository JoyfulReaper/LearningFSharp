// Instance Methods
open System

// Define instance methods with the member keyword
type Circle (diameter : float) =
    member x.Diameter = diameter
    member x.GetArea() =  // Access modifiers can be added after the member keyword
        let r = diameter / 2.0
        Math.PI * (r ** 2.0)

let c = Circle(5.0)
printfn "%f" (c.GetArea())

// Let bindings can also be used to define private methods
type Circle2 (diameter : float) =
    let getRadius() = diameter / 2.0
    member x.Diameter = diameter
    member x.GetArea() =
        Math.PI * ((getRadius()) ** 2.0)


// Overloaded methods
type Repository() =
    member x.Commit(files, desc, branch) =
        printfn "Committing %i files (%s) to \"%s\"" (List.length files) desc branch
    member x.Commit(files, desc) =
        x.Commit(files, desc, "default")

// Optional parameters
type Repository2() =
    static member Commit(files, desc, ?branch) =
        let targetBranch = defaultArg branch "default"
        printfn "Committed %i files (%s) to \"%s\"" (List.length files) desc targetBranch

// Slice Expressions
// To use slice expressions in your class you first need to 
// implment the GetSlice() method
type Sentence(initial: string) =
    let words = initial.Split ' '
    let text = initial
    member x.GetSlice(lower, upper) =
        match defaultArg lower 0 with
        | l when l >= words.Length -> Array.empty<string>
        | l -> match defaultArg upper (words.Length - 1) with
            | u when u >= words.Length -> words.[l..]
            | u -> words.[l..u]

let s = Sentence "Don't forget to drink your Bud Light"
s[1..3] |> Seq.iter (fun word -> printf "%s " word)
printfn ""
s[..3] |> Seq.iter (fun word -> printf "%s " word)
printfn ""
s[3..] |> Seq.iter (fun word -> printf "%s " word)