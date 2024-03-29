type Score = int * int

// Partial Active Pattern
let (|CorrectScore|_|) (expected: Score, actual: Score) =
    if expected = actual then Some () else None
    
// Multi-case Active Pattern
let (|Draw|HomeWin|AwayWin|) (score: Score) =
    match score with
    | (h, a) when h = a -> Draw
    | (h, a) when h > a -> HomeWin
    | _ -> AwayWin
    
// Partial Active Pattern
let (|CorrectResult|_|) (expected: Score, actual: Score) =
    match (expected, actual) with
    | (Draw, Draw) -> Some ()
    | (HomeWin, HomeWin) -> Some ()
    | (AwayWin, AwayWin) -> Some ()
    | _ -> None

let goalsScore (expected:Score) (actual:Score) =
    let (h,a) = expected
    let (h', a') = actual
    let home = [ h; h' ] |> List.min
    let away = [ a; a' ] |> List.min
    (home * 15) + (away * 20)
    
let goalsScore' (expected:Score) (actual:Score) =
    let home = [fst expected; fst actual] |> List.min
    let away = [snd expected; snd actual] |> List.min
    (home * 15) + (away * 20)
    
let resultScore (expected:Score) (actual:Score) =
        match (expected, actual) with
        | CorrectScore -> 400
        | CorrectResult -> 100
        | _ -> 0
    
// let calculatePoints (expected:Score) (actual:Score) =
//     let pointsForCorrectScore =
//         match (expected, actual) with
//         | CorrectScore -> 300
//         | _ -> 0
//     let pointsForCorrectResult =
//         match (expected, actual) with
//         | CorrectResult -> 100
//         | _ -> 0
//     let pointsForGoals = goalsScore' expected actual
//     pointsForCorrectScore + pointsForCorrectResult + pointsForGoals
    
// let calculatePoints (expected:Score) (actual:Score) =
//     let pointsForResult = resultScore expected actual
//     let pointsForGoals = goalsScore' expected actual
//     pointsForResult + pointsForGoals
    
let calculatePoints (expected:Score) (actual:Score) =
    [resultScore; goalsScore']
    |> List.sumBy (fun f -> f expected actual)
    
let assertnoScoreDrawCorrect =
    calculatePoints (0, 0) (0, 0) = 400
let assertHomeWinExactMatch =
    calculatePoints (3, 2) (3, 2) = 485
let assertHomeWin =
    calculatePoints (5, 1) (4, 3) = 180
let assertIncorrect =
    calculatePoints (2, 1) (0, 7) = 20
let assertDraw =
    calculatePoints (2, 2) (3, 3) = 170