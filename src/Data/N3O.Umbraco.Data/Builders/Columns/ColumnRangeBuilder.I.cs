using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Data.Builders;

public interface IColumnRangeBuilder {
    IFluentColumnRangeBuilder<TValue> Bool<TValue>();
    IFluentColumnRangeBuilder<TValue> CalendarMonth<TValue>();
    IFluentColumnRangeBuilder<TValue> CalendarWeek<TValue>();
    IFluentColumnRangeBuilder<TValue> Date<TValue>();
    IFluentColumnRangeBuilder<TValue> DateTime<TValue>();
    IFluentColumnRangeBuilder<TValue> Decimal<TValue>();
    IFluentColumnRangeBuilder<TValue> Integer<TValue>();
    IFluentColumnRangeBuilder<TValue> Lookup<TValue>();
    IFluentColumnRangeBuilder<TValue> Reference<TValue>();
    IFluentColumnRangeBuilder<TValue> Money<TValue>();
    IFluentColumnRangeBuilder<TValue> String<TValue>();
    IFluentColumnRangeBuilder<TValue> Time<TValue>();

    IFluentColumnRangeBuilder<TValue> OfType<TValue>(DataType dataType);
    
    IColumnRangeBuilder UseFormatter(IFormatter formatter);
}
