using ArchT.Services.OrderProcessor.Infrastructure.Database;
using ArchT.Services.OrderProcessor.Infrastructure.EventHub;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ArchT.Services.OrderProcessor.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        { 
            services.AddMediatR();
            services.AddLogging();
            services.AddTransient<OrderProcessorDb>();
            services.AddTransient<EventPublisher>();
            AssemblyScanner
                .FindValidatorsInAssembly(Assembly.GetExecutingAssembly())
                .ForEach(result => services.AddScoped(result.InterfaceType, result.ValidatorType));
        }
    }
}
