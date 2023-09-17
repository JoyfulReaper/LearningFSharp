open System

let tryDivide (x:decimal) (y:decimal) =
    try
        x/y
    with
    | :? System.DivideByZeroException as ex -> raise ex

let tryDivide' x y =
    try
        Ok (x/y)
    with
    | :? System.DivideByZeroException as ex -> 
        Error ex

let badDivide = tryDivide' 1m 0m
let goodDivide = tryDivide' 1m 1m