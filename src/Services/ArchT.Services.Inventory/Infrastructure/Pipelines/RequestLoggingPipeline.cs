using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ArchT.Services.Inventory.Infrastructure.Pipelines
{
    class RequestLoggingPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger logger;

        public RequestLoggingPipeline(ILoggerFactory loggerFactory)
        {
            this.logger = loggerFactory.CreateLogger<RequestLoggingPipeline<TRequest, TResponse>>();
        }

        async Task<TResponse> IPipelineBehavior<TRequest, TResponse>.Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var stringfiedRequest = JsonConvert.SerializeObject(request, Formatting.Indented);
            logger.LogInformation(stringfiedRequest);

            try
            {
                var response = await next();
                var stringfiedResponse = JsonConvert.SerializeObject(response, Formatting.Indented);
                logger.LogDebug(stringfiedResponse);
                return response;
            }
            catch(Exception ex)
            {
                logger.LogError(ex.ToString());
                throw ex;
            }
        }
    }
}
