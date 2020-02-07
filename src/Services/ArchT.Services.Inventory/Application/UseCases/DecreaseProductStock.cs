using ArchT.Services.Inventory.Application.Gateways;
using ArchT.Services.Inventory.Application.Models;
using ArchT.Services.Inventory.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ArchT.Services.Inventory.Application.UseCases
{
    class DecreaseProductStock : IRequestHandler<DecreaseProductStockRequest, CommandResponse>
    {
        private readonly IProductRepository repository;

        public DecreaseProductStock(IProductRepository repository)
        {
            this.repository = repository;
        }

        public async Task<CommandResponse> Handle(DecreaseProductStockRequest request, CancellationToken cancellationToken)
        {
            var inventory = Product.Load(await repository.Load(request.ProductId));
            var result = inventory.DecreaseStock(Amount.Create(request.Amount));
            switch (result)
            {
                case CompletedOperation<ProductStockDecreased> co:
                    await repository.Store(request.ProductId, co.Result);
                    return new CommandResponse { Completed = true, Information = co.Result.GetType().Name, Data = co.Result };

                case CanceledOperation co:
                    return new CommandResponse { Completed = false, Information = co.Exception.Message, Data = co.Exception };

                default:
                    return default;
            }
        }
    }
}
