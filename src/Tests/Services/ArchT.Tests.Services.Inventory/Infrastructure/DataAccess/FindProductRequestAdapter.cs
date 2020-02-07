using ArchT.Services.Inventory.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ArchT.Tests.Services.Inventory.Infrastructure.DataAccess
{
    class FindProductRequestAdapter : IRequestHandler<FindProductRequest, QueryResponse<ProductData>>
    {
        public Task<QueryResponse<ProductData>> Handle(FindProductRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new QueryResponse<ProductData>()
            {
                Completed = true,
                Information = string.Empty,
                Data = new ProductData { ProductId=request.ProductId, Name = $"Product {request.ProductId}", Stock = 10 }
            });
        }
    }
}
