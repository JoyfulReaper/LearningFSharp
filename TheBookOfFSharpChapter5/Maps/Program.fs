// Maps: Unordered, immutable dictionary
// Maps only make sense when the underlying entries won't change (since they are immutable)

// Create a map
let stateCapitals =
    Map [ ("Indiana", "Indianapolis")
          ("Michigan", "Lansing")
          ("Ohio", "Columbus")
          ("Kentucky", "Frankfort")
          ("Illinois", "Springfield") ]

printfn "%s" <| stateCapitals.["Indiana"]
stateCapitals |> Map.find "Michigan" |> printfn "%s"
stateCapitals |> Map.containsKey "Washington" |> printfn "Map contains \"Washington\" %b"


// TryFind returns an option type
match stateCapitals.TryFind "Washington" with
| Some capital -> printfn "Capital of Washington is %s" capital
| None -> printfn "Washington doesn't exist"

// TryFindKey
match stateCapitals |> Map.tryFindKey (fun k v -> v = "Columbus") with
| Some state -> printfn "Columbus is the capital of %s" state
| None -> printfn "Columbus is not a capital"

