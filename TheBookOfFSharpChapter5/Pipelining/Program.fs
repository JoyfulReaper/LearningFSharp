let add a b = a + b

// Pipelining: Creating function chains
// Evaluate one expression and send the result to another function as the final argument


//Forward Pipelining |>
// Sending values forward to the next function

// Ignoring a return value other than unit
add 2 3 |> ignore

let fahrenheitToCelsius degreesF = (degreesF - 32.0) * (5.0 / 9.0)

let marchHighTemps = [ 33.0; 30.0; 33.0; 38.0; 36.0; 31.0; 35.0;
                       42.0; 53.0; 65.0; 59.0; 42.0; 31.0; 41.0;
                       49.0; 45.0; 37.0; 42.0; 40.0; 32.0; 33.0;
                       42.0; 48.0; 36.0; 34.0; 38.0; 41.0; 46.0;
                       54.0; 57.0; 59.0]

marchHighTemps
|> List.average
|> fahrenheitToCelsius
|> printfn "March Average (C): %f" 


// Backward pipelining
// Sends the result of an expression to another function as the final argument from right to left
printfn "March Average (F): %f" <| List.average marchHighTemps

// Pipelining with noncurried functions
// Works with non-curried functions (methods) that accept a single argument
5.0 
|> System.TimeSpan.FromSeconds
|> System.Threading.Thread.Sleep



// Function composition: Same input/ouput rules as pipelining
// Allows creating function chains like pipelining.
// forward >> and backward << composition
// Instead of a one-time operation (pipelining) a new function is returned
let averageInCelsius = List.average >> fahrenheitToCelsius
printfn "averaveInCelsius: %f" <| averageInCelsius marchHighTemps

// Noncurried functions also work
let delay = System.TimeSpan.FromSeconds >> System.Threading.Thread.Sleep
delay 1.0