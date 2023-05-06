let isEven number = 
    if number = 0 then
        sprintf "zero"
    elif number % 2 = 0 then
        sprintf "%i is even" number
    else
        sprintf "%i is odd" number

let foo = 3
let bar = 4
printfn "%s" (isEven foo)
printfn "%s" (isEven bar)