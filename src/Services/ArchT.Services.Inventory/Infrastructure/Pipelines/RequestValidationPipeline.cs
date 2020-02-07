using ArchT.Services.Inventory.Contracts;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace ArchT.Services.Inventory.Infrastructure.Pipelines
{
    class RequestValidationPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TResponse: new()
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public RequestValidationPipeline(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        async Task<TResponse> IPipelineBehavior<TRequest, TResponse>.Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var failures = _validators
                .Select(v => v.Validate(request))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();

            switch(failures)
            {
                case var f when f.Any():
                    return await Task.FromResult(GenerateErrorResponse(failures));

                default:
                    return await next();
            }
        }

        TResponse GenerateErrorResponse(IEnumerable<ValidationFailure> failures)
        {
            var response = new TResponse();
            switch(response)
            {
                case CommandResponse r:
                    r.Completed = false;
                    foreach (var failure in failures)
                        r.Information += failure.ErrorMessage + "\n";
                    break;

                case QueryResponse r:
                    r.Completed = false;
                    foreach (var failure in failures)
                        r.Information += failure.ErrorMessage + "\n";
                    break;

            }
            return response;
        }
    }
}
