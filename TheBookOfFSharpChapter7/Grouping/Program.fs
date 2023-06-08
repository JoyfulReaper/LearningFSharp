[<AutoOpen>]
module QuerySource =
    open System

    type film = { id: int; name : string; releaseYear: int; gross : Nullable<float> }
                 override x.ToString() = sprintf "%s (%i)" x.name x.releaseYear

    type actor = { id: int; firstName : string; lastName: string }
                 override x.ToString() = sprintf "%s %s" x.firstName x.lastName

    type filmActor = { filmId: int; actorId: int }


    let films = 
        [ { id = 1; name = "The Terminator"; releaseYear = 1984; gross = Nullable 384000000.0 }
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

// Grouping
let filmsByReleaseYear =
    query {
        for f in QuerySource.films do
        groupBy f.releaseYear into g
        sortBy g.Key
        select (g.Key, g)
    }

Seq.iter (printfn "%A") filmsByReleaseYear

// groupValBy - Sepecifying values to include in the group
let filmsByReleaseYear2 =
    query {
        for f in QuerySource.films do
        groupValBy (f.name, f.gross) f.releaseYear into g
        sortBy g.Key
        select (g.Key, g)
    }

Seq.iter (printfn "%A") filmsByReleaseYear2

// Paginating
let getFilmPageBySize pageSize pageNumber =
    query { for f in QuerySource.films do
            skip (pageSize * (pageNumber - 1))
            take pageSize 
            select (f.ToString()) }

getFilmPageBySize 2 1 |> Seq.iter (printfn "%A")
getFilmPageBySize 2 2 |> Seq.iter (printfn "%A")
getFilmPageBySize 2 3 |> Seq.iter (printfn "%A")
getFilmPageBySize 2 4 |> Seq.iter (printfn "%A")

let getFilmPageByYear year =
    query { for f in QuerySource.films do
            sortBy f.releaseYear
            skipWhile (f.releaseYear < year)
            takeWhile (f.releaseYear = year)
            select (f.ToString()) }

getFilmPageByYear 1984 |> Seq.iter (printfn "%A")