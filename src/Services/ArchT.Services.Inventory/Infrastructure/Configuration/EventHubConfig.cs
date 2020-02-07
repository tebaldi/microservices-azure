using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArchT.Services.Inventory.Infrastructure.Configuration
{
    public class EventHubConfig
    {
        public string EventHubConnectionString { get; set; }
        public string StorageConnectionString { get; set; }
        public string StorageContainerName { get; set; }
    }
}
