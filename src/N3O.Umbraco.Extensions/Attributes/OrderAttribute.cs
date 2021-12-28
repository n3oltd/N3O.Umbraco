using System;

namespace N3O.Umbraco.Attributes {
    [AttributeUsage(AttributeTargets.All)]
    public class OrderAttribute : Attribute {
        public OrderAttribute(int order) {
            Order = order;
        }

        public int Order { get; }
    }
}
