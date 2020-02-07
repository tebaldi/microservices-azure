using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArchT.Services.OrderProcessor.Infrastructure.Database
{
    class OrderProcessedDocument : DbDocument
    {
        public dynamic Data { get; set; }
        public override string CollectionName => "OrdersProcessed";
    }
}
