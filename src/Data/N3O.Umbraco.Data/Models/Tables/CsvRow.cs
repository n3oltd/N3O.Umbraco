using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Parsing;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using System;

namespace N3O.Umbraco.Data.Models;

public class CsvRow {
    private readonly IParser _parser;
    private readonly int _rowNumber;
    private readonly Func<int> _getColumnCount;
    private readonly Func<string, int> _getFieldIndexByName;
    private readonly Func<int, (bool, string)> _getRawFieldByIndex;

    public CsvRow(IParser parser,
                  int rowNumber,
                  Func<int> getColumnCount,
                  Func<string, int> getFieldIndexByName,
                  Func<int, (bool, string)> getRawFieldByIndex) {
        _parser = parser;
        _rowNumber = rowNumber;
        _getColumnCount = getColumnCount;
        _getFieldIndexByName = getFieldIndexByName;
        _getRawFieldByIndex = getRawFieldByIndex;
    }

    public object ParseField(string heading, DataType dataType, Type targetType) {
        return ParseField(CsvSelect.For(heading), dataType, targetType);
    }

    public object ParseField(int index, DataType dataType, Type targetType) {
        return ParseField(CsvSelect.For(index), dataType, targetType);
    }

    public object ParseField(CsvSelect select, DataType dataType, Type targetType) {
        return ParseField(select, (parser, field) => parser.Parse(field, dataType, targetType));
    }

    public T ParseField<T>(string heading, Func<IParser, string, ParseResult<T>> parse) {
        return ParseField(CsvSelect.For(heading), parse);
    }

    public T ParseField<T>(int index, Func<IParser, string, ParseResult<T>> parse) {
        return ParseField(CsvSelect.For(index), parse);
    }

    public T ParseField<T>(CsvSelect select, Func<IParser, string, ParseResult<T>> parse) {
        var rawField = GetRawField(select);

        var parseResult = parse(_parser, rawField);

        if (parseResult.Success) {
            return parseResult.Value;
        } else {
            RaiseError(CsvErrors.ParsingFailed, select, rawField);

            return default;
        }
    }

    public int GetColumnCount() {
        return _getColumnCount();
    }

    public string GetRawField(string heading) {
        return GetRawField(CsvSelect.For(heading));
    }

    public string GetRawField(int index) {
        return GetRawField(CsvSelect.For(index));
    }

    public string GetRawField(CsvSelect select) {
        int index;

        if (select.ColumnIdentifier == CsvColumnIdentifiers.Heading) {
            index = _getFieldIndexByName(select.Heading);
        } else if (select.ColumnIdentifier == CsvColumnIdentifiers.Index) {
            index = select.Index.GetValueOrThrow();
        } else {
            throw UnrecognisedValueException.For(select.ColumnIdentifier);
        }

        var (success, rawField) = _getRawFieldByIndex(index);

        if (success) {
            return rawField;
        } else {
            RaiseError(CsvErrors.MissingColumn, select);

            return null;
        }
    }

    public event EventHandler<CsvErrorEventArgs> OnError;

    private void RaiseError(CsvError error, CsvSelect select, string rawField = null) {
        var eventArgs = new CsvErrorEventArgs(error, select, _rowNumber, rawField);

        var handler = OnError;
        handler?.Invoke(this, eventArgs);
    }
}
