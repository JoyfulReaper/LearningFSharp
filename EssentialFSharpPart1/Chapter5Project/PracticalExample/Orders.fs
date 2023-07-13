namespace PracticalExample.Orders

type Item = {
    ProductId: int
    Quantity: int
}

type Order = {
    Id: int
    Items: Item list
}

module Domain =
    
    let recalculate items =
        items
        |> List.groupBy (fun i -> i.ProductId)
        |> List.map (fun (id, items) -> 
            { ProductId = id; Quantity = items |> List.sumBy (fun i -> i.Quantity)})
        |> List.sortBy (fun i -> i.ProductId)

    // Item list -> Order -> Order
    let addItems newItems order =
        let items = 
            newItems @ order.Items // Concat operator - join 2 lists
            |> recalculate
        { order with Items = items }

    // 1 - Prepend new item to existing order items
    // 2 - Consolidate each product
    // 3 - Sort items in order by productid to make equality simpler
    // 4 - Update order with new list of items
    // Item -> Order -> Order
    let addItem item order =
        let items = 
            item::order.Items
            |> recalculate
        { order with Items = items }


    let removeProduct productId order =
        let items =
            order.Items
            |> List.filter(fun x -> x.ProductId <> productId)
            |> List.sortBy(fun i -> i.ProductId)
        { order with Items = items }

    let reduceItem productId quanity order =
        let items =
            { ProductId = productId; Quantity = -quanity} :: order.Items
            |> recalculate
            |> List.filter (fun x -> x.Quantity > 0)
        { order with Items = items }

    let clearItems order =
        { order with Items = []}



    (*

    let order = { Id = 1; Items = [ { ProductId = 1; Quantity = 1} ] }
    let newItemExistingProduct = { ProductId = 1; Quantity = 1 }
    let newItemNewProduct = { ProductId = 2; Quantity = 2}

    addItem newItemNewProduct order =
        { Id = 1; Items = [ { ProductId = 1; Quantity = 1 };{ ProductId = 2; Quantity = 2 } ] }

    addItem newItemExistingProduct order =
        { Id = 1; Items = [{ProductId = 1; Quantity = 3}]}
    *)