module CommandContracts

open DataContracts

type CommandResponse = { Completed:bool; Information:string; Data:obj; }

type AddItemToOrderRequest(orderId, item) = 
    member val OrderId=orderId with get, set
    member val Item=item with get, set
    new()=AddItemToOrderRequest("", ItemData())

type RemoveItemFromOrderRequest() =
    member val OrderId="" with get, set
    member val ItemId="" with get, set

type ProcessOrderRequest() =
    member val OrderId="" with get, set