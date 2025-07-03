using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Data.Converters;
using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Security;
using System;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Data.Builders;

public class FluentColumnRangeBuilder<TValue> : IFluentColumnRangeBuilder<TValue> {
    private readonly IServiceProvider _serviceProvider;
    private readonly IFormatter _formatter;
    private readonly IStringLocalizer _stringLocalizer;
    private readonly ILocalClock _localClock;
    private readonly DataType _dataType;
    private readonly List<Attribute> _attributes = [];
    private readonly MultiDictionary<string, object> _columnMetadata = new();
    private ICellConverter<TValue> _cellConverter;
    private IColumnHeading _columnHeading;
    private RangeColumnSort _rangeColumnSort = RangeColumnSorts.Preserve;
    private CollectionLayout _collectionLayout = CollectionLayouts.ValuePerColumn;
    private AccessControlList _accessControlList = AccessControlList.AuthenticatedUsers();
    private bool _hidden;
    private Func<IFormatter, string> _getComment;
    private int _order;

    public FluentColumnRangeBuilder(IServiceProvider serviceProvider,
                                    IFormatter formatter,
                                    IStringLocalizer stringLocalizer,
                                    ILocalClock localClock,
                                    DataType dataType) {
        _serviceProvider = serviceProvider;
        _formatter = formatter;
        _stringLocalizer = stringLocalizer;
        _localClock = localClock;
        _dataType = dataType;
    }

    public IFluentColumnRangeBuilder<TValue> AddAttribute(Attribute attribute) {
        _attributes.AddIfNotExists(attribute);

        return this;
    }

    public IFluentColumnRangeBuilder<TValue> AddMetadata(string key, object value) {
        _columnMetadata.Add(key, value);

        return this;
    }

    public IFluentColumnRangeBuilder<TValue> AddMetadata(string key, IEnumerable<object> values) {
        foreach (var value in values) {
            AddMetadata(key, value);
        }

        return this;
    }

    public IFluentColumnRangeBuilder<TValue> Comment(string comment) {
        return Comment(_ => comment);
    }

    public IFluentColumnRangeBuilder<TValue> Comment(Func<IFormatter, string> getComment) {
        _getComment = getComment;

        return this;
    }

    public IFluentColumnRangeBuilder<TValue> Title<TStrings>(Func<TStrings, string> propertySelector)
        where TStrings : class, IStrings, new() {
        _columnHeading = new TextColumnHeading(f => f.Format(propertySelector));

        return this;
    }

    public IFluentColumnRangeBuilder<TValue> Title(string keyPrefix, string text) {
        _columnHeading = new TextColumnHeading(_ => _stringLocalizer.Get("Columns", $"{keyPrefix} - Title", text));

        return this;
    }

    public IFluentColumnRangeBuilder<TValue> Title(string text) {
        _columnHeading = new TextColumnHeading(_ => text);

        return this;
    }

    public IFluentColumnRangeBuilder<TValue> Title<T>() where T : IColumnHeading {
        return Title(typeof(T));
    }

    public IFluentColumnRangeBuilder<TValue> Title(Type columnHeadingType) {
        _columnHeading = (IColumnHeading) _serviceProvider.GetRequiredService(columnHeadingType);

        return this;
    }

    public IFluentColumnRangeBuilder<TValue> TitleFromMetadata() {
        _columnHeading = new TitleMetadataColumnHeading();

        return this;
    }
    
    public IFluentColumnRangeBuilder<TValue> TitleFromValue() {
        _columnHeading = new CellValueColumnHeading();

        return this;
    }

    public IFluentColumnRangeBuilder<TValue> Converter<TCellConverter>()
        where TCellConverter : ICellConverter<TValue> {
        _cellConverter = _serviceProvider.GetRequiredService<TCellConverter>();

        return this;
    }

    public IFluentColumnRangeBuilder<TValue> Converter(Type cellConverterType) {
        if (!cellConverterType.ImplementsInterface<ICellConverter<TValue>>()) {
            throw Exception($"The specified cell converter type {cellConverterType.FullName.Quote()} does not implement {nameof(ICellConverter<TValue>)} for type {typeof(TValue).FullName}");
        }

        _cellConverter = (ICellConverter<TValue>) _serviceProvider.GetRequiredService(cellConverterType);
        if (_cellConverter == null) {
            Exception($"The cell converter {cellConverterType.FullName} does not implement {nameof(ICellConverter<TValue>)} for {nameof(TValue)}. Did you forget to specify a custom cell converter?");
        }

        return this;
    }

    public IFluentColumnRangeBuilder<TValue> CommaSeparateValues() {
        return CollectionLayout(CollectionLayouts.CommaSeparated);
    }

    public IFluentColumnRangeBuilder<TValue> ValuePerColumn() {
        return CollectionLayout(CollectionLayouts.ValuePerColumn);
    }

    public IFluentColumnRangeBuilder<TValue> CollectionLayout(CollectionLayout collectionLayout) {
        _collectionLayout = collectionLayout;

        return this;
    }

    public IFluentColumnRangeBuilder<TValue> PreserveColumnOrder() {
        return RangeColumnSort(RangeColumnSorts.Preserve);
    }

    public IFluentColumnRangeBuilder<TValue> SortColumns() {
        return RangeColumnSort(RangeColumnSorts.Alphabetical);
    }

    public IFluentColumnRangeBuilder<TValue> RangeColumnSort(RangeColumnSort rangeColumnSort) {
        _rangeColumnSort = rangeColumnSort;

        return this;
    }

    public IFluentColumnRangeBuilder<TValue> Hidden() {
        _hidden = true;

        return this;
    }

    public IFluentColumnRangeBuilder<TValue> VisibleTo(AccessControlList accessControlList) {
        _accessControlList = accessControlList;

        return this;
    }

    public IFluentColumnRangeBuilder<TValue> SetOrder(int order) {
        _order = order;

        return this;
    }

    public ColumnRange<TValue> Build() {
        if (_cellConverter == null) {
            Converter(_dataType.GetDefaultCellConverterType());
        }

        Validate();

        var columnRange = new ColumnRange<TValue>(_formatter,
                                                  _localClock,
                                                  _cellConverter,
                                                  _columnHeading,
                                                  _getComment,
                                                  _rangeColumnSort,
                                                  _collectionLayout,
                                                  _dataType,
                                                  _columnMetadata.ToDictionary(x => x.Key,
                                                                               x => x.Value as IEnumerable<object>),
                                                  _hidden,
                                                  _accessControlList,
                                                  _attributes,
                                                  _order);

        return columnRange;
    }

    private void Validate() {
        if (_columnHeading == null) {
            throw Exception("No column heading has been specified");
        }
    }

    private Exception Exception(string message) {
        return new Exception($"Error building column range for {typeof(TValue).FullName}: {message}");
    }

    public static implicit operator ColumnRange<TValue>(FluentColumnRangeBuilder<TValue> builder) {
        return builder.Build();
    }
}
