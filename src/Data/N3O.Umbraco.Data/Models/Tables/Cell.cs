using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Localization;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Data.Models;

public abstract class Cell : Value {
    protected Cell(DataType type,
                   object value,
                   Type targetType,
                   Dictionary<string, IEnumerable<object>> metadata) {
        Type = type;
        Value = value;
        TargetType = targetType;
        Metadata = metadata ?? new Dictionary<string, IEnumerable<object>>();
    }

    public DataType Type { get; }
    public object Value { get; }
    public Type TargetType { get; }
    public Dictionary<string, IEnumerable<object>> Metadata { get; }

    public string ToString(IFormatter formatter) {
        return Type.ConvertToText(formatter, Value);
    }
}

public class Cell<T> : Cell {
    protected internal Cell(DataType type,
                            T value,
                            Type targetType,
                            Dictionary<string, IEnumerable<object>> metadata)
        : base(type, value, targetType, metadata) {
        Value = value;
    }

    public new T Value { get; }
}
