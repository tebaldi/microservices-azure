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
    class FindOrder : IRequestHandler<FindOrderRequest, OrderData>
    {
        private readonly OrdersDb db;
        public FindOrder(OrdersDb db) => this.db = db;

        public Task<OrderData> Handle(FindOrderRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new OrderData { Id = request.OrderId });
        }
    }
}
