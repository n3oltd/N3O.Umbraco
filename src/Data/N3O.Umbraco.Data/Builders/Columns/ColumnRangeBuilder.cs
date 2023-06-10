using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Localization;
using System;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;

namespace N3O.Umbraco.Data.Builders;

public class ColumnRangeBuilder : IColumnRangeBuilder {
    private readonly IServiceProvider _serviceProvider;
    private readonly IStringLocalizer _stringLocalizer;
    private readonly ILocalClock _localClock;
    private IFormatter _formatter;

    public ColumnRangeBuilder(IServiceProvider serviceProvider,
                              IStringLocalizer stringLocalizer,
                              ILocalClock localClock,
                              IFormatter formatter) {
        _serviceProvider = serviceProvider;
        _stringLocalizer = stringLocalizer;
        _localClock = localClock;
        _formatter = formatter;
    }
    
    public IFluentColumnRangeBuilder<TValue> Bool<TValue>() {
        return OfType<TValue>(OurDataTypes.Bool);
    }

    public IFluentColumnRangeBuilder<TValue> Date<TValue>() {
        return OfType<TValue>(OurDataTypes.Date);
    }

    public IFluentColumnRangeBuilder<TValue> DateTime<TValue>() {
        return OfType<TValue>(OurDataTypes.DateTime);
    }

    public IFluentColumnRangeBuilder<TValue> Decimal<TValue>() {
        return OfType<TValue>(OurDataTypes.Decimal);
    }

    public IFluentColumnRangeBuilder<TValue> Integer<TValue>() {
        return OfType<TValue>(OurDataTypes.Integer);
    }

    public IFluentColumnRangeBuilder<TValue> Lookup<TValue>() {
        return OfType<TValue>(OurDataTypes.Lookup);
    }

    public IFluentColumnRangeBuilder<TValue> Reference<TValue>() {
        return OfType<TValue>(OurDataTypes.Reference);
    }

    public IFluentColumnRangeBuilder<TValue> Money<TValue>() {
        return OfType<TValue>(OurDataTypes.Money);
    }

    public IFluentColumnRangeBuilder<TValue> String<TValue>() {
        return OfType<TValue>(OurDataTypes.String);
    }

    public IFluentColumnRangeBuilder<TValue> Time<TValue>() {
        return OfType<TValue>(OurDataTypes.Time);
    }

    public IFluentColumnRangeBuilder<TValue> OfType<TValue>(DataType dataType) {
        var builder = new FluentColumnRangeBuilder<TValue>(_serviceProvider,
                                                           _formatter,
                                                           _stringLocalizer,
                                                           _localClock,
                                                           dataType);

        return builder;
    }

    public IColumnRangeBuilder UseFormatter(IFormatter formatter) {
        _formatter = formatter;

        return this;
    }
}
