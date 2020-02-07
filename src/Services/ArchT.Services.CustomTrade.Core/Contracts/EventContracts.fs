module EventContracts

open System

type EventContract() as this =
    let time = DateTime.UtcNow
    member val EventName=this.GetType().Name with get, set
    member val EventTime=time with get, set
    member val EventSequence=time.Ticks with get, set

type OrderLineUpdated(productId, quantity, price) =
    inherit EventContract()
    member val ProductId=productId with get, set
    member val Quantity=quantity with get, set
    member val Price=price with get, set
    new() = OrderLineUpdated("", 0, 0M)

type OrderUpdated(id:string, items:OrderLineUpdated list, processedTime:Option<DateTime>) =
    inherit EventContract()  
    member val Id=id with get, set
    member val Lines=items with get, set
    member val Processed=processedTime with get, set
    new() = OrderUpdated("", [], None)