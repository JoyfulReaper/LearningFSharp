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
    
    // 1 - Prepend new item to existing order items
    // 2 - Consolidate each product
    // 3 - Sort items in order by productid to make equality simpler
    // 4 - Update order with new list of items
    let addItem item order =
        let items = 
            item::order.Items
            |> List.groupBy (fun i -> i.ProductId) // (int * Item list) list
            |> List.map (fun (id, items) ->
                { ProductId = id; Quantity = items |> List.sumBy (fun i -> i.Quantity)})
            |> List.sortBy (fun i -> i.ProductId)
        { order with Items = items }

    (*
    let order = { Id = 1; Items = [ { ProductId = 1; Quantity = 1} ] }
    let newItemExistingProduct = { ProductId = 1; Quantity = 1 }
    let newItemNewProduct = { ProductId = 2; Quantity = 2}

    addItem newItemNewProduct order =
        { Id = 1; Items = [ { ProductId = 1; Quantity = 1 };{ ProductId = 2; Quantity = 2 } ] }

    addItem newItemExistingProduct order =
        { Id = 1; Items = [{ProductId = 1; Quantity = 3}]}
    *)