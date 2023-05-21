let printList lines =
    for line in lines do
        printfn "%O" line

// Sequence Expression:
let lines = seq { use r = new System.IO.StreamReader("ArnoldMovies.txt")
                  while not r.EndOfStream do yield r.ReadLine() }

printList lines

// Range Expressions
let s1 = seq {0..10} // ints
let s2 = seq {0.0..10.0} // floats
let s3 = seq {'a'..'z'} // chars
let s4 = seq {0..10..100} // Value that identifies how many items to skip when generating the sequence. Integral multiples of 10 from 0 to 100. Only works for numeric types
let s5 = seq {99..-1..0} // Declining sequence

// Empty Sequence
let emptySequence = Seq.empty<string>
let emptySequence2 = Seq.empty // Compiler automaticly generalizes

// Initalzing a sequence
let rand = System.Random()
let randTen = Seq.init 10 (fun _ -> rand.Next(100))

printList randTen

// Finding Sequence Length
// can force enumeration of entire sequence
seq { 0..99 } |> Seq.length |> printfn "Elements: %i"

// Checking if a sequence is empty
seq { for i in 1..10 do
      printfn "Evaluating %i" i
      yield i } |> Seq.isEmpty |> printfn "Empty: %b"

// Functional equivilent of enumberable for loop: Seq.iter
seq { 0..99 } |> Seq.iter (printf "%i ")

//Transforming Sequenceses: Seq.map
seq { 0..99 } |> Seq.map (fun i -> i * i) |> Seq.iter (printf "%i ")

// Sorting seqeuences
Seq.init 10 (fun _ -> rand.Next 100) |> Seq.sort |> Seq.iter (printfn "%i ")

// Seq.sortBy
//Tuples contiaing title and release year
let movies = 
    seq { use r = new System.IO.StreamReader("ArnoldMovies.txt")
          while not r.EndOfStream do
          let l = r.ReadLine().Split(',')
          yield l.[0], int l.[1] }

movies |> Seq.sortBy snd |> Seq.iter (fun t -> printfn "%s: %i" <| fst t <| snd t)

// Filtering seqeuences: Seq.filter
movies |> Seq.filter (fun (_, year) -> year < 1985) |> Seq.iter (fun (t, y) -> printfn "%s: %i" t y)


// Aggergating Seqeuences
// Seq.Fold - iterates over a sequence, applying a function to each element and returning the result as an accumulator value
seq {1..10} |> Seq.fold (fun s c -> s + c) 0 |> printfn "I folded it: %i"

seq {1..10} |> Seq.fold (+) 0 |> printfn "I folded it: %i"

// Seq.reduce - aggregation value passes through the computation is always the same type as the sequence's elements 
//(fold can transform data to another type). Does not accept initial aggregation value, instead initializes the aggregation value to the first value in the sequence

seq {1..10} |> Seq.reduce (+) |> printfn "I reduced it: %i"

// Sum
seq { 1..10 } |> Seq.sum |> ignore


// Average
seq { 1.0..10.0 } |> Seq.average |> ignore