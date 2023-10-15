type IRecentlyUsedList =
    abstract member IsEmpty : bool
    abstract member Size : int
    abstract member Capacity : int
    abstract member Clear : unit -> unit
    abstract member Add : string -> unit
    abstract member TryGet : int -> string option


type RecentlyUsedList(capacity:int) =
    
    // Items can not be accessed directly, only via the public interface: Encapsulation
    let items = ResizeArray<string>(capacity) // Mutable List<'T> from .NET
    
    let add item =
        items.Remove item |> ignore
        if items.Count = items.Capacity then items.RemoveAt 0
        items.Add item
   
    let get index =
        if index >= 0 && index < items.Count then
            Some items[items.Count - index - 1]
        else
            None
            
    interface IRecentlyUsedList with
        member _.IsEmpty = items.Count = 0
        member _.Size = items.Count
        member _.Clear() = items.Clear()
        member _.Add(item) = add item
        member _.TryGet(index) = get index
        member _.Capacity = items.Capacity
    
let mrul = RecentlyUsedList(5) :> IRecentlyUsedList

mrul.Capacity

mrul.Add "Test"
mrul.Size
mrul.Capacity

mrul.Add "Test2"
mrul.Add "Test3"
mrul.Add "Test4"
mrul.Add "Test"
mrul.Add "Test6"
mrul.Add "Test7"
mrul.Add "Test"

mrul.Size
mrul.Capacity

mrul.TryGet(0) = Some "Test"
mrul.TryGet(4) = Some "Test3"