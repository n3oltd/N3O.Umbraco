using N3O.Umbraco.Data.Attributes;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace N3O.Umbraco.Data.Builders;

public class RowProperty<TRow, TProperty, TValue> : IRowProperty<TRow> {
    private readonly PropertyInfo _property;
    private readonly IColumnRangeBuilder _columnRangeBuilder;
    private readonly IUntypedTableBuilder _tableBuilder;
    private ColumnRange<TValue> _columnRange;
    private bool _isCollection;

    public RowProperty(PropertyInfo property,
                       IColumnRangeBuilder columnRangeBuilder,
                       IUntypedTableBuilder tableBuilder) {
        _property = property;
        _columnRangeBuilder = columnRangeBuilder;
        _tableBuilder = tableBuilder;
    }

    public void AddValues(TRow row) {
        var propertyValue = _property.GetValue(row);

        if (_isCollection) {
            var values = (IEnumerable<TValue>)propertyValue;

            _tableBuilder.AddValues(_columnRange, values);
        } else {
            _tableBuilder.AddValue(_columnRange, (TValue) propertyValue);
        }
    }

    public void CreateColumnRange() {
        var builder = GetColumnRangeBuilder();

        SetLayout(builder);
        SetConverter(builder);
        SetTitle(builder);
        SetAttributesAndMetadata(builder);
        SetHidden(builder);
        SetVisibility(builder);

        _columnRange = builder.Build();
    }

    private IFluentColumnRangeBuilder<TValue> GetColumnRangeBuilder() {
        var dataType = _property.GetCustomAttribute<ColumnRangeAttribute>().DataType;

        return _columnRangeBuilder.OfType<TValue>(dataType);
    }

    private void SetLayout(IFluentColumnRangeBuilder<TValue> builder) {
        var collectionAttribute = _property.GetCustomAttribute<CollectionAttribute>();
        var valueAttribute = _property.GetCustomAttribute<ValueAttribute>();

        if (collectionAttribute != null && valueAttribute != null) {
            throw Exception("Each property must either be marked as a collection or value and cannot be marked as both");
        }

        if (valueAttribute != null) {
            if (typeof(TProperty) != typeof(TValue)) {
                Exception("Property is marked as a value but is a collection");
            }
        }

        if (collectionAttribute != null) {
            if (!typeof(TProperty).ImplementsInterface<IEnumerable<TValue>>()) {
                Exception("Property is marked as a collection but is a value");
            }

            builder.CollectionLayout(collectionAttribute.Layout);
            builder.RangeColumnSort(collectionAttribute.Sort);

            _isCollection = true;
        }
    }

    private void SetConverter(IFluentColumnRangeBuilder<TValue> builder) {
        var attribute = _property.GetCustomAttribute<ConverterAttribute>();

        if (attribute != null) {
            builder.Converter(attribute.CellConverterType);
        }
    }

    private void SetTitle(IFluentColumnRangeBuilder<TValue> builder) {
        var isSet = SetStaticTitle(builder);

        if (!isSet) {
            throw Exception("A title attribute must be specified");
        }
    }

    private bool SetStaticTitle(IFluentColumnRangeBuilder<TValue> builder) {
        var attribute = _property.GetCustomAttribute<TitleAttribute>();

        if (attribute == null) {
            return false;
        }

        if (attribute.DoNotLocalize) {
            builder.Title(attribute.Text);
        } else {
            var keyPrefix = $"{typeof(TRow).FullName}_{_property.Name}";

            builder.Title(keyPrefix, attribute.Text);
        }

        return true;
    }

    private bool SetTitleFromMetadata(IFluentColumnRangeBuilder<TValue> builder) {
        var attribute = _property.GetCustomAttribute<TitleFromMetadataAttribute>();

        if (attribute == null) {
            return false;
        }

        builder.TitleFromMetadata();

        return true;
    }
    
    private bool SetTitleFromValue(IFluentColumnRangeBuilder<TValue> builder) {
        var attribute = _property.GetCustomAttribute<TitleFromValueAttribute>();

        if (attribute == null) {
            return false;
        }

        builder.TitleFromValue();

        return true;
    }

    private bool SetCustomTitle(IFluentColumnRangeBuilder<TValue> builder) {
        var attribute = _property.GetCustomAttribute<CustomTitleAttribute>();

        if (attribute == null) {
            return false;
        }

        builder.Title(attribute.Type);

        return true;
    }

    private void SetAttributesAndMetadata(IFluentColumnRangeBuilder<TValue> builder) {
        var attributes = _property.GetCustomAttributes();

        foreach (var attribute in attributes) {
            builder.AddAttribute(attribute);

            if (attribute is ColumnMetadataAttribute columnMetadataAttribute) {
                var metadata = columnMetadataAttribute.GetMetadata();

                foreach (var (key, value) in metadata) {
                    builder.AddMetadata(key, value);
                }
            }
        }
    }
    
    private void SetHidden(IFluentColumnRangeBuilder<TValue> builder) {
        var attribute = _property.GetCustomAttribute<HiddenAttribute>();

        if (attribute != null) {
            builder.Hidden();
        }
    }

    private void SetVisibility(IFluentColumnRangeBuilder<TValue> builder) {
        var attribute = _property.GetCustomAttribute<VisibleToAttribute>();

        if (attribute != null) {
            builder.VisibleTo(attribute.AccessControlList);
        }
    }

    private Exception Exception(string message) {
        return new Exception($"Error processing property {_property.Name} of type {typeof(TRow).FullName}: {message}");
    }
}
