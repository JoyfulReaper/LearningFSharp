// List comprehensions
let items =
    [ for x in 1..5 do
        yield x]

// Since F# 5 yield can be omitted in most cases
let items' = [for x in 1..5 do x]

// Cons operator - Adding to a list
// head :: tail
let extendedItems = 6 :: items

// Pattern Matching on Lists

let readList items =
    match items with
    | [] -> "Empty List"
    | [head] -> $"Head: {head}" // list containing one item
    | head::tail -> sprintf "Head: %A and Tail: %A" head tail

let emptryList = readList []
let multipleList = readList [1;2;3;4;5]
let singleItemList = readList [1]

// Concatenating lists:
let list1 = [1..5]
let list2 = [3..7]
let emptyList = []

let joined = list1 @ list2
let joined2 = List.concat [list1;list2]

// Filtering lists
let myList = [1..9]
let getEvens items =
    items |> List.filter (fun x -> x % 2 = 0)

let evens = myList |> getEvens

let triple items =
    items
    |> List.map (fun x -> x * 3)

let myTriples = triple [1..5]

let print items =
    items |> List.iter (fun x -> printfn "My value is %i" x)

print myList

let items2 = [(1,0.25M);(5,0.25M);(1,2.25M);(1,125M);(7,10.9M)]
let sum items =
    items 
    |> List.map(fun (q, p) -> decimal q * p)
    |> List.sum

let sum' items =
    items
    |> List.sumBy (fun (q, p) -> decimal q * p)

items2 |> sum

// Folding

[1..10]
|> List.fold (fun acc v -> acc + v) 0

[1..10]
|> List.fold (+) 0

let items3 = [(1,0.25M);(5,0.25M);(1,2.25M);(1,125M);(7,10.9M)]

let getTotalFold items =
    items
    |> List.fold (fun acc (q, p) -> acc + decimal q * p) 0M

let total = getTotalFold items3

/////// Grouping Data and Uniqueness

let myList2 = [1;2;3;4;5;7;6;5;4;3]
let gbResult = myList2 |> List.groupBy (fun x -> x)

for (group, items) in gbResult do
    printfn "Group: %i" group
    for item in items do
        printfn "Item: %i" item

gbResult
|> List.iter (fun (group, items) ->
    printfn "Group: %i" group
    printfn "Items: %A" items)

gbResult
|> List.iter (fun (group, items) ->
    printfn "Group: %i" group
    items
    |> List.iter (fun item -> printfn "Item: %i" item))

// Distinct
let unique items =
    items
    |> List.groupBy id
    |> List.map (fun (i, _) -> i) 

let unResult = unique myList2

// Solving a problem many ways
// Sum the squares of the odd nunmbers
let nums = [1..10]

// Step by Step
nums
|> List.filter (fun v -> v % 2 = 1)
|> List.map (fun v -> v * v)
|> List.sum

// Using Option and choose
nums
|> List.choose (fun v -> if v % 2 = 1 then Some (v * v) else None)
|> List.sum

// Fold
nums
|> List.fold (fun acc v -> acc + if v % 2 = 0 then (v * v) else 0) 0

// Don't use reduce, there is an example and explanation of why not to use it in the book

// Recommended version
nums
|> List.sumBy (fun v -> if v % 2 = 0 then (v * v) else 0)