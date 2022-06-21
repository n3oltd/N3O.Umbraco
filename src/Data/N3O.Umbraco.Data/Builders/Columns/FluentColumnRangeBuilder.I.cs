using N3O.Umbraco.Data.Converters;
using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Security;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Data.Builders;

public interface IFluentColumnRangeBuilder<TValue> {
    IFluentColumnRangeBuilder<TValue> AddAttribute(Attribute attribute);

    IFluentColumnRangeBuilder<TValue> AddMetadata(string key, object value);
    IFluentColumnRangeBuilder<TValue> AddMetadata(string key, IEnumerable<object> values);

    IFluentColumnRangeBuilder<TValue> Comment(string comment);
    IFluentColumnRangeBuilder<TValue> Comment(Func<IFormatter, string> getComment);
    
    IFluentColumnRangeBuilder<TValue> Title<TStrings>(Func<TStrings, string> propertySelector)
        where TStrings : class, IStrings, new();
    IFluentColumnRangeBuilder<TValue> Title(string text);
    IFluentColumnRangeBuilder<TValue> Title(string keyPrefix, string text);
    IFluentColumnRangeBuilder<TValue> Title<T>() where T : IColumnHeading;
    IFluentColumnRangeBuilder<TValue> Title(Type columnHeadingType);
    IFluentColumnRangeBuilder<TValue> TitleFromMetadata();
    IFluentColumnRangeBuilder<TValue> TitleFromValue();

    IFluentColumnRangeBuilder<TValue> Converter<TCellConverter>() where TCellConverter : ICellConverter<TValue>;
    IFluentColumnRangeBuilder<TValue> Converter(Type cellConverterType);

    IFluentColumnRangeBuilder<TValue> CommaSeparateValues();
    IFluentColumnRangeBuilder<TValue> ValuePerColumn();
    IFluentColumnRangeBuilder<TValue> CollectionLayout(CollectionLayout collectionLayout);

    IFluentColumnRangeBuilder<TValue> PreserveColumnOrder();
    IFluentColumnRangeBuilder<TValue> SortColumns();
    IFluentColumnRangeBuilder<TValue> RangeColumnSort(RangeColumnSort rangeColumnSort);

    IFluentColumnRangeBuilder<TValue> Hidden();
    IFluentColumnRangeBuilder<TValue> VisibleTo(AccessControlList accessControlList);

    IFluentColumnRangeBuilder<TValue> SetOrder(int order);
    
    ColumnRange<TValue> Build();
}
