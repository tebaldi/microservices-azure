using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArchT.Services.OrderProcessor.Infrastructure.Database
{
    abstract class DbDocument
    {
        public DbDocument() { Events = new List<dynamic>(); }
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public List<dynamic> Events { get; set; }
        public abstract string CollectionName { get; }
    }
}
