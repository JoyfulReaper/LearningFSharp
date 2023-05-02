type DayOfWeek =
| Sunday = 0
| Monday = 1
| Tuesday = 2
| Wednesday = 3
| Thursday = 4
| Friday = 5
| Saturday = 6

let firstDayOfTheWeek = DayOfWeek.Sunday
printfn "The first day of the week is %s" (firstDayOfTheWeek.ToString())

let someInt = 5
let dayFive = enum<DayOfWeek> someInt
printfn "The integral value of %i corresponds to %s in the DayOfWeek enum" someInt (dayFive.ToString())