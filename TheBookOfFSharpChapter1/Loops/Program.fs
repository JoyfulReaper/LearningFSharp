
//While Loop
let echoUserInput (getInput: unit -> string) =
    let mutable input = getInput()
    while not (input.ToUpper().Equals("QUIT")) do
        printfn "You entered: %s" input
        input <- getInput()

echoUserInput(fun () -> printfn "Type something and press enter"
                        System.Console.ReadLine())

// Simple For loop
for i = 0 to 10 do printfn "%i" i
for i = 10 downto 0 do printfn "%A" i

// Enumerable for loop
for i in [0..5] do
    printfn "%A" i

//enumerable for loop
type MpaaRating =
| G
| PG
| PG13
| R
| NC17

type movie = { Title : string; Year : int; Rating : MpaaRating option }

let movies = [ { Title = "The Last Witch Hunter"; Year = 2014; Rating = None }
               { Title = "Riddick"; Year = 2013; Rating = Some(R) }
               { Title = "Fast Five"; Year = 2011; Rating = Some(PG13) }
               { Title = "Babylon A.D."; Year = 2008; Rating = Some(PG13) } ]

for { Title = t; Year = y; Rating = Some(r) } in movies do
    printfn "%s (%i) - %A" t y r