using N3O.Umbraco.Data.Lookups;
using System;

namespace N3O.Umbraco.Data.Attributes {
    public abstract class FooterAttribute : Attribute {
        protected FooterAttribute(AggregationFunction footerFunction) {
            FooterFunction = footerFunction;
        }

        public AggregationFunction FooterFunction { get; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class AverageFooterAttribute : FooterAttribute {
        public AverageFooterAttribute() : base(AggregationFunctions.Average) { }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class CountFooterAttribute : FooterAttribute {
        public CountFooterAttribute() : base(AggregationFunctions.Count) { }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class MinFooterAttribute : FooterAttribute {
        public MinFooterAttribute() : base(AggregationFunctions.Min) { }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class MaxFooterAttribute : FooterAttribute {
        public MaxFooterAttribute() : base(AggregationFunctions.Max) { }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class SumFooterAttribute : FooterAttribute {
        public SumFooterAttribute() : base(AggregationFunctions.Sum) { }
    }
}