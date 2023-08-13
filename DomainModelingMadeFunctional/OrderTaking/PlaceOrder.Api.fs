module OrderTaking.PlaceOrder.Api

// ======================================================
// This file contains the JSON API interface to the PlaceOrder workflow
//
// 1) The HttpRequest is turned into a DTO, which is then turned into a Domain object
// 2) The main workflow function is called
// 3) The output is turned into a DTO which is turned into a HttpResponse
// ======================================================


open Newtonsoft.Json
open OrderTaking.Common
open OrderTaking.PlaceOrder

type JsonString = string

/// Very simplified version!
type HttpRequest = {
    Action : string
    Uri : string
    Body : JsonString 
    }

/// Very simplified version!
type HttpResponse = {
    HttpStatusCode : int
    Body : JsonString 
    }

/// An API takes a HttpRequest as input and returns a async response
type PlaceOrderApi = HttpRequest -> Async<HttpResponse>

// =============================
// Implementation
// =============================

// setup dummy dependencies    
// TODO experiment with putting these in a library and using them as dependencies here

let checkProductExists : Implementation.CheckProductCodeExists =
    fun productCode ->
        true // dummy implmentation

let checkAddressExists : Implementation.CheckAddressExists =
    fun unvalidatedAddress -> 
        let checkedAddress = Implementation.CheckedAddress unvalidatedAddress 
        AsyncResult.retn checkedAddress 

let getProductPrice : Implementation.GetProductPrice =
    fun productCode ->
        Price.unsafeCreate 1M // dummy implementation

let createOrderAcknowledgmentLetter : Implementation.CreateOrderAcknowledgmentLetter =
    fun pricedOrder ->
        let letterTest = Implementation.HtmlString "some text"
        letterTest 

let sendOrderAcknowledgment : Implementation.SendOrderAcknowledgment =
    fun orderAcknowledgement ->
        Implementation.Sent 

// -------------------------------
// workflow
// -------------------------------

/// This function converts the workflow output into a HttpResponse