module Orders

open Values
open Exceptions
open Events
open System

type EmptyState = { Id:OrderId; }

type ActiveState = { Id:OrderId; Lines:Item list; }

type ProcessedState = { Id:OrderId; Lines:Item list; ProcessedTime:OccurrenceTime; }

type Order =
    | Empty of EmptyState
    | Active of ActiveState
    | Processed of ProcessedState

type CompletedOrderResult = { Order:Order; Result:OrderEvent; }

type FailedOrderResult = { Error:Exception; }

type OrderResult =
    | Completed of CompletedOrderResult
    | Failed of FailedOrderResult

type EmptyState with
    member this.Add item = Order.Active { Id=this.Id; Lines=[item] }        

type ActiveState with
    member this.Add item = Order.Active { this with Lines=item :: this.Lines }

    member this.Remove productId =
        let lines = this.Lines |> List.filter (fun i -> i.ProductId <> productId)
        match lines with
        | [] -> Order.Empty { Id=this.Id }
        | _ -> Order.Active { this with Lines=lines }           

    member this.Process =
        Order.Processed { Id=this.Id; Lines=this.Lines; ProcessedTime=DateTime.UtcNow; }

type Order with
    static member Load (snapshot:OrderLoaded) =
        let mapf (i:OrderLineLoaded) = { ProductId=i.ProductId; Quantity=i.Quantity; Price=i.Price; }
        let lines = List.map mapf snapshot.Lines
        match lines with
        | [] -> Order.Empty { Id=snapshot.OrderId }
        | _ -> 
            match snapshot.ProcessedTime  with
            | Some datetime -> 
                Order.Processed { Id=snapshot.OrderId; Lines=lines; ProcessedTime=datetime } 
            | None -> Order.Active { Id=snapshot.OrderId; Lines=lines; }

    member this.Add item = 
        match this with
        | Empty state -> 
            let order = state.Add item
            let result = OrderEvent.ItemAdded (ItemAddedToOrder(state.Id, item.ProductId, item.Quantity, item.Price))
            OrderResult.Completed { Order=order; Result=result; }
        | Active state -> 
            let order = state.Add item
            let result = OrderEvent.ItemAdded (ItemAddedToOrder(state.Id, item.ProductId, item.Quantity, item.Price))
            OrderResult.Completed { Order=order; Result=result; }
        | Processed _ -> OrderResult.Failed { Error=(CannotAddException (CannotAdd.ItemToProcessedOrder (sprintf "%A" item))) }

    member this.Remove productId = 
        match this with
        | Empty _ -> OrderResult.Failed { Error=(CannotRemoveException (CannotRemove.ItemToEmptyOrder (sprintf "%A" productId))) }
        | Active state -> 
            let order = state.Remove productId
            let result = OrderEvent.ItemRemoved (ItemRemovedFromOrder(state.Id, productId))
            OrderResult.Completed { Order=order; Result=result; }
        | Processed _ -> OrderResult.Failed { Error=(CannotRemoveException (CannotRemove.ItemToProcessedOrder (sprintf "%A" productId))) }

    member this.Process =
        match this with
        | Empty state -> OrderResult.Failed { Error=(CannotProcessException (CannotProcess.EmptyOrder (sprintf "%A" state.Id))) }
        | Active state -> 
            let order = state.Process
            match order with 
            | Processed processed ->
                let result = OrderEvent.Processed (OrderProcessed(state.Id, processed.ProcessedTime))
                OrderResult.Completed { Order=order; Result=result; }
            | invalid ->
                OrderResult.Failed { Error=(OrderStateException { Expected="Processed"; Actual=invalid.GetType().Name }) }
        | Processed state -> OrderResult.Failed { Error=(CannotProcessException (CannotProcess.ProcessedOrder (sprintf "%A" state.Id))) }