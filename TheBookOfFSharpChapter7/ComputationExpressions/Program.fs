// Computation Expressions

type FizzBuzzSequenceBuilder() =
    member x.Yield(v) =
        match (v % 3, v % 5) with
        | 0, 0 -> "FizzBuzz"
        | 0, _ -> "Fizz"
        | _, 0 -> "Buzz"
        | _ -> v.ToString()

    member x.Delay(f) = f() |> Seq.singleton

    member x.Delay(f: unit -> string seq) = f()

    member x.Combine(l, r) =
        Seq.append (Seq.singleton l) (Seq.singleton r)

    member x.Combine(l, r) =
        Seq.append (Seq.singleton l) r

    member x.For(g, f) = Seq.map f g

let fizzBuzz = FizzBuzzSequenceBuilder()

fizzBuzz {
    for x = 1 to 99 do yield x 
}

////////////////

open System.Text

type StringFragement =
| Empty
| Fragment of string
| Concat of StringFragement * StringFragement
    override x.ToString() =
        let rec flatten frag (sb : StringBuilder) =
            match frag with
            | Empty -> sb
            | Fragment(s) -> sb.Append(s)
            | Concat(s1, s2) -> sb |> flatten s1 |> flatten s2
        (StringBuilder() |> flatten x).ToString()

type StringFragmentBuilder() =
    member x.Zero = Empty
    member x.Yield(v) = Fragment(v)
    member x.YieldFrom(v) = v
    member x.Combine(l, r) = Concat(l, r)
    member x.Delay(f) = f()
    member x.For(s, f) =
        Seq.map f s
        |> Seq.reduce (fun l r -> x.Combine(l, r))

let buildString = StringFragmentBuilder

