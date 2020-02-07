using ArchT.Services.Inventory.Infrastructure.EventHub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArchT.Services.Inventory.Infrastructure.Extensions
{
    static class EventTopicsExtensions
    {
        public static string Map(this EventTopics topic)
        {
            switch (topic)
            {
                case EventTopics.Products:
                    return "inventory-products";                

                default:
                    return default;
            }

        }
    }
}
