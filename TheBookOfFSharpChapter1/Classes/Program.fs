open System

// Class with a primary constructor with 3 arguments and
// 3 implicit read-only properties
type Person (id: Guid, name: string, age: int) =
    member x.Id = id
    member x.Name = name
    member x.Age = age

let me = Person(Guid.NewGuid(), "Dave", 33)

// Class with a primary constructor with 2 arguments and type annotations
// a single field definition for the calculated age
// and a do block to print a message when the class is created
// Type annotation is needed for DOB so the compiler can resolve the
// correct subtract operator overload
type Person2 (name: string, dob: DateTime) =
    let age = (DateTime.Now - dob).TotalDays / 365.25
    do printfn "Creating person %s (Age %f)" name age
    member x.Name = name
    member x.Age = age
    member x.DateOfBirth = dob

let someone = Person2("Dave", DateTime(1979, 10, 12))

// Compiler can frequently infer the types of the constructor arguments
type Person3 (name, age) =
    do printfn "Creating person %s (Age %i)" name age
    member x.Name = name
    member x.Age = age

let anotherDave = Person3("Dave", 33)

// Private constructor
type Greeter private () =
    static let _instance = lazy (Greeter())
    static member Instance with get() = _instance.Force()
    member x.SayHello() = printfn "Hello"

Greeter.Instance.SayHello()

// Additional Constructors
type Person4 (name, age) =
    do printfn "Creating person %s (Age %i)" name age
    new (name) = Person4(name, 0)
    new () = Person4("")
    member x.Name = name
    member x.Age = age

let p4_1 = Person4("Dave", 33)
let p4_2 = Person4("Dave")
let p4_3 = Person4()

// Additional constructors use then keyword for invoking additonal code
type Person5 (name, age) =
    do printfn "Creating person %s (Age %i)" name age
    new (name) = 
        Person5(name, 0)
        then printfn "Creating %s with default age" name
    new () = 
        Person5("")
        then printfn "Creating person with default name and age"
    member x.Name = name
    member x.Age = age

let p5_1 = Person5("Dave", 33)
let p5_2 = Person5("Dave")
let p5_3 = Person5()


// Classes without a primary constructor
type Person6 = 
    val _name: string
    val _age : int
    new (name, age) = { _name = name; _age = age }
    new (name) = Person6(name, 0)
    new () = Person6("")
    member x.Name = x._name
    member x.Age = x._age

// Self-identifiers
// Required to reference a class member from within the constructor
type Person7 (name, age) as this =
    do printf "Creating person %s (Age %i)" this.Name this.Age
    member x.Name = name
    member x.Age = age

let someDude = Person7("SomeDude", 33)


// Structs
[<Struct>]
type Circle(diameter : float) =
    member x.getRadius() = diameter / 2.0
    member x.Diameter = diameter
    member x.GetArea() = Math.PI * (x.getRadius() ** 2)
