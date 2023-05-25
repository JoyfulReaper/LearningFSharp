// Lists
let names = [ "Rose"; "Martha"; "Donna"; "Amy"; "Clara"; ]
let numbers = [ 1..11 ]

// Accessing elements
printfn "%i" numbers.[0]
printfn "%i" <| numbers.Item(3)

// Head is the first element, tails is the rest of the elements
printfn "Head: %s" <| names.Head
List.iter (printfn "Tail: %s") <| names.Tail

// By seperating a list into head and tail, you're free to operate against the head and then iterate with the tail
// Example (like List.exists)
let rec contains fn l =
    if l = [] then false
    else fn(List. head l) || contains fn (List.tail l)

printfn "Contains 'Rose': %b" <| contains (fun n -> n = "Rose") names
printfn "Contains 'Rose': %b" <| contains (fun n -> n = "Rose") []

// Combining lists
// con operator ::
let newNames = "Ace" :: names
List.iter (printfn "%s") newNames

// Concatenation with @ and List.append
let classicNames = [ "Susan"; "Barbra"; "Sarah Jane" ]
let modernNames = [ "Rose"; "Martha"; "Donna"; "Amy"; "Clara"; ]

let allNames = classicNames @ modernNames
let allNames2 = List.append classicNames modernNames

// Combining a sequence of lists with List.concat
let combinedNames = List.concat [[ "Susan"; "Sarah Jane"]
                                 [ "Rose"; "Martha";]
                                 [ "Donna"; "Amy"; "Clara"; ]]

printfn "Sets:"
// Sets
let aplhabet = ['A'..'Z'] |> Set.ofList
let vowels = Set.empty.Add('A').Add('E').Add('I').Add('O').Add('U')

// Union of sets - The elements contained in either the first or second set
let set1 = [1..5] |> Set.ofList
let set2 = [3..7] |> Set.ofList
let union1 = Set.union set1 set2
let union2 = set1 + set2

Seq.iter(fun x -> printf "%i " x) union1
printfn ""

// Intersection of sets - The elements contained in both the first and second set
let intersect = Set.intersect set1 set2

Seq.iter(fun x -> printf "%i " x) intersect
printfn ""

// Difference of sets - The elements contained in the first set but not the second
let difference = Set.difference set1 set2

Seq.iter(fun x -> printf "%i " x) difference
printfn ""


// Subsets and Supersets
// Subset - All elements of the first set are contained in the second set
// Superset - All elements of the second set are contained in the first set

let set3 = [1..5] |> Set.ofList
let set4 = [1..5] |> Set.ofList

printfn "Set3 is a superset of Set4: %b" <| Set.isSuperset set3 set4
printfn "Set3 is a subset of Set4: %b" <| Set.isSubset set3 set4
printfn "Set3 is a proper superset of Set4: %b" <| Set.isProperSuperset set3 set4
printfn "Set3 is a proper subset of Set4: %b" <| Set.isProperSubset set3 set4