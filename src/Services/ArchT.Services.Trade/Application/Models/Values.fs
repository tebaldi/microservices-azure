module Values

open System

type Money = decimal

type Amount = int

type OccurrenceTime = DateTime

type OrderId = string

type ProductId = string

type Item = { ProductId:ProductId; Quantity:Amount; Price:Money; }