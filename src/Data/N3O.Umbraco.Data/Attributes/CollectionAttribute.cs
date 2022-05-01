using N3O.Umbraco.Data.Lookups;
using System;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;

namespace N3O.Umbraco.Data.Attributes {
    public abstract class CollectionAttribute : ColumnRangeAttribute {
        protected CollectionAttribute(int order, DataType dataType, string layoutId, string sortId = null)
            : base(dataType, order) {
            Layout = CollectionLayouts.GetById(layoutId);
            Sort = RangeColumnSorts.GetById(sortId) ?? RangeColumnSorts.Preserve;
        }

        public CollectionLayout Layout { get; }
        public RangeColumnSort Sort { get; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class BoolCollectionAttribute : CollectionAttribute {
        public BoolCollectionAttribute(int order, string layoutId, string sort = null)
            : base(order, OurDataTypes.Bool, layoutId, sort) { }
    }
    
    [AttributeUsage(AttributeTargets.Property)]
    public class ContentCollectionAttribute : CollectionAttribute {
        public ContentCollectionAttribute(int order, string layoutId, string sort = null)
            : base(order, OurDataTypes.Content, layoutId, sort) { }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class DateCollectionAttribute : CollectionAttribute {
        public DateCollectionAttribute(int order, string layoutId, string sort = null)
            : base(order, OurDataTypes.Date, layoutId, sort) { }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class DateTimeCollectionAttribute : CollectionAttribute {
        public DateTimeCollectionAttribute(int order, string layoutId, string sort = null)
            : base(order, OurDataTypes.DateTime, layoutId, sort) { }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class DecimalCollectionAttribute : CollectionAttribute {
        public DecimalCollectionAttribute(int order, string layoutId, string sort = null)
            : base(order, OurDataTypes.Decimal, layoutId, sort) { }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class GuidCollectionAttribute : CollectionAttribute {
        public GuidCollectionAttribute(int order, string layoutId, string sort = null)
            : base(order, OurDataTypes.Guid, layoutId, sort) { }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class IntegerCollectionAttribute : CollectionAttribute {
        public IntegerCollectionAttribute(int order, string layoutId, string sort = null)
            : base(order, OurDataTypes.Integer, layoutId, sort) { }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class LookupCollectionAttribute : CollectionAttribute {
        public LookupCollectionAttribute(int order, string layoutId, string sort = null)
            : base(order, OurDataTypes.Lookup, layoutId, sort) { }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class MoneyCollectionAttribute : CollectionAttribute {
        public MoneyCollectionAttribute(int order, string layoutId, string sort = null)
            : base(order, OurDataTypes.Money, layoutId, sort) { }
    }
    
    [AttributeUsage(AttributeTargets.Property)]
    public class PublishedContentCollectionAttribute : CollectionAttribute {
        public PublishedContentCollectionAttribute(int order, string layoutId, string sort = null)
            : base(order, OurDataTypes.PublishedContent, layoutId, sort) { }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ReferenceCollectionAttribute : CollectionAttribute {
        public ReferenceCollectionAttribute(int order, string layoutId, string sort = null)
            : base(order, OurDataTypes.Reference, layoutId, sort) { }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class StringCollectionAttribute : CollectionAttribute {
        public StringCollectionAttribute(int order, string layoutId, string sort = null)
            : base(order, OurDataTypes.String, layoutId, sort) { }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class TimeCollectionAttribute : CollectionAttribute {
        public TimeCollectionAttribute(int order, string layoutId, string sort = null)
            : base(order, OurDataTypes.Time, layoutId, sort) { }
    }
    
    [AttributeUsage(AttributeTargets.Property)]
    public class YearMonthCollectionAttribute : CollectionAttribute {
        public YearMonthCollectionAttribute(int order, string layoutId, string sort = null)
            : base(order, OurDataTypes.YearMonth, layoutId, sort) { }
    }
}