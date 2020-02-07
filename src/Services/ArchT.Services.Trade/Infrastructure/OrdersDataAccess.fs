module OrdersDataAccess

open Values
open Database
open Events
open DocumentCollections
open DataContracts
open EventHub
open EventContracts

let private mapDocumentEvents (document:OrderDocument) (newEvents:OrderEvent list) =
    let mapf i =
        match i with
        | ItemAdded evt ->            
            document.Lines <- document.Lines @ [OrderLineDocument(evt.OrderId, evt.ProductId, evt.Quantity, evt.Price)]
            document.Events <- document.Events @ [evt]
        | ItemRemoved evt ->             
            document.Lines <- document.Lines |> List.filter (fun i' -> i'.ProductId <> evt.ProductId)
            document.Events <- document.Events @ [evt]
        | Processed evt ->
            document.ProcessedTime <- Some(evt.ProcessedTime)
            document.Events <- document.Events @ [evt]
    List.map mapf newEvents |> ignore
    document

let private mapOrderData (document:OrderDocument) =
    let items = List.map (fun (i:OrderLineDocument) -> ItemData(i.ProductId, i.Quantity, i.Price)) document.Lines
    OrderData(document.id, items, document.ProcessedTime.IsSome)
    
let private mapEventContract (document:OrderDocument) =
    let lines = List.map (fun (i:OrderLineDocument) -> OrderLineUpdated(i.ProductId, i.Quantity, i.Price)) document.Lines
    OrderUpdated(document.id, lines, document.ProcessedTime)

let getOrdersDataAsync (db:TradeDb) = async {
    let! documents = db.QueryAsync<OrderDocument> OrdersCollection
    let result = List.map mapOrderData documents
    return result
}

let loadOrderAsync (db:TradeDb) (orderId:OrderId) = async {
    let! response = db.FindItemAsync<OrderDocument> OrdersCollection orderId
    match response with 
    | null -> return { OrderId=orderId; Lines=[]; ProcessedTime=None; }
    | _ -> 
        let mapf (i:OrderLineDocument) = { OrderId=orderId; ProductId=i.ProductId; Quantity=i.Quantity; Price=i.Price; }
        let items = List.map mapf response.Document.Lines
        return { OrderId=orderId; Lines=items; ProcessedTime=response.Document.ProcessedTime; }
}

let storeOrderAsync (db:TradeDb) (publisher:EventPublisher) ((orderId:OrderId), (event:OrderEvent)) = async {
    let! response = db.FindItemAsync<OrderDocument> OrdersCollection orderId
    match response with 
    | null ->        
        let document = mapDocumentEvents (OrderDocument(orderId, [], None, [])) [event]
        let eventContract = mapEventContract document
        do! db.CreateItemAsync<OrderDocument> OrdersCollection document
        do! publisher.PublishAsync OrdersTopic eventContract orderId
        if document.ProcessedTime.IsSome
        then do! publisher.PublishAsync OrdersProcessingTopic eventContract orderId
    | _ ->
        let document = mapDocumentEvents response.Document [event]    
        let eventContract = mapEventContract document        
        do! db.UpdateItemAsync<OrderDocument> OrdersCollection orderId document
        do! publisher.PublishAsync OrdersTopic eventContract orderId
        if document.ProcessedTime.IsSome
        then do! publisher.PublishAsync OrdersProcessingTopic eventContract orderId
}