using ArchT.Services.Inventory.Infrastructure.Pipelines;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ArchT.Tests.Services.Inventory.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructureTestServices(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestLoggingPipeline<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationPipeline<,>));
            services.AddMediatR();
            services.AddLogging();
            AssemblyScanner
                .FindValidatorsInAssembly(Assembly.Load("ArchT.Services.Inventory"))
                .ForEach(result => services.AddScoped(result.InterfaceType, result.ValidatorType));
            Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.Namespace != null && t.Namespace.Contains("Infrastructure.DataAccess")).ToList()
                .ForEach(t =>
                {
                    foreach (var i in t.GetInterfaces())
                        services.AddScoped(i, t);
                });
        }
    }
}
