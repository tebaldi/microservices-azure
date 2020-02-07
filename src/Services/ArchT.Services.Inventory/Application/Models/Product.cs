using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArchT.Services.Inventory.Application.Models
{
    public class Product
    {
        private readonly ProductId productId;
        private Name productName;
        private Amount stock;

        private Product(ProductId productId, Name productName, Amount stock)
        {
            this.productId = productId; this.productName = productName; this.stock = stock;
        }

        public static Product Load(ProductLoaded snapshot) => new Product(
                ProductId.Create(snapshot.ProductId), Name.Create(snapshot.Name), Amount.Create(snapshot.Stock));

        public OperationResult UpdateName(Name name)
        {
            var oldValue = productName;
            var newValue = name;
            switch (newValue)
            {
                default:
                    productName = newValue;
                    return CompletedOperation<ProductNameUpdated>.Create(ProductNameUpdated.Create(
                        productId.ToString(), oldValue.ToString(), newValue.ToString()));
            }
        }

        public OperationResult IncreaseStock(Amount amount)
        {
            var oldValue = stock;
            var newValue = stock.Increase(amount);
            switch (newValue)
            {
                default:
                    stock = newValue;
                    return CompletedOperation<ProductStockIncreased>.Create(ProductStockIncreased.Create(
                        productId.ToString(), int.Parse(oldValue.ToString()), int.Parse(newValue.ToString())));
            }
        }

        public OperationResult DecreaseStock(Amount amount)
        {
            var oldValue = stock;
            var newValue = stock.Decrease(amount);
            switch (newValue)
            {
                case var a when a.IsNegative:
                    return CanceledOperation.Create(new StockNotAvailableException(
                        int.Parse(amount.ToString()), int.Parse(stock.ToString())));

                default:
                    stock = newValue;
                    return CompletedOperation<ProductStockDecreased>.Create(ProductStockDecreased.Create(
                        productId.ToString(), int.Parse(oldValue.ToString()), int.Parse(newValue.ToString())));
            }
        }
    }
}
