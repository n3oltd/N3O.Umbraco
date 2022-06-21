using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Localization;
using System;

namespace N3O.Umbraco.Data.Converters;

public interface ICellConverter<in TValue> {
    Cell Convert(IFormatter formatter, ILocalClock clock, TValue value, Type targetType);
}

public interface INullableCellConverter<TValue> : ICellConverter<TValue>, ICellConverter<TValue?>
    where TValue : struct { }
