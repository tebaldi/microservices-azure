namespace ArchT.Services.Trade.Controllers

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore.Mvc
open ApiServices
open DataContracts
open Microsoft.Extensions.Configuration
open Configurations
open CommandContracts

[<Route("api/v1/trade"); ApiController>]
type TradeController (configuration:IConfiguration) =
    inherit ControllerBase()

    [<HttpGet; Route("orders")>]
    member this.GetOrders() = async {
        let! response = OrdersService(configuration).GetOrdersAsync
        return response
    }

    [<HttpPost; Route("orders/{id}/add-item")>]
    member this.AddItemToOrder([<FromBody>] request:AddItemToOrderRequest) = async {
        let! response = OrdersService(configuration).AddItemToOrderAsync request
        return response
    }

    [<HttpPost; Route("orders/{id}/remove-item")>]
    member this.RemoveItemFromOrder([<FromBody>] request:RemoveItemFromOrderRequest) = async {
        let! response = OrdersService(configuration).RemoveItemFromOrderAsync request
        return response
    }

    [<HttpPost; Route("orders/{id}/process")>]
    member this.ProcessOrder([<FromBody>] request:ProcessOrderRequest) = async {
        let! response = OrdersService(configuration).ProcessOrderAsync request
        return response
    }
