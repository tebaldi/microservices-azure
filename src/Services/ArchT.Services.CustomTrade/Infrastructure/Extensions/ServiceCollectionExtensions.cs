using ArchT.Services.CustomTrade.Infrastructure.Database;
using ArchT.Services.CustomTrade.Infrastructure.Pipelines;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ArchT.Services.CustomTrade.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {   services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestLoggingPipeline<,>));
            services.AddMediatR();
            services.AddLogging();
            services.AddTransient<OrdersDb>();
            AssemblyScanner
                .FindValidatorsInAssembly(Assembly.GetExecutingAssembly())
                .ForEach(result => services.AddScoped(result.InterfaceType, result.ValidatorType));
            Assembly
                .GetExecutingAssembly().GetTypes()
               .Where(t => t.Namespace.Contains("Infrastructure.DataAccess")).ToList()
               .ForEach(t =>
               {
                   foreach (var i in t.GetInterfaces())
                       services.AddScoped(i, t);
               });
        }
    }
}
