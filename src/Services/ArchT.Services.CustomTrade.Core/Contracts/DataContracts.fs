module DataContracts

type ItemData(productId, quantity, price) =
    member val ProductId=productId with get, set
    member val Quantity=quantity with get, set
    member val Price=price with get, set
    new() = ItemData("", 0, 0M)

type OrderData(id, items, processed) =
    member val Id=id with get, set
    member val Items:ItemData list=items with get, set
    member val Processed=processed with get, set
    new() = OrderData("", [], false)