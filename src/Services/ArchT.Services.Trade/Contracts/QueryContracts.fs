module QueryContracts

open DataContracts

type FindOrderRequest = { OrderId:int; }

type FindOrderResponse =
    | Data of OrderData
    | None

type GetOdersRequest = NoFilter

type GetOdersResponse = 
    | Data of OrderData list
    | None