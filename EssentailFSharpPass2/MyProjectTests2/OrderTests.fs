namespace OrderTests

open MyProject.Orders
open MyProject.Orders.Domain
open Xunit
open FsUnit

module ``Add item to Order`` =
    
    [<Fact>]
    let ``When product does not exist in empt order`` () =
        let myEmptyOrder = { Id = 1; Items = []}
        let expeted = { Id = 1; Items = [ {ProductId = 1; Quantity = 3}]}
        let actual = myEmptyOrder |> addItem { ProductId = 1; Quantity = 3}
        actual |> should equal expeted

    [<Fact>]
    let ``When product does not exist in non empty order`` () =
        let myOrder = { Id = 1; Items = [{ ProductId = 1; Quantity = 1}]}
        let expected = { Id = 1; Items = [ { ProductId = 1; Quantity = 1}; { ProductId = 2; Quantity = 5}]}
        let actual = myOrder |> addItem { ProductId = 2; Quantity = 5}
        actual |> should equal expected
    
    [<Fact>]
    let ``When product exists in non empty order`` () =
        let myOrder = { Id = 1; Items = [{ ProductId = 1; Quantity = 1}]}
        let expected = { Id = 1; Items = [ { ProductId = 1; Quantity = 4}]}
        let actual = myOrder |> addItem { ProductId = 1; Quantity = 3}
        actual |> should equal expected

module ``Add Multiple itmes to an order`` =
    
    [<Fact>]
    let ``when new products added to empty order`` () = 
        let myEmptyOrder = { Id = 1; Items = [] }
        let expected = { Id = 1; Items = [ { ProductId = 1; Quantity = 1 }; { ProductId = 2; Quantity = 5 } ] }
        let actual = myEmptyOrder |> addItems [ { ProductId = 1; Quantity = 1 }; { ProductId = 2; Quantity = 5 } ]
        actual |> should equal expected

    [<Fact>]
    let ``when new products and updated existing to order`` () = 
        let myOrder = { Id = 1; Items = [ { ProductId = 1; Quantity = 1 } ] }
        let expected = { Id = 1; Items = [ { ProductId = 1; Quantity = 2 }; { ProductId = 2; Quantity = 5 } ] }
        let actual = myOrder |> addItems [ { ProductId = 1; Quantity = 1 }; { ProductId = 2; Quantity = 5 } ]
        actual |> should equal expected

module ``Removing a product`` =

    [<Fact>]
    let ``when remove all items of existing productid`` () = 
        let myOrder = { Id = 1; Items = [ { ProductId = 1; Quantity = 1 } ] }
        let expected = { Id = 1; Items = [] }
        let actual = myOrder |> removeProduct 1 
        actual |> should equal expected

    [<Fact>]
    let ``should do nothing for non existent productid`` () = 
        let myOrder = { Id = 2; Items = [ { ProductId = 1; Quantity = 1 } ] }
        let expected = { Id = 2; Items = [ { ProductId = 1; Quantity = 1 } ] }
        let actual = myOrder |> removeProduct 2 
        actual |> should equal expected
        
module ``Reduce item quantity`` =

    [<Fact>]
    let ``reduce existing item quantity`` () = 
        let myOrder = { Id = 1; Items = [ { ProductId = 1; Quantity = 5 } ] }
        let expected = { Id = 1; Items = [ { ProductId = 1; Quantity = 2 } ] }
        let actual = myOrder |> reduceItem 1 3
        actual |> should equal expected

    [<Fact>]
    let ``reduce existing item and remove`` () = 
        let myOrder = { Id = 2; Items = [ { ProductId = 1; Quantity = 5 } ] }
        let expected = { Id = 2; Items = [] }
        let actual = myOrder |> reduceItem 1 5
        actual |> should equal expected

    [<Fact>]
    let ``reduce item with no quantity`` () = 
        let myOrder = { Id = 3; Items = [ { ProductId = 1; Quantity = 1 } ] }
        let expected = { Id = 3; Items = [ { ProductId = 1; Quantity = 1 } ] }
        let actual = myOrder |> reduceItem 2 5
        actual |> should equal expected

    [<Fact>]
    let ``reduce item with no quantity for empty order`` () = 
        let myEmptyOrder = { Id = 4; Items = [] }
        let expected = { Id = 4; Items = [] }
        let actual = myEmptyOrder |> reduceItem 2 5
        actual |> should equal expected

module ``Empty an order of all items`` =

    [<Fact>]
    let ``order with existing item`` () = 
        let myOrder = { Id = 1; Items = [ { ProductId = 1; Quantity = 1 } ] }
        let expected = { Id = 1; Items = [] }
        let actual = myOrder |> clearItems
        actual |> should equal expected

    [<Fact>]
    let ``order with existing items`` () = 
        let myOrder = { Id = 1; Items = [ { ProductId = 1; Quantity = 1 };{ ProductId = 2; Quantity = 5 } ] }
        let expected = { Id = 1; Items = [] }
        let actual = myOrder |> clearItems
        actual |> should equal expected

    [<Fact>]
    let ``empty order is unchanged`` () = 
        let myEmptyOrder = { Id = 2; Items = [] }
        let expected = { Id = 2; Items = [] }
        let actual = myEmptyOrder |> clearItems
        actual |> should equal expected