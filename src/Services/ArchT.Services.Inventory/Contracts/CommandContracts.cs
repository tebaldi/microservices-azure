using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ArchT.Services.Inventory.Contracts
{
    public class CommandRequest : IRequest<CommandResponse> { }

    public class CommandResponse
    {
        public bool Completed { get; set; }
        public string Information { get; set; }
        public object Data { get; set; }
    }

    public class UpdateProductNameRequest : CommandRequest
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
    }

    public class IncreaseProductStockRequest : CommandRequest
    {
        public string ProductId { get; set; }
        public int Amount { get; set; }
    }

    public class DecreaseProductStockRequest : CommandRequest
    {
        public string ProductId { get; set; }
        public int Amount { get; set; }
    }
}
