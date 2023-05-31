// Measures

// Defining a measure
[<Measure>] type foot



// Measure Formulas:
[<Measure>] type ft = foot

// You can multiply measures by separating them with a space (or asterisk)
[<Measure>] type lb
[<Measure>] type lbft = lb ft

// You can divide measures by separating them with a slash
[<Measure>] type m
[<Measure>] type h
[<Measure>] type mph = m / h

// Expressing exponential relastionship
[<Measure>] type sqft = ft ^ 2

// Applying a measure to a value
let length = 10.0<ft>
let area = 10.0<sqft>

// Converting measureless values to measures
[<Measure>] type dpi
let resolution = 300.0 * 1.0<dpi>

let resolution2 = LanguagePrimitives.FloatWithMeasure<dpi>(300.0)

// Converting measures to measureless values
let noMeasure = 300.0<dpi> / 1.0<dpi>
let noMeasure2 = float 300.0<dpi>

// Enforcing Measures:
let getArea  (w: float<ft>) (h: float<ft>) = w * h
let getArea2  (w: float<ft>) (h: float<ft>) : float<sqft> = w * h

// Ranges -- Must provide step value
let measuredRange = [1.0<ft> .. 1.0<ft> .. 10.0<ft>]

// Static conversion factors
[<Measure>] type inch = static member perFoot = 12.0<inch/ft>
let twentyFourInches = 2.0<ft> * inch.perFoot

let twoFeet = 24.0<inch> / inch.perFoot

// Static conversion functions -- Mutually recursive
[<Measure>]
type f =
    static member toCelsius (t: float<f>) = ((float t - 32.0) * (5.0/9.0)) * 1.0<c>
    static member fromCelsius (t: float<c>) = ((float t * (9.0/5.0)) + 32.0) * 1.0<f>
and
    [<Measure>]
    c =
        static member toFarenheit = f.fromCelsius
        static member fromFarenheit = f.toCelsius

// Or using intrinsic type extensions
[<Measure>] type f2
[<Measure>] type c2

let fahrenheitToCelsius (t: float<f>) = 
    ((float t - 32.0) * (5.0/9.0)) * 1.0<c>

let celsiusToFahrenheit (t: float<c>) = 
    ((float t * (9.0/5.0)) + 32.0) * 1.0<f>

type f2 with
    static member toCelsius (t: float<f2>) = fahrenheitToCelsius
    static member fromCelsius (t: float<c2>) = celsiusToFahrenheit

type c2 with
    static member toFarenheit = celsiusToFahrenheit
    static member fromFarenheit = fahrenheitToCelsius

// Custom Measure Aware Types - Define generic type with a type parameter decorated with the Measure attribute
type Point<[<Measure>] 'u> = { X: float<'u>; Y: float<'u> } with
    member this.FindDistance other =
        let deltaX = other.X - this.X
        let deltaY = other.Y - this.Y
        sqrt ((deltaX * deltaX) + (deltaY * deltaY))

let p = { X = 10.0<inch>; Y = 10.0<inch> }
let distance = p.FindDistance { X = 20.0<inch>; Y = 15.0<inch> }

printfn "Distance: %f<inch>" distance