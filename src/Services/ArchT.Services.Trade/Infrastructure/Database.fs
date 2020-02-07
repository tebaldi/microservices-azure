module Database

open System
open Configurations
open Microsoft.Azure.Documents
open Microsoft.Azure.Documents.Client
open Microsoft.Azure.Documents.Linq
open System.Linq
open Microsoft.Extensions.Configuration

let private openClient endpoint (authKey:string) = new DocumentClient(Uri(endpoint), authKey)

let private createDatabase (config:DbConfig) =
    let client = openClient config.AccountEndpoint config.AccountKeys
    try
        let uri = UriFactory.CreateDatabaseUri(config.DatabaseId)
        client.ReadDatabaseAsync(uri).Wait()  
    with :? AggregateException as aex ->
        match aex.Flatten().InnerException with                
        | :? DocumentClientException as ex ->
            match ex.StatusCode.Value with
            | System.Net.HttpStatusCode.NotFound ->
                let db = Microsoft.Azure.Documents.Database()
                db.Id <- config.DatabaseId
                client.CreateDatabaseAsync(db).Wait()
            | _ -> raise(ex)
        | _ -> raise(aex)

let private createCollection (config:DbConfig) collectionId =
    let client = openClient config.AccountEndpoint config.AccountKeys
    try
        let uri = UriFactory.CreateDocumentCollectionUri(config.DatabaseId, collectionId)
        client.ReadDocumentCollectionAsync(uri).Wait()  
    with :? AggregateException as aex ->
        match aex.Flatten().InnerException with                
        | :? DocumentClientException as ex ->
            match ex.StatusCode.Value with
            | System.Net.HttpStatusCode.NotFound ->
                let dburi = UriFactory.CreateDatabaseUri(config.DatabaseId)
                let docCollection = DocumentCollection()
                docCollection.Id <- collectionId                
                client.CreateDocumentCollectionAsync(dburi, docCollection).Wait()
            | _ -> raise(ex)
        | _ -> raise(aex)

let private readDocumentAsync<'T> (config:DbConfig) collectionId id = async {
    let client = openClient config.AccountEndpoint config.AccountKeys
    let uri = UriFactory.CreateDocumentUri(config.DatabaseId, collectionId, id)
    try
        let document = client.ReadDocumentAsync<'T>(uri).Result
        return document
    with _ -> return null
}

let private queryAsync<'T> (config:DbConfig) collectionId = async {
    let client = openClient config.AccountEndpoint config.AccountKeys
    let uri = UriFactory.CreateDocumentCollectionUri(config.DatabaseId, collectionId)
    let feedOptions = FeedOptions()
    feedOptions.MaxItemCount <- Nullable(-1)
    let (query:IDocumentQuery<'T>) = client.CreateDocumentQuery<'T>(uri, feedOptions).AsDocumentQuery()
    let mutable (results:'T list) = []
    while query.HasMoreResults do
        let partial = query.ExecuteNextAsync<'T>().Result;
        let list = Seq.cast partial |> Seq.toList
        results <- results @ list
    return results
}

let private createItemAsync<'T> (config:DbConfig) collectionId item = async {
    let client = openClient config.AccountEndpoint config.AccountKeys
    let uri = UriFactory.CreateDocumentCollectionUri(config.DatabaseId, collectionId)
    client.CreateDocumentAsync(uri, item).Wait()
}

let private updateItemAsync<'T> (config:DbConfig) collectionId id item = async {
    let client = openClient config.AccountEndpoint config.AccountKeys
    let uri = UriFactory.CreateDocumentUri(config.DatabaseId, collectionId, id)
    client.ReplaceDocumentAsync(uri, item).Wait()
}

let private deleteItemAsync<'T> (config:DbConfig) collectionId id = async {
    let client = openClient config.AccountEndpoint config.AccountKeys
    let uri = UriFactory.CreateDocumentCollectionUri(config.DatabaseId, collectionId)
    client.DeleteDocumentAsync(uri, id).Wait()
}

let private mapConfig (configuration:IConfiguration) =
    let config = DbConfig()
    do config.AccountEndpoint <- configuration.GetSection("DbConfig").GetValue("AccountEndpoint")
    do config.AccountKeys <- configuration.GetSection("DbConfig").GetValue("AccountKeys")
    do config.DatabaseId <- configuration.GetSection("DbConfig").GetValue("DatabaseId")
    config

let OrdersCollection = "Orders"

type TradeDb(configuration) =
    let dbConfig = mapConfig configuration
    do createDatabase dbConfig; do createCollection dbConfig OrdersCollection
    member this.QueryAsync<'T> collectionId = queryAsync<'T> dbConfig collectionId
    member this.FindItemAsync<'T> collectionId id = readDocumentAsync<'T> dbConfig collectionId id
    member this.CreateItemAsync<'T> collectionId item = createItemAsync<'T> dbConfig collectionId item
    member this.UpdateItemAsync<'T> collectionId id item = updateItemAsync<'T> dbConfig collectionId id item
    member this.DeleteItemAsync<'T> collectionId item = deleteItemAsync<'T> dbConfig collectionId item
