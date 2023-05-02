let getCommandLineArgs() =
#if INTERACTIVE
    fsi.CommandLineArgs
#else
    System.Environment.GetCommandLineArgs()
#endif

getCommandLineArgs() |> printfn "%A"
