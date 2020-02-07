using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArchT.Services.Inventory.Application.Models
{
    public abstract class Event
    {
        private DateTime? time;
        private long? sequence;
        private string name;
        public string EventName { get => string.IsNullOrEmpty(name) ? GetType().Name : name; set => name = value; }
        public DateTime EventTime { get => time ?? DateTime.UtcNow; set => time = value; }
        public long EventSequence { get => sequence ?? EventTime.Ticks; set => sequence = value; }
        protected Event() { }
    }

    public class ProductLoaded : Event
    {
        public readonly string ProductId;
        public readonly string Name;
        public readonly int Stock;
        private ProductLoaded(string productId, string name, int stock)
        {
            ProductId = productId; Name = name; Stock = stock;
        }
        public static ProductLoaded Create(string productId, string name, int stock) => new ProductLoaded(productId, name, stock);
    }

    public class ProductNameUpdated : Event
    {
        public readonly string ProductId;
        public readonly string OldValue;
        public readonly string NewValue;
        private ProductNameUpdated(string productId, string oldValue, string newValue)
        {
            ProductId = productId; OldValue = oldValue; NewValue = newValue;
        }
        public static ProductNameUpdated Create(string productId, string oldValue, string newValue) => new ProductNameUpdated(productId, oldValue, newValue);
    }

    public class ProductStockIncreased : Event
    {
        public readonly string ProductId;
        public readonly int OldValue;
        public readonly int NewValue;
        private ProductStockIncreased(string productId, int oldValue, int newValue)
        {
            ProductId = productId; OldValue = oldValue; NewValue = newValue;
        }
        public static ProductStockIncreased Create(string productId, int oldValue, int newValue) => new ProductStockIncreased(productId, oldValue, newValue);
    }

    public class ProductStockDecreased : Event
    {
        public readonly string ProductId;
        public readonly int OldValue;
        public readonly int NewValue;
        private ProductStockDecreased(string productId, int oldValue, int newValue)
        {
            ProductId = productId; OldValue = oldValue; NewValue = newValue;
        }
        public static ProductStockDecreased Create(string productId, int oldValue, int newValue) => new ProductStockDecreased(productId, oldValue, newValue);
    }
}
