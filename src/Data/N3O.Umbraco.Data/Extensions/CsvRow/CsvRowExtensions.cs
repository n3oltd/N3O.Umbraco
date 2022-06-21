using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.Parsing;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using System;

namespace N3O.Umbraco.Data.Extensions;

public static partial class CsvRowExtensions {
    public static object SelectField(this CsvRow csvRow, CsvSelect select, DataType dataType, Type targetType) {
        if (select.ColumnIdentifier == CsvColumnIdentifiers.Heading) {
            return csvRow.ParseField(select.Heading, dataType, targetType);
        } else if (select.ColumnIdentifier == CsvColumnIdentifiers.Index) {
            return csvRow.ParseField(select.Index.GetValueOrThrow() - 1, dataType, targetType);
        } else {
            throw UnrecognisedValueException.For(select.ColumnIdentifier);
        }
    }

    public static T SelectField<T>(this CsvRow csvRow,
                                   CsvSelect select,
                                   Func<IParser, string, ParseResult<T>> parse) {
        if (select.ColumnIdentifier == CsvColumnIdentifiers.Heading) {
            return csvRow.ParseField(select.Heading, parse);
        } else if (select.ColumnIdentifier == CsvColumnIdentifiers.Index) {
            return csvRow.ParseField(select.Index.GetValueOrThrow() - 1, parse);
        } else {
            throw UnrecognisedValueException.For(select.ColumnIdentifier);
        }
    }
}
