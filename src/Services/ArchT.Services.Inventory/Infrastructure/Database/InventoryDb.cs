using ArchT.Services.Inventory.Infrastructure.Configuration;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ArchT.Services.Inventory.Infrastructure.Database
{
    class InventoryDb : IDisposable
    {
        private readonly DbConfig config = new DbConfig();
        private readonly DocumentClient client;

        public InventoryDb(IConfiguration configuration)
        {
            configuration.Bind(nameof(DbConfig), config);

            this.client = new DocumentClient(new Uri(config.AccountEndpoint), config.AccountKeys);
            CreateDatabaseIfNotExistsAsync().Wait();
            CreateCollectionIfNotExistsAsync("Products").Wait();
        }

        public async Task<T> FindItemAsync<T>(string id) where T : DbDocument
        {
            try
            {
                var collectionId = GetCollectionId<T>();
                Document document = await client.ReadDocumentAsync(UriFactory.CreateDocumentUri(config.DatabaseId, collectionId, id));
                return (T)(dynamic)document;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return default;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<IEnumerable<T>> GetItemsAsync<T>(Expression<Func<T, bool>> predicate) where T : DbDocument
        {
            var collectionId = GetCollectionId<T>();

            var query = client.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(config.DatabaseId, collectionId),
                new FeedOptions { MaxItemCount = -1 })
                .Where(predicate)
                .AsDocumentQuery();

            var results = new List<T>();
            while (query.HasMoreResults)
                results.AddRange(await query.ExecuteNextAsync<T>());

            return results;
        }

        public async Task<Document> CreateItemAsync<T>(T item) where T : DbDocument
        {
            var collectionId = GetCollectionId<T>();
            return await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(config.DatabaseId, collectionId), item);
        }

        public async Task<Document> UpdateItemAsync<T>(string id, T item) where T : DbDocument
        {
            var collectionId = GetCollectionId<T>();
            return await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(config.DatabaseId, collectionId, id), item);
        }

        public async Task DeleteItemAsync<T>(string id) where T: DbDocument
        {
            var collectionId = GetCollectionId<T>();
            await client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(config.DatabaseId, collectionId, id));
        }

        private async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                await client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(config.DatabaseId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDatabaseAsync(new Microsoft.Azure.Documents.Database { Id = config.DatabaseId });
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task CreateCollectionIfNotExistsAsync(string collectionId)
        {
            try
            {
                await client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(config.DatabaseId, collectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    try
                    {
                        await client.CreateDocumentCollectionAsync(
                            UriFactory.CreateDatabaseUri(config.DatabaseId),
                            new DocumentCollection { Id = collectionId });
                    }
                    catch (Exception ex)
                    { throw ex; }
                }
                else
                {
                    throw;
                }
            }
        }

        private string GetCollectionId<T>() where T: DbDocument
        {
            var instance = (T)typeof(T).GetConstructors().First().Invoke(new object[] { });
            return instance.CollectionName;
        }

        public void Dispose()
        {
            client?.Dispose();
        }
    }
}
