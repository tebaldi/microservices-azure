module OrderCommands

open CommandContracts
open Values
open Orders
open Repositories

type AddItemToOrder(repository:OderRepository) =
    member this.Execute (command:AddItemToOrderRequest) = async {
        let! snaphost = repository.Load command.OrderId
        let order = Order.Load snaphost
        let item = { ProductId=command.Item.ProductId; Quantity=command.Item.Quantity; Price=command.Item.Price }
        let result = order.Add item
        match result with
        | Completed completed ->
            do! repository.Store (command.OrderId, completed.Result)
            return { Completed=true; Information=completed.Result.ToString(); Data=completed.Result }
        | Failed failed -> return { Completed=false; Information=failed.Error.GetType().Name; Data=failed.Error.Message }
    }

type RemoveItemFromOrder(repository:OderRepository) =
    member this.Execute (command:RemoveItemFromOrderRequest) = async {
        let! snaphost = repository.Load command.OrderId
        let order = Order.Load snaphost
        let result = order.Remove command.ItemId
        match result with
        | Completed completed ->
            do! repository.Store (command.OrderId, completed.Result)
            return { Completed=true; Information=completed.Result.ToString(); Data=completed.Result }
        | Failed failed -> return { Completed=false; Information=failed.Error.GetType().Name; Data=failed.Error.Message }
    }

type ProcessOrder(repository:OderRepository) =
    member this.Execute (command:ProcessOrderRequest) = async {
        let! snaphost = repository.Load command.OrderId
        let order = Order.Load snaphost
        let result = order.Process
        match result with
        | Completed completed ->
            do! repository.Store (command.OrderId, completed.Result)
            return { Completed=true; Information=completed.Result.ToString(); Data=completed.Result }
        | Failed failed -> return { Completed=false; Information=failed.Error.GetType().Name; Data=failed.Error.Message }
    }