// Interfaces

//Implementing an interface
open System

type MyDisposable() =
    interface IDisposable with
        member x.Dispose() = printfn "disposing"

// To invoke Dispose need to case to IDisposable with the static cast operator
let d = new MyDisposable()
(d :> IDisposable).Dispose()


// Defining interfaces
open System.IO
open System.Drawing

type IImageAdapter =
    abstract member PixelDimensions : SizeF with get
    abstract member VerticalResolution : int with get
    abstract member HorizontalResolution : int with get


// Interface inheritance
type ITransparentImageAdapter =
    inherit IImageAdapter
    abstract member TransparentColor : Color with get, set