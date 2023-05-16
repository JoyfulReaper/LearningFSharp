// Object Expressions

type IWeapon =
    abstract Description : string with get
    abstract Power : int with get

type Character(name : string, maxHP : int) =
    member x.Name = name
    member val HP = maxHP with get,set
    member val Weappon : IWeapon option = None with get, set
    member x.Attack(o : Character) =
        let power = match x.Weappon with
                    | Some(w) -> w.Power
                    | None -> 1 // Fists
        o.HP <- System.Math.Max(0, o.HP - power)
    override x.ToString() =
        sprintf "%s: %i/%i" name x.HP maxHP


let witchKing = Character("Witch-king", 100)
let frodo = Character("Frodo", 50)

// The object expression
let forgeWeapon  desc power =
    { new IWeapon with
        member x.Description with get() = desc
        member x.Power with get() = power
      interface System.IDisposable with // Implementing multiple base types in an object expression (pg 99)
        member x.Dispose() = printfn "Disposing..." }

let morgulBlade = forgeWeapon "Morgul-Blade" 25
let sting = forgeWeapon "Sting" 10

witchKing.Weappon <- Some(morgulBlade)
frodo.Weappon <- Some(sting)


// Type Extensions
module ColorExtensions =
    open System
    open System.Drawing
    open System.Text.RegularExpressions

    // Rexex to parse ARGB component from hex string
    let private hexPattern =
        Regex("^#(?<color>[\dA-F]{8})$", RegexOptions.IgnoreCase ||| RegexOptions.Compiled)

    // Type Extension
    type Color with
        static member FromHex(hex) =
            match hexPattern.Match hex with
            | matches when matches.Success ->
              Color.FromArgb <| Convert.ToInt32(matches.Groups.["color"].Value, 16)
            | _ -> Color.Empty
        member x.ToHex() = sprintf "#%02X%02X%02X%02X" x.A x.R x.G x.B