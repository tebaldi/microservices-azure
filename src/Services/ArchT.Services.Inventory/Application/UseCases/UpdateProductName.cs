using ArchT.Services.Inventory.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using ArchT.Services.Inventory.Contracts;
using System.Threading;
using ArchT.Services.Inventory.Application.Gateways;

namespace ArchT.Services.Inventory.Application.UseCases
{
    class UpdateProductName : IRequestHandler<UpdateProductNameRequest, CommandResponse>
    {
        private readonly IProductRepository repository;

        public UpdateProductName(IProductRepository repository)
        {
            this.repository = repository;
        }

        public async Task<CommandResponse> Handle(UpdateProductNameRequest request, CancellationToken cancellationToken)
        {
            var inventory = Product.Load(await repository.Load(request.ProductId));
            var result = inventory.UpdateName(Name.Create(request.Name));
            switch (result)
            {
                case CompletedOperation<ProductNameUpdated> co:
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
