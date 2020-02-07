using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ArchT.Services.Inventory.Contracts
{
    public class QueryRequest<TResponse> : IRequest<QueryResponse<TResponse>> { }

    public class QueryResponse
    {
        public bool Completed { get; set; }
        public string Information { get; set; }
    }

    public class QueryResponse<TResponse> : QueryResponse
    {   
        public TResponse Data { get; set; }
    }

    public class GetProductsRequest : QueryRequest<IEnumerable<ProductData>>
    {
    }

    public class FindProductRequest : QueryRequest<ProductData>
    {
        public string ProductId { get; set; }
    }
}
