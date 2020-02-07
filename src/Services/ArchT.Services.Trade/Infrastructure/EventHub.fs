module EventHub

open System
open Configurations
open Microsoft.Azure.Documents
open Microsoft.Azure.Documents.Client
open Microsoft.Azure.Documents.Linq
open Microsoft.Extensions.Configuration
open Microsoft.Azure.EventHubs
open Newtonsoft.Json
open System.Text
open EventContracts
open Microsoft.Azure.EventHubs.Processor

let private openClient (config:EventHubConfig) topic =
    let builder = new EventHubsConnectionStringBuilder(config.EventHubConnectionString)
    builder.EntityPath <- topic
    EventHubClient.CreateFromConnectionString(builder.ToString())

let private createHost (config:EventHubConfig) topic =
    new EventProcessorHost(topic, PartitionReceiver.DefaultConsumerGroupName,
        config.EventHubConnectionString, config.StorageConnectionString, config.StorageContainerName)

let private publishAsync (config:EventHubConfig) (topic:string) (eventContract:EventContract) (partitionKey:string) = async {
    let client = openClient config topic
    try
        let json = JsonConvert.SerializeObject(eventContract)
        let data = new EventData(Encoding.UTF8.GetBytes(json))
        client.SendAsync(data, partitionKey).Wait()
    finally
        client.Close()
}

let private mapConfig (configuration:IConfiguration) =
    let config = EventHubConfig()
    do config.EventHubConnectionString <- configuration.GetSection("EventHubConfig").GetValue("EventHubConnectionString")
    do config.StorageConnectionString <- configuration.GetSection("EventHubConfig").GetValue("StorageConnectionString")
    do config.StorageContainerName <- configuration.GetSection("EventHubConfig").GetValue("StorageContainerName")
    config

let OrdersTopic = "trade-orders"
let OrdersProcessingTopic = "orders-processing"

type EventPublisher(configuration) =  
    let config = mapConfig configuration
    member this.PublishAsync topic eventContract partitionKey = publishAsync config topic eventContract partitionKey    

type EventConsumer(configuration, topic, factory) =
    let config = mapConfig configuration
    let host = createHost config topic
    member this.StartAsync = async { host.RegisterEventProcessorFactoryAsync(factory).Wait() }
    member this.StopAsync = async { host.UnregisterEventProcessorAsync().Wait() }