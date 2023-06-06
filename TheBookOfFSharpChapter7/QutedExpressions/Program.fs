open Microsoft.FSharp.Quotations


// Quoted Expressions

// Strongly typed quoted expression represnting multiplying two values
let x, y = 10, 10
let expr = <@ x * y @>

// Lambda expression that multiplies two values
let expr2 = <@ fun a b -> a * b @>

// Multiple expressions in a single quoted expression
let expr3 = <@ let mult x y = x * y
             mult 10 20 @>

// .NET Reflection - Creating quoted expressions through reflection
type Calc =
    [<ReflectedDefinition>]
    static member Multiply a b = a * b

let expr4 = 
    typeof<Calc>
        .GetMethod("Multiply")
        |> Expr.TryGetReflectedDefinition

// Manual Composition - equivalant to <@@ fun x y -> x * y @@>
let operators = 
    System.Type.GetType("Microsoft.FSharp.Core.Operators")
let multiplyOperator = operators.GetMethod("op_Multiply")
let varX, varY =
    multiplyOperator.GetParameters()
    |> Array.map (fun p -> Var(p.Name, p.ParameterType))
    |> (function | [| x; y |] -> x, y
                    | _ -> failwith "Not supported")

let call = Expr.Call(multiplyOperator, [Expr.Var(varX); Expr.Var(varY)])
let innerLambda = Expr.Lambda(varY, call)
let outerLambda = Expr.Lambda(varX, innerLambda)

// Splicing Quoted Expressions
let numbers = seq{ 1 .. 10 }
let sum = <@ Seq.sum numbers @>
let count = <@ Seq.length numbers @>

// Combine using strongly typed splice operator '%' (weakly typed is '%%')
let avgExpr = <@ %sum / %count @>