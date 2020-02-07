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
    public class UpdateProductNameValidator : AbstractValidator<UpdateProductNameRequest>
    {
        public UpdateProductNameValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
