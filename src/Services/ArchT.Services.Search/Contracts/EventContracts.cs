﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArchT.Services.Search.Contracts
{
    public abstract class EventContract
    {
        private DateTime? time;
        private long? sequence;
        private string name;
        public string EventName { get => string.IsNullOrEmpty(name) ? GetType().Name : name; set => name = value; }
        public DateTime EventTime { get => time ?? DateTime.UtcNow; set => time = value; }
        public long EventSequence { get => sequence ?? EventTime.Ticks; set => sequence = value; }
    }

    public class ProductUpdated : EventContract
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
    }
}
