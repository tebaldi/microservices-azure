using ArchT.Services.Inventory.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ArchT.Tests.Services.Inventory.Infrastructure.DataAccess
{
    class GetProductsRequestAdapter : IRequestHandler<GetProductsRequest, QueryResponse<IEnumerable<ProductData>>>
    {
        public Task<QueryResponse<IEnumerable<ProductData>>> Handle(GetProductsRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new QueryResponse<IEnumerable<ProductData>>()
            {
                Completed = true,
                Information = string.Empty,
                Data = new[]
                {
                    new ProductData { ProductId="P1", Name = "Product 1", Stock = 10 },
                    new ProductData { ProductId="P2", Name = "Product 2", Stock = 3 },
                }
            });
        }
    }
}
