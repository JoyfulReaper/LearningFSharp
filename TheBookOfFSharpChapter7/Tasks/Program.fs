open System
open System.Threading
open System.Threading.Tasks

// Task Parallelism

// Creating and starting tasks
Parallel.Invoke(
    (fun () -> printfn "Task 1"),
    (fun () -> Task.Delay(100).Wait()
               printfn "Task 2"),
    (fun () -> printfn "Task 3")
)

// Creating a task with task constructor
let t = new Task(fun () -> printfn "Manual Task")
t.Start()

// Using task factory (prefered)
let t2 = Task.Factory.StartNew(fun () -> printfn "Factory Task")

// Task that returns a value
let t3 = Task.Factory.StartNew(fun () -> System.Random().Next())
t3.Result |> printfn "Result: %i"

// Waiting for task completion
let randomWait (delayMs : int) (msg : string) =
    fun () -> (System.Random().Next delayMs |> Task.Delay).Wait()
              Console.WriteLine msg

let waitTask = Task.Factory.StartNew(randomWait 1000 "Task Finished!")
waitTask.Wait()
printfn "Done Waiting!"

// WaitAny and WaitAll can be used to wait for any or all tasks in an array of tasks to complete

// Continuations
let antecedent =
    new Task<string>(
        fun() ->
            Console.WriteLine("Started antecedent")
            System.Threading.Thread.Sleep(1000)
            Console.WriteLine("Completed antecedent")
            "Jobs done")

let continuation =
    antecedent.ContinueWith(
        fun (a: Task<string>) ->
            Console.WriteLine("Started Continuation")
            Console.WriteLine("Antecedent status: {0}", a.Status)
            Console.WriteLine("Antecedent Result: {0}", a.Result)
            Console.WriteLine("Completed Continuation"))

antecedent.Start()
Console.WriteLine("Waiting for continuation")
continuation.Wait()
Console.WriteLine("Done")

// ContinueWithAny and ContinueWithAll TaskFactory Methods can continue arrays of tasks - pg 238
let antecedents =
  [|
    new Task(
      fun () ->
        Console.WriteLine("Started first antecedent")
        System.Threading.Thread.Sleep(1000)
        Console.WriteLine("Completed first antecedent"))
    new Task(
      fun () ->
        Console.WriteLine("Started second antecedent")
        System.Threading.Thread.Sleep(1250)
        Console.WriteLine("Completed second antecedent"))
    new Task(
      fun () ->
        Console.WriteLine("Started third antecedent")
        System.Threading.Thread.Sleep(1000)
        Console.WriteLine("Completed third antecedent"))
  |];;

let continuation2 =
  Task.Factory.ContinueWhenAll(
    antecedents,
    fun (a : Task array) ->
      Console.WriteLine("Started continuation")
      for x in a do Console.WriteLine("Antecedent status: {0}", x.Status)
      Console.WriteLine("Completed continuation"));;

for a in antecedents do a.Start();;

Console.WriteLine("Waiting for continuation")
continuation2.Wait()
Console.WriteLine("Done");;

// Cancelling Tasks
let taskWithCancellation (cancelDelay : int) (taskDelay : int) =
  use tokenSource = new System.Threading.CancellationTokenSource(cancelDelay)
  let token = tokenSource.Token

  try
    let t =
      Task.Factory.StartNew(
        (fun () ->
          token.ThrowIfCancellationRequested()
          printfn "passed cancellation check; waiting"
          System.Threading.Thread.Sleep taskDelay
          token.ThrowIfCancellationRequested()
          ),
        token
      )
    t.Wait()
  with
  | ex -> printfn "%O" ex
  printfn "Done";;

taskWithCancellation 1000 3000;;