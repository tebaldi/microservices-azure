module Events

open System

type OrderLineLoaded = { OrderId:string; ProductId:string; Quantity:int; Price:decimal; }
type OrderLoaded = { OrderId:string; Lines:OrderLineLoaded list; ProcessedTime:Option<DateTime>; }

type DomainEvent() =
    member this.EventName=this.GetType().Name
    member this.EventOcurrency=DateTime.UtcNow
    member this.EventSequence=this.EventOcurrency.Ticks


type ItemAddedToOrder(orderId:string, productId:string, quantity:int, price:decimal) = 
    inherit DomainEvent()
    member this.OrderId=orderId;
    member this.ProductId=productId;
    member this.Quantity=quantity;
    member this.Price=price;

type ItemRemovedFromOrder(orderId:string, productId:string) =
    inherit DomainEvent()
    member this.OrderId=orderId;
    member this.ProductId=productId;

type OrderProcessed(orderId:string, processedTime:DateTime) =
    inherit DomainEvent()
    member this.OrderId=orderId;
    member this.ProcessedTime=processedTime

type OrderEvent = 
    | ItemAdded of ItemAddedToOrder
    | ItemRemoved of ItemRemovedFromOrder
    | Processed of OrderProcessed