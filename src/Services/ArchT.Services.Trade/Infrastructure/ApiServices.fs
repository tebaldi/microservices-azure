module ApiServices

open Database
open OrdersDataAccess
open Configurations
open Repositories
open OrderCommands
open EventHub

type OrdersService(config) =
    let db = TradeDb(config)
    let publisher = EventPublisher(config)
    let load orderId = loadOrderAsync db orderId
    let store (orderId, event) = storeOrderAsync db publisher (orderId, event)
    let repository = { Load=load; Store=store; }
    member this.GetOrdersAsync = getOrdersDataAsync db
    member this.AddItemToOrderAsync command = AddItemToOrder(repository).Execute command
    member this.RemoveItemFromOrderAsync command = RemoveItemFromOrder(repository).Execute command
    member this.ProcessOrderAsync command = ProcessOrder(repository).Execute command
