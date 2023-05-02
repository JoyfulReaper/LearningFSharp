type Container() =
    member x.Fill ?stopAtPercent =
        printfn "%s" <| match (defaultArg stopAtPercent 0.5) with
                        | 1.0 -> "Filled it up"
                        | stopAt -> sprintf "Filled to %s" (stopAt.ToString("P2"))
let bottle = Container()

bottle.Fill()
bottle.Fill(1.0)
bottle.Fill(0.75)