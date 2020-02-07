using ArchT.Services.Inventory.Application.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ArchT.Services.Inventory.Infrastructure.Database.InventoryDb;

namespace ArchT.Services.Inventory.Infrastructure.Database
{
    abstract class DbDocument
    {
        public DbDocument() { Events = new List<dynamic>(); }
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public List<dynamic> Events { get; set; }
        public abstract string CollectionName { get; }

        public void CompressEvents(int maxEvents = 10)
        {
            switch (Events.Count)
            {
                case var c when c > maxEvents:
                    Events = Events.Skip(Math.Max(0, c - maxEvents)).ToList();
                    break;

            }
        }
    }
}
