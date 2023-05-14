open System
open System.Collections.Generic

// Abstract Class
[<AbstractClass>]
type Node(name: string, ?content : Node list) =
    member x.Name = name
    member x.Content = content

// Abstract Properties
[<AbstractClass>]
type AbstractBaseClass() =
    abstract member SomeData : string with get,set

// Abstract Class subtypes
type BindingBackedClass() =
    inherit AbstractBaseClass()
    let mutable someData = ""
    override x.SomeData
        with get() = someData
        and set(v) = someData <- v

type DictionaryBackedClass() =
    inherit AbstractBaseClass()
    let dict = System.Collections.Generic.Dictionary<string, string>()
    [<Literal>]
    let SomeDataKey = "SomeData"
    override x.SomeData
        with get() =
            match dict.TryGetValue(SomeDataKey) with
            | true, v -> v
            | _, _ -> ""
        and set(v) =
            match System.String.IsNullOrEmpty(v) with
            | true when dict.ContainsKey(SomeDataKey) ->
                dict.Remove(SomeDataKey) |> ignore
            | _ -> dict.[SomeDataKey] <- v


// Abstract Methods
[<AbstractClass>]
type Shape() =
    abstract member GetArea : unit -> float

type Circle(r : float) =
    inherit Shape()
    member val Radius = r
    override x.GetArea() =
        Math.Pow(Math.PI * r, 2.0)

type Rectangle(w: float, h : float) =
    inherit Shape()
    member val Width = w
    member val Height = h
    override x.GetArea() = w * h

// Virtual Members
type NodeV(name : string) =
    let children = List<Node>()
    member x.Children with get() = children.AsReadOnly()
    abstract member AddChild : Node -> unit     // Define abstract member
    abstract member RemoveChild : Node -> unit
    default x.AddChild(n) = children.Add n      // Provide default implmentation with the default keyword
    default x.RemoveChild(n) = children.Remove n |> ignore

type TerminalNode(name : string) =
    inherit NodeV(name)
    [<Literal>]
    let notSupportedMessage = "Cannot add or remove children"
    override x.AddChild(n) =
        raise (NotSupportedException(notSupportedMessage))
    override x.RemoveChild(n) =
        raise (NotSupportedException(notSupportedMessage))