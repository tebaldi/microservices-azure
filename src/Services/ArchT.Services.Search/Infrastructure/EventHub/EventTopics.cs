using ArchT.Services.Search.Infrastructure.Configuration;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArchT.Services.Search.Infrastructure.EventHub
{
    enum EventTopics
    {
        Products, Orders
    }
}
