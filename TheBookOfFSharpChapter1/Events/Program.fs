// Basic Event Handling
let ticks = ref 0
let t = new System.Timers.Timer(500.0)
t.Elapsed.Add(fun ea -> printfn "tick"; ticks.Value <- ticks.Value + 1)
t.Start()
while ticks.Value < 5 do ()
t.Dispose()



// Observing Events
open System
open System.Data

let dt = new DataTable("person")
dt.Columns.AddRange
    [| new DataColumn("person_id", typedefof<int>)
       new DataColumn("first_name", typedefof<string>)
       new DataColumn("last_name", typedefof<string>) |]
dt.Constraints.Add("pk_person", dt.Columns.[0], true) |> ignore

// h1 will contain the events that satisify the predicate 
// h2 will contian the events that do not satisfy the predicate
let h1, h2 =
    dt.RowChanged
        |> Event.partition
            (fun ea ->
                let ln = ea.Row.["last_name"] :?> string
                ln.Equals("Pond", StringComparison.InvariantCultureIgnoreCase))

h1.Add (fun _ -> printfn "Come along, Pond")
h2.Add (fun _ -> printfn "Row changed")

dt.Rows.Add(1, "Rory", "Williams") |> ignore
dt.Rows.Add(2, "Amelia", "Pond") |> ignore




// Custom Events
type Toggle() =
    let toggleChangedEvent = Event<_>()
    let mutable isOn = false

    [<CLIEvent>] // Required to use event with other .NET languages
    member x.ToggleChanged = toggleChangedEvent.Publish

    member x.Toggle() =
        isOn <- not isOn
        toggleChangedEvent.Trigger (x, isOn)

let myToggle = Toggle()
let onHandler, offHandler =
    myToggle.ToggleChanged
    |> Event.map (fun (_, isOn) -> isOn)
    |> Event.partition (fun isOn -> isOn)

onHandler |> Event.add (fun _ -> printfn "Turned on!")
offHandler |> Event.add (fun _ -> printfn "Turned off!")

myToggle.Toggle()
myToggle.Toggle()