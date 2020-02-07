module QueryContracts

open DataContracts
open MediatR
open System.Collections.Generic

type FindOrderRequest() =
    interface IRequest<OrderData>
    member val OrderId="" with get, set

type GetOdersRequest() =
    interface IRequest<IEnumerable<OrderData>>
