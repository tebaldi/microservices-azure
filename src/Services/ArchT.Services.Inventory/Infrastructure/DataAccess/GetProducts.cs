using ArchT.Services.Inventory.Contracts;
using ArchT.Services.Inventory.Infrastructure.Database;
using ArchT.Services.Inventory.Infrastructure.Extensions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ArchT.Services.Inventory.Infrastructure.DataAccess
{
    class GetProducts : IRequestHandler<GetProductsRequest, QueryResponse<IEnumerable<ProductData>>>
    {
        private readonly InventoryDb db;
        public GetProducts(InventoryDb db) => this.db = db;

        public async Task<QueryResponse<IEnumerable<ProductData>>> Handle(GetProductsRequest request, CancellationToken cancellationToken)
        {
            var records = await db.GetItemsAsync<ProductDocument>(_ => true);
            return new QueryResponse<IEnumerable<ProductData>>()
            {
                Completed = true,
                Information = string.Empty,
                Data = records.MapProductData()
            };
        }
    }
}
