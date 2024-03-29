open System

type Coordinate(latitude: float, longitude: float) =
    member _.Latitude = latitude
    member _.Longitude = longitude

let c1 = Coordinate(25.0, 11.98)
let c2 = Coordinate(25.0, 11.98)

let c3 = c1
c1 = c2 // False
c1 = c3 // True - reference the same instance

[<AllowNullLiteral>]
type GpsCoordinate(latitude: float, longitude: float) =
    let equals (other: GpsCoordinate) =
        if isNull other then
            false
        else
            latitude = other.Latitude
            && longitude = other.Longitude
    
    member _.Latitude = latitude
    member _.Longitude = longitude
    
    override this.GetHashCode() =
        hash (this.Latitude, this.Longitude)
    
    override _.Equals(obj) =
        match obj with
        | :? GpsCoordinate as other -> equals other
        | _ -> false
    
    interface IEquatable<GpsCoordinate> with
        member _.Equals(other: GpsCoordinate) =
            equals other
    
    static member op_Equality(this: GpsCoordinate, other: GpsCoordinate) =
        this.Equals(other)

let g1 = GpsCoordinate(25.0, 11.98)
let g2 = GpsCoordinate(25.0, 11.98)

let g3 = g1
g1 = g2
g1 = g3