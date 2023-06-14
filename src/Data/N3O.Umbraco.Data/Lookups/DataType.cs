using N3O.Umbraco.Data.Converters;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.References;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Data.Lookups;

public abstract class DataType : NamedLookup {
    private readonly Type _clrType;
    private readonly object _defaultValue;

    protected DataType(string id, string name, Type clrType, object defaultValue, bool requiresTargetType)
        : base(id, name) {
        _defaultValue = defaultValue;
        _clrType = clrType;

        RequiresTargetType = requiresTargetType;
    }

    public bool RequiresTargetType { get; }

    public Type GetClrType() => _clrType;
    public object GetDefaultValue() => _defaultValue;

    public abstract Cell Cell(object value, Type targetType);
    public abstract Type GetDefaultCellConverterType();
    public abstract string ToInvariantText(object value);
    public abstract string ToText(IFormatter formatter, object value);
    public abstract bool ValuesAreEqual(object value1, object value2);

    public virtual object GetRequest(object value, Type targetType) => value;
    public virtual object GetResponse(object value, Type targetType) => value;
}

public class DataType<T, TDefaultCellConverter, TTextConverter> : DataType
    where TDefaultCellConverter : ICellConverter<T>
    where TTextConverter : ITextConverter<T>, new() {
    private readonly Func<T, T, bool> _valuesAreEqual;
    private readonly ITextConverter<T> _textConverter = new TTextConverter();

    public DataType(string id, string name, bool requiresTargetType, Func<T, T, bool> valuesAreEqual)
        : base(id, name, typeof(T), default(T), requiresTargetType) {
        _valuesAreEqual = valuesAreEqual;
    }

    public override Type GetDefaultCellConverterType() => typeof(TDefaultCellConverter);

    public Cell<T> Cell(T value,
                        Type targetType,
                        Dictionary<string, IEnumerable<object>> metadata = null) {
        return new Cell<T>(this, value, targetType, metadata);
    }

    public Cell<T> Cell(T value, Dictionary<string, IEnumerable<object>> metadata = null) {
        if (RequiresTargetType) {
            throw new Exception($"DataType {Id.Quote()} requires a target type");
        }

        return new Cell<T>(this, value, null, metadata);
    }

    public override Cell Cell(object value, Type targetType) {
        return Cell((T) value, targetType);
    }

    public string ToInvariantText(T value) {
        return _textConverter.ToInvariantText(value);
    }

    public override string ToInvariantText(object value) {
        return ToInvariantText((T) value);
    }
    
    public string ToText(IFormatter formatter, T value) {
        return _textConverter.ToText(formatter, value);
    }

    public override string ToText(IFormatter formatter, object value) {
        return ToText(formatter, (T) value);
    }

    public override bool ValuesAreEqual(object obj1, object obj2) {
        if (obj1 == null && obj2 == null) {
            return true;
        }

        if (obj1 == null || obj2 == null) {
            return false;
        }

        if (obj1 is T value1 && obj2 is T value2) {
            return _valuesAreEqual(value1, value2);
        }

        return false;
    }
}

public class DataTypes : StaticLookupsCollection<DataType> {
    public static readonly DataType<Blob, BlobCellConverter, BlobTextConverter> Blob =
        new("blob", "Blob", false, (a, b) => a == b);
    
    public static readonly DataType<bool?, BoolCellConverter, BoolTextConverter> Bool
        = new("bool", "Boolean", false, (a, b) => a == b);

    public static readonly DataType<IContent, ContentCellConverter, ContentTextConverter> Content
        = new("content", "Content", false, (a, b) => a?.Id == b?.Id);
    
    public static readonly DataType<LocalDate?, DateCellConverter, DateTextConverter> Date
        = new("date", "Date", false, (a, b) => a == b);

    public static readonly DataType<LocalDateTime?, DateTimeCellConverter, DateTimeTextConverter> DateTime
        = new("dateTime", "Date & Time", false, (a, b) => a == b);

    public static readonly DataType<decimal?, DecimalCellConverter, DecimalTextConverter> Decimal
        = new("decimal", "Decimal", false, (a, b) => a == b);

    public static readonly DataType<Guid?, GuidCellConverter, GuidTextConverter> Guid
        = new("guid", "Guid", false, (a, b) => a == b);

    public static readonly DataType<long?, IntegerCellConverter, IntegerTextConverter> Integer
        = new("integer", "Integer", false, (a, b) => a == b);

    public static readonly DataType<INamedLookup, LookupCellConverter, LookupTextConverter> Lookup
        = new("lookup", "Lookup", true, (a, b) => a?.Id == b?.Id);

    public static readonly DataType<Money, MoneyCellConverter, MoneyTextConverter> Money
        = new("money", "Money", false, (a, b) => a == b);

    public static readonly DataType<IPublishedContent, PublishedContentCellConverter, PublishedContentTextConverter> PublishedContent
        = new("publishedContent", "Published Content", false, (a, b) => a?.Id == b?.Id);
    
    public static readonly DataType<Reference, ReferenceCellConverter, ReferenceTextConverter> Reference
        = new("reference", "Reference", false, (a, b) => a == b);

    public static readonly DataType<string, StringCellConverter, StringTextConverter> String
        = new("string", "String", false, (a, b) => a == b);

    public static readonly DataType<LocalTime?, TimeCellConverter, TimeTextConverter> Time
        = new("time", "Time", false, (a, b) => a == b);

    public static readonly DataType<YearMonth?, YearMonthCellConverter, YearMonthTextConverter> YearMonth
        = new("yearMonth", "Year Month", false, (a, b) => a == b);

    public static DataType FindByClrType(Type clrType) {
        return GetAllTypes().SingleOrDefault(x => x.GetClrType() == clrType);
    }

    public static IEnumerable<DataType> GetAllTypes() => StaticLookups.GetAll<DataTypes, DataType>().ToArray();

    public static readonly IEnumerable<DataType> NumericTypes = new DataType[] {
        Decimal, Integer, Money
    };

    public static readonly IEnumerable<DataType> OrdinalTypes = new DataType[] {
        Date, DateTime, Decimal, Integer, Money, Time, YearMonth
    };
}
