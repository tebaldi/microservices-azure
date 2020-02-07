module Repositories

open Values
open Events

type OderRepository = {
    Load: OrderId -> Async<OrderLoaded>
    Store: OrderId * OrderEvent -> Async<unit>
}