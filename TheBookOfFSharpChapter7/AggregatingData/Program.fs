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

open System
open FSharp.Linq

// Aggregating Data
// count, 
//minBy, maxBy, sumBy, averageBy -- and Nullable versions

let filmCount = query { for f in QuerySource.films do count }
printfn "Film count: %i" filmCount

let highestGrossingFilm = 
    query { for f in QuerySource.films do maxByNullable f.gross }
printfn "Highest grossing film: %A" highestGrossingFilm

// Detecting Items
// contains, exists, all
let kindergartenCop =
    {id = 6; name = "Kindergarten Cop"; releaseYear = 1990; gross = Nullable 91457688.0}

let filmsContainsKindergartenCop =
    query { for f in QuerySource.films do
            contains kindergartenCop }

printfn "Contains Kindergarten Cop: %b" filmsContainsKindergartenCop

// Not efficent - Enumerates the entire sequence
let filmsContainsKindergartenCop2 =
    query { for f in QuerySource.films do
            select f.name
            contains "Kindergarten Cop" }

// More efficent - Stops enumerating when it finds a match
let filmsContainsKindergartenCop3 =
    query { for f in QuerySource.films do
            exists (f.name = "Kindergarten Cop") }

let didAFilmGrossOver50m =
    query { for f in QuerySource.films do
            exists (f.gross ?>= 50000000.0) }

let allFilmsGrossed50m =
    query { for f in QuerySource.films do 
            all (f.gross ?>= 50000000.0) }

printfn ("%b") allFilmsGrossed50m


// Inner Join
let filmsWithActors =
    query { for f in QuerySource.films do
            join fa in QuerySource.filmActors on (f.id = fa.filmId) 
            join a in QuerySource.actors on (fa.actorId = a.id) 
            select (f.name, f.releaseYear, a.lastName, a.firstName) }

Seq.iter (fun (n, r, f, l) -> printfn "%s %i %s %s" n r f l) filmsWithActors

// Group Join
let actorsByFilm =
    query { for f in QuerySource.films do
            groupJoin fa in QuerySource.filmActors on (f.id = fa.filmId) into junction
            select (f.name, query { for j in junction do 
                                    join a in QuerySource.actors on (j.actorId = a.id)
                                    select (a.lastName, a.firstName) })
    }

// Left outer join + Null issues - pg 217-218
printfn "Left outer join with null awareness"
let actorsFilmActors = 
    query { for a in QuerySource.actors do 
            join fa in QuerySource.filmActors on (a.id = fa.actorId)
            select (fa.filmId, a) }

let res = query { for f in QuerySource.films do
                  leftOuterJoin (id, a) in actorsFilmActors on (f.id = id) into junction
                  for x in junction do
                  select (match (x :> obj) with
                          | null -> (f.name, "", "")
                          | _ -> let _, a = x
                                 (f.name, a.lastName, a.firstName))
              } |> Seq.iter (printfn "%O")