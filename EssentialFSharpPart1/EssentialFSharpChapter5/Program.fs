// Intoduction to Collections

// Empty List
let items = []

let items2 = [2;5;3;1;4]

let items3 = [1..5]

// List Comprehension:
let items4 = [
    for x in 1..5 do
        yield x
]

// Since F# 5 - yield keyword can be dropped
let items5 = [for x in 1..5 do x]

// Cons operator can be used to add an item to a list:
// head :: tail
let extendedItems = 6::items3

// List pattern matching
let readList items =
    match items with
    | [] -> "Empty List"
    | [head] -> $"Head: {head}"
    | head::tail -> sprintf "Head: %A and Tail %A" head tail

let emptyList = readList []
let multipleList = readList [1;2;3;4;5]
let singleItemList = readList [1]

emptyList |> printfn "%s"
multipleList |> printfn "%s"
singleItemList |> printfn "%s"

// List pattern matching
let readList2 items =
    match items with
    | [] -> "Empty List"
    | head::tail -> sprintf "Head: %A and Tail %A" head tail

let emptyList2 = readList2 []
let multipleList2 = readList2 [1;2;3;4;5]
let singleItemList2 = readList2 [1]

emptyList2 |> printfn "%s"
multipleList2 |> printfn "%s"
singleItemList2 |> printfn "%s"

// Concatenating lists

let list1 = [1..5]
let list2 = [3..7]
let emptyList3 = []

let joined = list1 @ list2
let joinedEmpty = list1 @ emptyList3
let emptyJoined = emptyList3 @ list1

let joinedconcat = List.concat [list1;list2]

// List filtering: predicate function a -> bool signature:
let myList = [1..9]

let getEvens items =
    items
    |> List.filter(fun x -> x % 2 = 0)

let events = getEvens myList

// Summing List:
let sum (items: int list) =
    items |> List.sum

let mySum = sum myList

// Performing an operation on each item in a list and returning a new list
// List.map - Higer order function that transforms 'a list to 'b list
// using a function that converts 'a -> 'b where 'a and 'b could be the same type
// Select is the closest LINQ equivalent to List.map (except lazily evaluated like Seq.map)
let triple items =
    items
    |> List.map(fun x -> x * 3)

let myTriples = triple [1..5]

// Performing an operation on each item in a list, without returning a new list
let print items =
    items
    |> List.iter (fun x -> (printfn "My value is %i" x))

print myList

// More complicated List.map example. Changes structure of output list
let items6 = [(1,0.25M);(5,0.25M);(1,2.25M);(7,10.9M)]
let sumTupple items =
    items
    |> List.map (fun (q, p) -> decimal q * p) // Need to convert q to decimal. Pattern matching to deconstruct tupple in lambda
    |> List.sum

// List.sumBy
let sumTupple2 items =
    items
    |> List.sumBy(fun (q, p) -> decimal q * p)

// Folding - List.fold - LINQ Aggregate
// ('a -> 'b -> 'a) -> 'a -> 'b list -> 'a
// folder -> initial value -> input list -> output value
[1..10]
|> List.fold (fun acc v -> acc + v) 0

[1..10]
|> List.fold (+) 0

// Total price example using List.fold:
let items7 = [(1,0.25M);(5,0.25M);(1,2.25M);(7,10.9M)]

let getTotal items =
    items
    |> List.fold (fun acc (q,p) -> acc + decimal q * p) 0M

let total = getTotal items7

// tuple pair inputs with ||>
let getTotal2 items =
    (0M, items) ||> List.fold (fun acc (q,p) -> acc + decimal q * p)

let total2 = getTotal2 items7

// Grouping Data and Uniqueness
let myList2 = [1;2;3;4;5;7;6;5;4;3]

// int list -> (int * int list) list
// Returns a list of tuples containing the data you grouped by as firs item
// and list of instances that exist in orginal list as the second
let gbResult = myList2 |> List.groupBy(fun x -> x)

// List of unique itmes using List.groupBy and List.map
let unique items =
    items
    |> List.groupBy id // Anonymous function x -> x can be replaced by the identity function: id
    |> List.map (fun (i, _) -> i)

let unResult = unique myList2

let distinct = myList2 |> List.distinct

// With Set
let uniqueSet items =
    items
    |> Set.ofList

let setResult = uniqueSet myList2

// Solving a Problem in Many ways:
// Sum of the squares of the odd numbers
let nums = [1..10]

// Step by step
nums
|> List.filter (fun v -> v % 2 = 1)
|> List.map (fun v -> v * v)
|> List.sum

// Using option and choose
nums
|> List.choose(fun v -> if v % 2 = 1 then Some (v * v) else None)
|> List.sum

// Fold
nums
|> List.fold (fun acc v -> acc + if v % 2 = 1 then (v * v) else 0) 0

// Recomended version
nums
|> List.sumBy(fun v -> if v % 2 = 1 then (v * v) else 0)


/// A Practical Example
