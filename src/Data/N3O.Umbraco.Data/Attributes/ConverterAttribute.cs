using N3O.Umbraco.Data.Converters;
using N3O.Umbraco.Extensions;
using System;

namespace N3O.Umbraco.Data.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class ConverterAttribute : Attribute {
    public ConverterAttribute(Type converterType) {
        if (!converterType.ImplementsGenericInterface(typeof(ICellConverter<>))) {
            throw new Exception($"Type {converterType.FullName.Quote()} must implement {nameof(ICellConverter<object>)}");
        }

        CellConverterType = converterType;
    }

    public Type CellConverterType { get; }
}
