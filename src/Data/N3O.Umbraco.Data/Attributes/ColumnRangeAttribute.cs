using N3O.Umbraco.Data.Lookups;
using System;

namespace N3O.Umbraco.Data.Attributes;

public abstract class ColumnRangeAttribute : Attribute {
    protected ColumnRangeAttribute(DataType dataType, int order) {
        DataType = dataType;
        Order = order;
    }

    public DataType DataType { get; }
    public int Order { get; }
}
