namespace ArchT.Tests.Services.Trade

module AddItemToOrderTests =
    open System
    open Xunit    
    open Events
    open DataContracts
    open CommandContracts
    open OrderCommands
    open Repositories

    let ``Given I have created an order with id`` id continuation =
        let command = AddItemToOrderRequest(id, new ItemData())
        continuation command

    let ``And I have added an item`` (command:AddItemToOrderRequest) item continuation =
        let command' = AddItemToOrderRequest(command.OrderId, item)
        let load id = async { return { OrderId=id; Lines=[]; ProcessedTime=None; } }
        let store (id, event) = async { return (); } 
        let repository = { Load=load; Store=store; }
        let processor = new AddItemToOrder(repository)
        let response = processor.Execute command' |> Async.RunSynchronously
        match response.Completed with
            | true -> continuation response
            | false -> Assert.True(false, response.Information)

    let ``The command should complete succefully`` response = printfn "%A" response

    [<Fact>]
    let ``Add Item on Empty Order Test`` () =
        ``Given I have created an order with id`` "orderId"
         ``And I have added an item`` (ItemData("productId", 10, 50M))
         ``The command should complete succefully``
