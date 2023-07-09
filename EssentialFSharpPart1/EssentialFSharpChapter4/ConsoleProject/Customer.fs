//namespace MyProject.Customer // Namespace.Namespace
module Myproject.Customer // Namespace.Module

type Customer = {
    Id: int
    IsVip: bool
    Credit: decimal
}

// Customer -> (Customer * decimal)
let getPurchanges customer =
    let purchases = if customer.Id % 2 = 0 then 120M else 80M
    (customer, purchases)

// (Customer * decimal) -> Customer
let tryPromoteToVip purchases =
    let (customer, amount) = purchases
    if amount > 100M then { customer with IsVip = true }
    else customer

// Customer -> Customer
let increaseCreditIfVip customer =
    let increase = if customer.IsVip then 100M else 50M
    { customer with Credit = customer.Credit + increase }

// Customer -> Customer
let upgradeCustomer customer =
    customer
    |> getPurchanges
    |> tryPromoteToVip
    |> increaseCreditIfVip

///////////////////From Eariler in the Chapter...////////////////
(*

namespace MyProject.Customer // Namespace.Namespace

open System

// Customer record type is used by the modules below
// So it is defined in the scope of the namespace
// modules have access to the types in the namespace without having to add and import declaration
type Customer = {
    Name: string
}

module Domain =
    // string -> Customer
    let create (name:string) =
        {Name = name}

module Db =
    open System.IO

    // Customer -> bool
    let save (customer:Customer) =
        // Imagine this talks to a database
        ()

*)