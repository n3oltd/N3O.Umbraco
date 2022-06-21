using N3O.Umbraco.Data.Lookups;
using System;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;

namespace N3O.Umbraco.Data.Attributes;

public abstract class ValueAttribute : ColumnRangeAttribute {
    protected ValueAttribute(int order, DataType dataType) : base(dataType, order) { }
}

[AttributeUsage(AttributeTargets.Property)]
public class BoolAttribute : ValueAttribute {
    public BoolAttribute(int order) : base(order, OurDataTypes.Bool) { }
}

[AttributeUsage(AttributeTargets.Property)]
public class ContentAttribute : ValueAttribute {
    public ContentAttribute(int order) : base(order, OurDataTypes.Content) { }
}

[AttributeUsage(AttributeTargets.Property)]
public class DateAttribute : ValueAttribute {
    public DateAttribute(int order) : base(order, OurDataTypes.Date) { }
}

[AttributeUsage(AttributeTargets.Property)]
public class DateTimeAttribute : ValueAttribute {
    public DateTimeAttribute(int order) : base(order, OurDataTypes.DateTime) { }
}

[AttributeUsage(AttributeTargets.Property)]
public class DecimalAttribute : ValueAttribute {
    public DecimalAttribute(int order) : base(order, OurDataTypes.Decimal) { }
}

[AttributeUsage(AttributeTargets.Property)]
public class GuidAttribute : ValueAttribute {
    public GuidAttribute(int order) : base(order, OurDataTypes.Guid) { }
}

[AttributeUsage(AttributeTargets.Property)]
public class IntegerAttribute : ValueAttribute {
    public IntegerAttribute(int order) : base(order, OurDataTypes.Integer) { }
}

[AttributeUsage(AttributeTargets.Property)]
public class LookupAttribute : ValueAttribute {
    public LookupAttribute(int order) : base(order, OurDataTypes.Lookup) { }
}

[AttributeUsage(AttributeTargets.Property)]
public class MoneyAttribute : ValueAttribute {
    public MoneyAttribute(int order) : base(order, OurDataTypes.Money) { }
}

[AttributeUsage(AttributeTargets.Property)]
public class PublishedContentAttribute : ValueAttribute {
    public PublishedContentAttribute(int order) : base(order, OurDataTypes.PublishedContent) { }
}

[AttributeUsage(AttributeTargets.Property)]
public class ReferenceAttribute : ValueAttribute {
    public ReferenceAttribute(int order) : base(order, OurDataTypes.Reference) { }
}

[AttributeUsage(AttributeTargets.Property)]
public class StringAttribute : ValueAttribute {
    public StringAttribute(int order) : base(order, OurDataTypes.String) { }
}

[AttributeUsage(AttributeTargets.Property)]
public class TimeAttribute : ValueAttribute {
    public TimeAttribute(int order) : base(order, OurDataTypes.Time) { }
}

[AttributeUsage(AttributeTargets.Property)]
public class YearMonthAttribute : ValueAttribute {
    public YearMonthAttribute(int order) : base(order, OurDataTypes.YearMonth) { }
}
