// Query Expressions
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

open Microsoft.FSharp.Linq.NullableOperators
// Query Expressions

let evenNumbers = 
    query {
        for n in [1..10] do
        where (n % 2 = 0)
        sortByDescending n
    }

evenNumbers |> Seq.iter (printfn "%i")

// Basic Querying - page 203
// Enumerable for loop and a projection
let allFilms = query { for f in films do select f }
allFilms |> Seq.iter (printfn "%A")

// Select isn't limited to projecting only the source data items;
// they can also transform the data into a new shape
let filmNameAndYear = query { for f in films do 
                              select (f.name, f.releaseYear) }

filmNameAndYear |> Seq.iter (fun (filmName, year) -> printfn "Film: %s Year: %i" filmName year)


// Filtering Data - page 204
// Predicate-based filtering
let filmsFrom1984 = query { for f in films do 
                            where (f.releaseYear = 1984)
                            select (f.ToString()) }

printfn "Films from 1984:"
filmsFrom1984 |> Seq.iter (printfn "%s")

// Nullable operators
// Gross is a Nullable<float> so we need to use the Nullable operators
let filmsGrossingOver40M = query { for f in QuerySource.films do
                                   where (f.gross ?<= 40_000_000.0)
                                   select (f.ToString()) }

let under40mFrom1987 = query { for f in QuerySource.films do 
                               where (f.releaseYear = 1987 && f.gross ?<= 40_000_000.0) }


// Distinct Item Filter - page 206
let releaseYears = query { for f in QuerySource.films do
                           select f.releaseYear 
                           distinct }


// Accessing arbitrary items from a sequence - page 207
// head or headOrDefault corresponds to the First and FirstOrDefault methods
// last or lastOrDefault are also available
let firstFilm = query { for f in QuerySource.films do
                        headOrDefault }

// Getting a specific item from a sequence
// nth -> like the ElementAt method
let thirdFilm = query { for f in QuerySource.films do
                        nth 2 }

// Finding the first item that matches a some criteria
let firstFilmFrom87 = query { for f in QuerySource.films do find (f.releaseYear = 1987) }

// Verfiying the match is a single item and it exists - also exactlyOneOrDefault
let filmWithId4 = query { for f in QuerySource.films do 
                          where (f.id = 4)
                          exactlyOne }



// Sorting - Page 209
// Ascending - also sortByNullable
let filmsByName = query { for f in QuerySource.films do 
                          sortBy f.name
                          select (f.ToString()) }

// Descending - also sortByNullableDescending
let filmsByNameDescending = query { for f in QuerySource.films do 
                                    sortByDescending f.name
                                    select (f.ToString()) }

// Sorting by multiple fields
// thenBy, thenByNullable, thenByDescending, thenByNullableDescending
let filmsByReleaseThenGross = query { for f in QuerySource.films do
                                      sortBy f.releaseYear
                                      thenByNullableDescending f.gross
                                      select (f.releaseYear, f.name, f.gross) }