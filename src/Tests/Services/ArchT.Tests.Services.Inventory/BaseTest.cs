using ArchT.Tests.Services.Inventory.Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArchT.Tests.Services.Inventory
{
    public abstract class BaseTest
    {
        private readonly ServiceProvider serviceProvider;

        public BaseTest()
        {
            var services = new ServiceCollection();
            services.AddInfrastructureTestServices();
            serviceProvider = services.BuildServiceProvider();
        }

        protected T GetService<T>() => serviceProvider.GetService<T>();
    }
}
