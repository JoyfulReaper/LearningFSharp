// Supporting Structural integrity for an F# object
// Override GetHashCode and Equals and implement IEquatable<T> interface
// For .NET interop need op_Equality operator and AllowNullLiteral attribute

open System

[<AllowNullLiteral>]
type GpsCoordinate (latitude: float, longitude: float) =
        let equals (other: GpsCoordinate) =
            if isNull other then
                false
            else
                latitude = other.Latitude && longitude = other.Longitude
        member _.Latitude = latitude
        member _.Longitude = longitude
        
        override this.GetHashCode() =
            hash (this.Latitude, this.Longitude)
            
        override _.Equals(obj) =
            match obj with
            | :? GpsCoordinate as other -> equals other
            | _ -> false
            
        interface IEquatable<GpsCoordinate> with
            member _.Equals(other) = equals other
            
        static member op_Equality (this: GpsCoordinate, other: GpsCoordinate) =
            this.Equals(other)
        
let c1 = GpsCoordinate(25.0, 11.98)
let c2 = GpsCoordinate(25.0, 11.98)
let c3 = c1

c1 = c2 
c1 = c3