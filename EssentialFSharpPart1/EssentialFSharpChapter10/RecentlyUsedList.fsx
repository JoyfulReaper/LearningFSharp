type RecentlyUsedList() =
    let items = ResizeArray<string>() // .NET mutable List

    let add item =
        items.Remove item |> ignore
        items.Add item

    let get index =
        if index >=0 && index < items.Count
        then Some items[items.Count - index - 1]
        else None

    member _.IsEmpty = items.Count = 0 // Read only properties
    member _.Size = items.Count
    member _.Clear() = items.Clear() // Functions
    member _.Add(item) = add item
    member _.TryGet(index) = get index

let mrul = RecentlyUsedList()

mrul.Add "Test"
mrul.IsEmpty

mrul.Add "Test2"
mrul.Add "Test3"
mrul.Add "Test"
mrul.TryGet(0) = Some "Test" // Should return true