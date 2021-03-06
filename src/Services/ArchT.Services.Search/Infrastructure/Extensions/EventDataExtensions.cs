﻿using ArchT.Services.Search.Contracts;
using ArchT.Services.Search.Infrastructure.EventHub;
using Microsoft.Azure.EventHubs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchT.Services.Search.Infrastructure.Extensions
{
    static class EventDataExtensions
    {
        public static T Map<T>(this EventData eventData)
        {
            var json = Encoding.UTF8.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);
            var data = JsonConvert.DeserializeObject<T>(json);
            return data;
        }
    }
}
