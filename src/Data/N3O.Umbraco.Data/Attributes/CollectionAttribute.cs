using N3O.Umbraco.Data.Lookups;
using System;

namespace N3O.Umbraco.Data.Attributes {
    public abstract class CollectionAttribute : ColumnRangeAttribute {
        protected CollectionAttribute(int order,
                                      DataType dataType,
                                      string layoutId,
                                      int maxValues = -1,
                                      string sortId = null)
            : base(dataType, order) {
            Layout = CollectionLayouts.GetById(layoutId);
            MaxValues = maxValues;
            Sort = RangeColumnSorts.GetById(sortId) ?? RangeColumnSorts.Preserve;
        }

        public CollectionLayout Layout { get; }
        public int MaxValues { get; }
        public RangeColumnSort Sort { get; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class BoolCollectionAttribute : CollectionAttribute {
        public BoolCollectionAttribute(int order, string layoutId, int maxValues = -1, string sort = null)
            : base(order, DataTypes.Bool, layoutId, maxValues, sort) { }
    }
    
    [AttributeUsage(AttributeTargets.Property)]
    public class ContentCollectionAttribute : CollectionAttribute {
        public ContentCollectionAttribute(int order, string layoutId, int maxValues = -1, string sort = null)
            : base(order, DataTypes.Content, layoutId, maxValues, sort) { }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class DateCollectionAttribute : CollectionAttribute {
        public DateCollectionAttribute(int order, string layoutId, int maxValues = -1, string sort = null)
            : base(order, DataTypes.Date, layoutId, maxValues, sort) { }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class DateTimeCollectionAttribute : CollectionAttribute {
        public DateTimeCollectionAttribute(int order, string layoutId, int maxValues = -1, string sort = null)
            : base(order, DataTypes.DateTime, layoutId, maxValues, sort) { }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class DecimalCollectionAttribute : CollectionAttribute {
        public DecimalCollectionAttribute(int order, string layoutId, int maxValues = -1, string sort = null)
            : base(order, DataTypes.Decimal, layoutId, maxValues, sort) { }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class GuidCollectionAttribute : CollectionAttribute {
        public GuidCollectionAttribute(int order, string layoutId, int maxValues = -1, string sort = null)
            : base(order, DataTypes.Guid, layoutId, maxValues, sort) { }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class IntegerCollectionAttribute : CollectionAttribute {
        public IntegerCollectionAttribute(int order, string layoutId, int maxValues = -1, string sort = null)
            : base(order, DataTypes.Integer, layoutId, maxValues, sort) { }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class LookupCollectionAttribute : CollectionAttribute {
        public LookupCollectionAttribute(int order, string layoutId, int maxValues = -1, string sort = null)
            : base(order, DataTypes.Lookup, layoutId, maxValues, sort) { }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class MoneyCollectionAttribute : CollectionAttribute {
        public MoneyCollectionAttribute(int order, string layoutId, int maxValues = -1, string sort = null)
            : base(order, DataTypes.Money, layoutId, maxValues, sort) { }
    }
    
    [AttributeUsage(AttributeTargets.Property)]
    public class PublishedContentCollectionAttribute : CollectionAttribute {
        public PublishedContentCollectionAttribute(int order, string layoutId, int maxValues = -1, string sort = null)
            : base(order, DataTypes.PublishedContent, layoutId, maxValues, sort) { }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ReferenceCollectionAttribute : CollectionAttribute {
        public ReferenceCollectionAttribute(int order, string layoutId, int maxValues = -1, string sort = null)
            : base(order, DataTypes.Reference, layoutId, maxValues, sort) { }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class StringCollectionAttribute : CollectionAttribute {
        public StringCollectionAttribute(int order, string layoutId, int maxValues = -1, string sort = null)
            : base(order, DataTypes.String, layoutId, maxValues, sort) { }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class TimeCollectionAttribute : CollectionAttribute {
        public TimeCollectionAttribute(int order, string layoutId, int maxValues = -1, string sort = null)
            : base(order, DataTypes.Time, layoutId, maxValues, sort) { }
    }
    
    [AttributeUsage(AttributeTargets.Property)]
    public class YearMonthCollectionAttribute : CollectionAttribute {
        public YearMonthCollectionAttribute(int order, string layoutId, int maxValues = -1, string sort = null)
            : base(order, DataTypes.YearMonth, layoutId, maxValues, sort) { }
    }
}