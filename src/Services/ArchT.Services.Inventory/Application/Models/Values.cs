using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ArchT.Services.Inventory.Application.Models
{
    public abstract class OperationResult { }

    public class CompletedOperation : OperationResult { }

    public class CompletedOperation<T> : OperationResult
    {
        public readonly T Result;
        private CompletedOperation(T result) => Result = result;
        public static CompletedOperation<T> Create(T result) => new CompletedOperation<T>(result);
    }

    public class CanceledOperation : OperationResult
    {
        public readonly Exception Exception;
        private CanceledOperation(Exception exception) => Exception = exception;
        public static CanceledOperation Create(Exception exception) => new CanceledOperation(exception);
    }

    public abstract class Value<T>
    {
        protected readonly T value;
        protected Value(T value) => this.value = value;
        public override string ToString() => value.ToString();
        public override bool Equals(object obj) => (obj as Value<T>).value.Equals(value);
        public override int GetHashCode() => value.GetHashCode();
    }

    public class ProductId : Value<string>
    {
        private ProductId(string value) : base(value) { }        
        public static ProductId Create(string id) => new ProductId(id);
    }

    public class Name : Value<string>
    {
        private Name(string value) : base(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value ?? string.Empty)) { }
        public static Name Create(string value) => new Name(value);
    }

    public class Amount : Value<int>
    {
        private Amount(int value) : base(value) { }
        public static Amount Create(int value) => new Amount(value);
        public Amount Increase(Amount amount) => new Amount(value + amount.value);
        public Amount Decrease(Amount amount) => new Amount(value - amount.value);
        public bool IsNegative => value < 0;
    }
}
