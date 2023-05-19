// Discriminated Unions: user-defined data types whose values are restricted to a known set of values called union cases.

// Discriminated unions enforce a specific set of values (pg 123)
let showValue (v : _ option) =
    printfn "%s" (match v with 
                  | Some x -> x.ToString()
                  | None -> "None")

Some 123 |> showValue
Some "abc" |> showValue
None |> showValue


// Defining Discriminated Unions
// Begin with type keyword, Union cases delmited with bar, Union case names must start with an Uppercase letter to help the compiler differentiate union cases from other identifiers in pattern matching
// Typically used for Representing simple object hierarchies, Representing tree structures, Replacing type abbereviations

// Simple Object Hierarchies
// Can be used as a subsitute from formal classes and inheritance

// OO Environment solution:
type IShape = interface end

type Circle(r : float) =
    interface IShape
    member x.Radius = r

type Rectangle(w : float, h : float) =
    interface IShape
    member x.Width = w
    member x.Heigh = h

type Triangle (l1 :float, l2 : float, l3: float) =
    interface IShape
    member x.Leg1 = l1 
    member x.Leg2 = l2
    member x.Leg3 = l3

// Discriminated Union Alternative
type Shape =
/// Describe circle by radius
| Circle of float         
/// Describe Rectangle by width and height
| Rectangle of float * float        
/// Describe triangle by its three sides
| Triangle of float * float * float 

let c = Circle(3.0)
let r = Rectangle(10.0, 12.0)
let t = Triangle(25.0, 20.0, 7.0)

// Named union type fields
type Shape2 =
| Circle2 of Radius : float
| Rectangle2 of Width : float * Height: float
| Triangle2 of Leg1 : float * Leg2 : float * Leg3 : float

// Can now also use named arguments
let c2 = Circle2(Radius = 3.0)
let r2 = Rectangle2(Width = 10.0, Height = 12.0)
let t2 = Triangle2(Leg1 = 25.0, Leg2 = 20.0, Leg3 = 7.0)


// Tree structures

// Self-refencing -> Data associated with a union case can be another case from the same union
type Markup =
| ContentElement of string * Markup list
| EmptyElement of string
| Content of string

let movieList =
  ContentElement("html",
    [ ContentElement("head", [ ContentElement("title", [ Content "Guilty Pleasures" ])])
      ContentElement("body",
        [ ContentElement("article",
            [ ContentElement("h1", [ Content "Some Guilty Pleasures" ])
              ContentElement("p",
                [ Content "These are "
                  ContentElement("strong", [ Content "a few" ])
                  Content " of my guilty pleasures" ])
              ContentElement("ul",
                [ ContentElement("li", [ Content "Crank (2006)" ])
                  ContentElement("li", [ Content "Starship Troopers (1997)" ])
                  ContentElement("li", [ Content "RoboCop (1987)" ])])])])])

let rec toHtml markup =
    match markup with
    | ContentElement (tag, children) ->
        use w = new System.IO.StringWriter()
        children
            |> Seq.map toHtml
            |> Seq.iter (fun (s : string) -> w.Write(s))
        sprintf "<%s>%s</%s>" tag (w.ToString()) tag
    | EmptyElement (tag) -> sprintf "<%s />" tag
    | Content (c) -> sprintf "%s" c

let html = movieList |> toHtml
printfn "%s" html


// Replacing Type Abbrevations
open System.IO

type HtmlString = string // Type abbrevation

let displayHtml (html : HtmlString) =
    let fn = Path.Combine(Path.GetTempPath(), "HtmlDemo.htm")
    let bytes = System.Text.UTF8Encoding.UTF8.GetBytes html
    using (new FileStream(fn, FileMode.Create, FileAccess.Write))
          (fun fs -> fs.Write(bytes, 0, bytes.Length))
    System.Diagnostics.Process.Start($"C:\\Program Files\\Mozilla Firefox\\firefox.exe", fn).WaitForExit() // Doesn't work right...
    File.Delete fn

movieList |> toHtml |> displayHtml

// For more type safety, replace HtmlString definition with a single case discriminated union:
type HtmlString2 = | HtmlString2 of string

// Need to change the function to extract the associated string
// Change the functions signiture to  include an Identifer pattern
let displayHtml2 (HtmlString2(html)) =
    let fn = Path.Combine(Path.GetTempPath(), "HtmlDemo.htm")
    let bytes = System.Text.UTF8Encoding.UTF8.GetBytes html
    using (new FileStream(fn, FileMode.Create, FileAccess.Write))
          (fun fs -> fs.Write(bytes, 0, bytes.Length))
    System.Diagnostics.Process.Start($"C:\\Program Files\\Mozilla Firefox\\firefox.exe", fn).WaitForExit() // Doesn't work right...
    File.Delete fn

// Wrap the string from toHtml in an HtmlString instance and pass to displayHtml2
HtmlString2(movieList |> toHtml) |> displayHtml2


// Revise toHtml to return an HtmlString2
let rec toHtml2 markup =
    match markup with
    | ContentElement (tag, children) ->
        use w = new System.IO.StringWriter()
        children
            |> Seq.map toHtml2
            |> Seq.iter (fun (HtmlString2(html)) -> w.Write(html))
        HtmlString2 (sprintf "<%s>%s</%s>" tag (w.ToString()) tag)
    | EmptyElement (tag) -> HtmlString2 (sprintf "<%s />" tag)
    | Content (c) -> HtmlString2 (sprintf "%s" c)