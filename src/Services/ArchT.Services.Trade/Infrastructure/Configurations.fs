module Configurations

type DbConfig(endpoint, keys, database) =
    member val AccountEndpoint=endpoint with get, set
    member val AccountKeys=keys with get, set
    member val DatabaseId=database with get, set
    new() = DbConfig("", "", "")

type EventHubConfig() =
    member val EventHubConnectionString="" with get, set
    member val StorageConnectionString="" with get, set
    member val StorageContainerName="" with get, set       