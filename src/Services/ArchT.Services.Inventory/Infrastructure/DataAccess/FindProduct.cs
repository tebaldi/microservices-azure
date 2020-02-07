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
    class FindProduct : IRequestHandler<FindProductRequest, QueryResponse<ProductData>>
    {
        private readonly InventoryDb db;
        public FindProduct(InventoryDb db) => this.db = db;

        public async Task<QueryResponse<ProductData>> Handle(FindProductRequest request, CancellationToken cancellationToken)
        {
            var record = await db.FindItemAsync<ProductDocument>(request.ProductId);
            return new QueryResponse<ProductData>()
            {
                Completed = true,
                Information = string.Empty,
                Data = record.MapProductData()
            };
        }
    }
}
