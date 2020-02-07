using ArchT.Services.Search.Infrastructure.EventHub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArchT.Services.Search.Infrastructure.Extensions
{
    static class EventTopicsExtensions
    {
        public static string Map(this EventTopics topic)
        {
            switch (topic)
            {
                case EventTopics.Products:
                    return "inventory-products";

                case EventTopics.Orders:
                    return "trade-orders";

                default:
                    return default;
            }

        }
    }
}
