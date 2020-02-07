module Exceptions

type CannotAdd = ItemToProcessedOrder of string
exception CannotAddException of CannotAdd

type CannotRemove = ItemToEmptyOrder of string | ItemToProcessedOrder of string
exception CannotRemoveException of CannotRemove

type CannotProcess = EmptyOrder of string | ProcessedOrder of string
exception CannotProcessException of CannotProcess

type InvalidOrderState = { Expected:string; Actual:string }
exception OrderStateException of InvalidOrderState