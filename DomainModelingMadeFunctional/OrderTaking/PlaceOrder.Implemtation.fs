module internal OrderTaking.PlaceOrder.ImplementationWithoutEffects

open OrderTaking.Common

// ======================================================
// This file contains the implementation for the PlaceOrder workflow
// WITHOUT any effects like Result or Async
//
// This represents the code in chapter 9, "Composing a Pipeline"
//
// There are two parts:
// * the first section contains the (type-only) definitions for each step
// * the second section contains the implementations for each step
//   and the implementation of the overall workflow
// ======================================================


// ------------------------------------
// the workflow itself, without effects

type PlaceOrderWithoutEffects = 
    UnvalidatedOrder -> PlaceOrderEvent list

// ======================================================
// Override the SimpleType constructors 
// so that they raise exceptions rather than return Results
// ======================================================

// helper to convert Results into exceptions so we can reuse the smart constructors in SimpleTypes.
let failOnError aResult =
    match aResult with
    | Ok success -> success
    | Error error -> failwithf "%A" error

module String50 =
    let create fieldName = String50.create fieldName >> failOnError
    let createOption fieldName = String50.createOption fieldName >> failOnError

module EmailAddress =
    let create fieldName = EmailAddress.create fieldName >> failOnError

module ZipCode =
    let create fieldName = ZipCode.create fieldName >> failOnError

module OrderId =
    let create fieldName = OrderId.create fieldName >> failOnError

module OrderLineId =
    let create fieldName = OrderLineId.create fieldName >> failOnError

module WidgetCode =
    let create fieldName = WidgetCode.create fieldName >> failOnError

module GizmoCode =
    let create fieldName = GizmoCode.create fieldName >> failOnError

module ProductCode =
    let create fieldName = ProductCode.create fieldName >> failOnError

module UnitQuantity  =
    let create fieldName = UnitQuantity.create fieldName >> failOnError

module KilogramQuantity =
    let create fieldName = KilogramQuantity.create fieldName >> failOnError

module OrderQuantity  =
    let create fieldName productCode = OrderQuantity.create fieldName productCode >> failOnError

module Price =
    let create = Price.create >> failOnError
    let multiply qty price = Price.multiply qty price |> failOnError

module BillingAmount =
    let create = BillingAmount.create >> failOnError
    let sumPrices = BillingAmount.sumPrices >> failOnError

// ======================================================
// Section 1 : Define each step in the workflow using types
// ======================================================

// ---------------------------
// Validation step
// ---------------------------

// Product validation

type CheckProductCodeExists =
    ProductCode -> bool

// Address validation Exception
exception AddressValidationFailure of string

type CheckedAddress = CheckedAddress of UnvalidatedAddress

type CheckAddressExists =
    UnvalidatedAddress -> CheckedAddress

// ---------------------------
// Validated Order 
// ---------------------------

type ValidatedOrderLine = {
    OrderLineId : OrderLineId
    ProductCode : ProductCode
    Quantity : OrderQuantity
}

type ValidatedOrder = {
    OrderId : OrderId
    CustomerInfo : CustomerInfo
    ShippingAddress : Address
    BillingAddress : Address
    Line : ValidatedOrderLine list
}

type ValidateOrder = 
    CheckProductCodeExists  // dependency
      -> CheckAddressExists // dependency
      -> UnvalidatedOrder   // input
      -> ValidatedOrder     // output

// ---------------------------
// Pricing step
// ---------------------------