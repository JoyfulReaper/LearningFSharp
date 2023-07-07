namespace OrderTaking.Domain

// Types follow

// Product Code Related
type WidgetCode = WidgetCode of string
    // Constraint: Starting with W and then 4 digits
type GizmoCode = GizmoCode of string
    // Constraint: Starting with G and then 3 digits
type ProductCode =
    | Widget of WidgetCode
    | Gizmo of GizmoCode

// Order Quantity Related
type UnitQuantity = UnitQuantity of int
type KilogramQuantity = KilogramQuantity of decimal
type OrderQuantity = 
    | Unit of UnitQuantity
    | Kilos of KilogramQuantity

type OrderId = Undefined
type OrderLineId = Undefined
type CustomerId = Undefined

type CustomerInfo = Undefined
type ShippingAddress = Undefined
type BillingAddress = Undefined
type Price = Undefined
type BillingAmount = Undefined

type Order = {
    Id: OrderId
    CustomerId: CustomerId
    ShippingAddress: ShippingAddress
    BillingAddress: BillingAddress
    OrderLines: OrderLine list
    AmountToBill: BillingAmount
}
and OrderLine = {
    Id: OrderLineId
    OrderId: OrderId
    ProductCode: ProductCode
    OrderQuantity: OrderQuantity
    Price: Price
}

(*
type UnvalidatedOrder = {
    OrderId: string
    CustomerInfo: ...
    ShippingAddress: ...
    ...
}

type PlaceOrderEvents = {
    AcknowledgementSent: ...
    OrderPlaced...
    BillableOrderPlaced...
}

type PlaceOrderError =
 | ValidationError of ValidationError list
 | ..// Other errors
 and ValidationError = {
    FieldName: string
    ErrorDescription: string
 }

 /// The "Place Order" process
 type PlaceOrder =
    UnvalidatedOrder -> Result<PlaceOrderEvents,PlaceOrderError>
 *)