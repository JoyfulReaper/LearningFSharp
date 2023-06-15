// Agent based programming
type Agent<'T> = MailboxProcessor<'T>

type Message = | Message of obj

let echoAgent = 
    Agent<Message>.Start(
        fun inbox ->
            let rec loop () =
                async {
                    let! (Message(content)) = inbox.Receive()
                    printfn "%O" content
                    return! loop()
                }
            loop())

let echoAgent2 =
    Agent<Message>.Start(fun inbox ->
        let rec loop () =
            inbox.Scan(fun (Message(x)) ->
                match x with
                | :? string
                | :? int -> 
                    Some (async { printfn "%O" x 
                                  return! loop() })
                | _ -> printfn "<not handled>"; None)
        loop())

type ReplyMessage = | ReplyMessage of obj * AsyncReplyChannel<obj>

let echoAgent3 =
    Agent.Start(fun inbox ->
        let rec loop () =
            async {
                let! (ReplyMessage(m,c)) = inbox.Receive()
                c.Reply m
                return! loop()
            }
        loop())

echoAgent3.PostAndReply(fun c -> ReplyMessage("hello", c)) |> printfn "Response: %O"

// Agent-Based Calculator
type Operation =
| Add of float
| Subtract of float
| Multiply of float
| Divide of float
| Clear
| Current of AsyncReplyChannel<float>

let calcAgent =
    Agent.Start(fun inbox ->
        let rec loop total =
            async {
                let! msg = inbox.Receive()
                let newValue =
                    match msg with
                    | Add x -> total + x
                    | Subtract x -> total - x
                    | Multiply x -> total * x
                    | Divide x -> total / x
                    | Clear -> 0.0
                    | Current channel ->
                      channel.Reply total
                      total
                return! loop newValue }
        loop 0.0)

[ Add 10.0
  Subtract 5.0
  Multiply 10.0
  Divide 2.0 ] |> List.iter (calcAgent.Post)

calcAgent.PostAndReply(Current) |> printfn "Result: %f"
calcAgent.Post(Clear)
calcAgent.PostAndReply(Current) |> printfn "Result: %f"