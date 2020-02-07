using ArchT.Services.CustomTrade.Infrastructure.Database;
using ArchT.Services.CustomTrade.Infrastructure.Extensions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static DataContracts;
using static QueryContracts;

namespace ArchT.Services.CustomTrade.Infrastructure.DataAccess
{
    class GetOrders : IRequestHandler<GetOdersRequest, IEnumerable<OrderData>>
    {
        private readonly OrdersDb db;
        public GetOrders(OrdersDb db) => this.db = db;

        public Task<IEnumerable<OrderData>> Handle(GetOdersRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(Enumerable.Empty<OrderData>());
        }
    }
}
