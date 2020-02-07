using ArchT.Services.Inventory.Contracts;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ArchT.Services.Inventory.Application.Validators
{
    public class IncreaseProductStockValidator : AbstractValidator<IncreaseProductStockRequest>
    {
        public IncreaseProductStockValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.Amount).GreaterThan(0);
        }
    }
}
