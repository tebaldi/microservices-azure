module DocumentCollections

open System

type OrderLineDocument(orderId:string, productId:string, quantity:int, price:decimal) =
    member val OrderId=orderId with get, set
    member val ProductId=productId with get, set
    member val Quantity=quantity with get, set
    member val Price=price with get, set
    new() = OrderLineDocument("", "", 0, 0M)

type OrderDocument(id:string, lines:OrderLineDocument list, processedTime: Option<DateTime>, events: obj list) =
    member val id = id with get, set
    member val Lines = lines with get, set
    member val ProcessedTime = processedTime with get, set
    member val Events = events with get, set
    new() = OrderDocument("", [], None, [])
        