open System
open System.Drawing
// Operators


// Prefix operators
type  RgbColor(r, g, b) =
    member x.Red = r
    member x.Green = g
    member x.Blue = b
    override x.ToString() = sprintf "(%i %i %i)" r g b
    // negate a color
    static member (~-) (r : RgbColor) = // Custom prefix operator
        RgbColor(
            r.Red ^^^ 0xFF,
            r.Green ^^^ 0xFF,
            r.Blue ^^^ 0xFF)


let yellow = RgbColor(255, 255, 0)
let blue = -yellow

printfn $"Yellow: {yellow} Blue: {blue}"


// Infix Operators
type  RgbColor2(r, g, b) =
    member x.Red = r
    member x.Green = g
    member x.Blue = b
    override x.ToString() = sprintf "(%i %i %i)" r g b
    // negate a color
    static member (~-) (r : RgbColor2) = // Custom prefix operator
        RgbColor(
            r.Red ^^^ 0xFF,
            r.Green ^^^ 0xFF,
            r.Blue ^^^ 0xFF)
    static member (+) (l : RgbColor2, r : RgbColor2) = // infix operator
        RgbColor2(
            Math.Min(255, l.Red + r.Red),
            Math.Min(255, l.Green + r.Green),
            Math.Min(255, l.Blue + r.Blue))
    static member (-) (l : RgbColor2, r : RgbColor2) =
        RgbColor2(
            Math.Min(255, l.Red - r.Red),
            Math.Min(255, l.Green - r.Green),
            Math.Min(255, l.Blue - r.Blue))

let red2 = RgbColor2(255, 0, 0)
let green2 = RgbColor2(0, 255, 0)
let blue2 = RgbColor2(0, 0, 255)

let magenta = RgbColor2(255, 0, 255)
printfn $"magenta {magenta} - red {red2} = {magenta - red2}"


// Global operators: Allow new operators for type you don't control:
let (+)(l : Color)(r : Color) =
    Color.FromArgb(
        255,
        Math.Min(255, int <| l.R + r.R),
        Math.Min(255, int <| l.G + r.G),
        Math.Min(255, int <| l.B + r.B))