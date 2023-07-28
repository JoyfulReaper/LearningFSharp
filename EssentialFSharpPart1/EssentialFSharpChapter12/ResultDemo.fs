namespace ComputationExpression

module ResultDemo =
    open FsToolkit.ErrorHandling

    type Customer = {
        Id : int
        IsVip : bool
        Credit : decimal
    }

    // Customer -> Result<(Customer * decimal, exn>
    let getPurchases customer =
        try
            // Imagine this function is fetching data from a database
            let purchases =
                if customer.Id % 2 = 0 then (customer, 120M) else (customer,80M)
            Ok purchases
        with
        | ex -> Error ex

    // Customer * decimal -> Customer
    let tryPromoteToVip purchases =
        let customer, amount = purchases
        if amount > 100M then { customer with IsVip = true}
        else customer

    // Customer -> Result<Customer,exn>
    let increaseCreditIfVip customer =
        try
            // Image this function could cause an exception
            let increase = if customer.IsVip then 100M else 50M
            Ok { customer with Credit = customer.Credit + increase}
        with
        | ex -> Error ex

    let upgradeCustomer customer =
        customer
        |> getPurchases
        |> Result.map tryPromoteToVip
        |> Result.bind increaseCreditIfVip

    let upgradeCustomer' customer =
        result {
            let! purchases = getPurchases customer
            let promoted = tryPromoteToVip purchases
            return! increaseCreditIfVip promoted
        }