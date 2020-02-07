using ArchT.Services.Inventory.Application.Models;
using ArchT.Services.Inventory.Contracts;
using ArchT.Services.Inventory.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArchT.Services.Inventory.Infrastructure.Extensions
{
    static class ProductDocumentExtensions
    {
        public static ProductUpdated MapProductUpdated(this ProductDocument doc) =>
            new ProductUpdated { ProductId = doc.Id, Name = doc.Name, Stock = doc.Stock };

        public static ProductData MapProductData(this ProductDocument doc) =>
            new ProductData { ProductId = doc.Id, Name = doc.Name, Stock = doc.Stock };

        public static IEnumerable<ProductData> MapProductData(this IEnumerable<ProductDocument> docs) =>
            docs.Select(d => d.MapProductData());

        public static void MapEvents(this ProductDocument doc, params Event[] events)
        {
            foreach (var evt in events)
            {
                switch (evt)
                {
                    case ProductNameUpdated e:
                        doc.Name = e.NewValue;
                        break;

                    case ProductStockIncreased e:
                        doc.Stock = e.NewValue;
                        break;

                    case ProductStockDecreased e:
                        doc.Stock = e.NewValue;
                        break;

                }
            }
            doc.Events.AddRange(events);
            doc.CompressEvents();
        }
    }
}
