[<AutoOpen>]
module QuerySource =
    open System

    type film = { id: int; name : string; releaseYear: int; gross : Nullable<float> }
                 override x.ToString() = sprintf "%s (%i)" x.name x.releaseYear

    type actor = { id: int; firstName : string; lastName: string }
                 override x.ToString() = sprintf "%s %s" x.firstName x.lastName

    type filmActor = { filmId: int; actorId: int }


    let films = 
        [ { id = 1; name = "The Terminator"; releaseYear = 1984; gross = Nullable 38400000.0 }
          { id = 2; name = "Predator"; releaseYear = 1987; gross = Nullable 59735548.0 }
          { id = 3; name = "Commando"; releaseYear = 1985; gross = Nullable<float>() }
          { id = 4; name = "The Running Man"; releaseYear = 1987; gross = Nullable 38122105.0 }
          { id = 5; name = "Conan the Destroyer"; releaseYear = 1984; gross = Nullable<float>() } ]
          
    let actors =
        [ { id = 1; firstName = "Arnold"; lastName = "Schwarzenegger" } 
          { id = 2; firstName = "Linda"; lastName = "Hamilton" }
          { id = 3; firstName = "Carl"; lastName = "Weathers" }
          { id = 4; firstName = "Jesse"; lastName = "Ventura" }
          { id = 5; firstName = "Vernon"; lastName = "Wells" } ]

    let filmActors = 
        [ { filmId = 1; actorId = 1 }
          { filmId = 1; actorId = 2 }
          { filmId = 2; actorId = 1 }
          { filmId = 2; actorId = 3 }
          { filmId = 2; actorId = 4 }
          { filmId = 3; actorId = 1 }
          { filmId = 3; actorId = 5 }
          { filmId = 4; actorId = 1 }
          { filmId = 4; actorId = 4 }
          (* Intentionally omitted actor for filmId = 5 *)]

// Extending Query Expressions
open System
open Microsoft.FSharp.Linq

type QueryBuilder with
    [<CustomOperation("exactlyOneWhen")>]
    member __.ExactlyOneWhen (source : QuerySource<'T, 'Q>,
                                       [<ProjectionParameter>] selector) =
        System.Linq.Enumerable.Single (source.Source, Func<_,_>(selector))

    [<CustomOperation("exactlyOneOrDefaultWhen")>]
    member __.ExactlyOneOrDefaultWhen (source: QuerySource<'T, 'Q>,
                                      [<ProjectionParameter>] selector ) =
        System.Linq.Enumerable.SingleOrDefault(source.Source, Func<_,_>(selector))

    [<CustomOperation("averageByNotNull")>]
      member inline __.AverageByNotNull< 'T, 'Q, 'Value
                          when 'Value :> ValueType
                          and 'Value : struct
                          and 'Value : (new : unit -> 'Value)
                          and 'Value : (static member op_Explicit : 'Value -> float)>
        (source : QuerySource<'T, 'Q>,
         [<ProjectionParameter>] selector : 'T -> Nullable<'Value>) =
          source.Source
          |> Seq.fold
              (fun (s, c) v -> let i = v |> selector
                               if i.HasValue then
                                (s + float i.Value, c + 1)
                               else (s, c))
              (0.0, 0)
          |> (function
              | (_, 0) -> Nullable<float>()
              | (sum, count) -> Nullable(sum / float count))


let filmFour = query { for f in QuerySource.films do
                       exactlyOneWhen (f.id = 4) }

let averageGross = query { for f in QuerySource.films do 
                           averageByNotNull f.gross }

printfn "Average Gross: %f" averageGross.Value