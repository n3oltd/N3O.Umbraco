using CsvHelper;
using CsvHelper.Configuration;
using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using IParser = N3O.Umbraco.Data.Parsing.IParser;

namespace N3O.Umbraco.Data;

public class CsvReader : ICsvReader {
    private readonly IParser _parser;
    private readonly ITableBuilder _tableBuilder;
    private readonly IColumnRangeBuilder _columnRangeBuilder;
    private readonly bool _hasColumnHeadings;
    private readonly StreamReader _streamReader;
    private readonly CsvHelper.CsvReader _csv;
    private readonly IReadOnlyList<string> _columnHeadings;
    private readonly Dictionary<string, int> _columnIndexMap = new();
    private bool? _firstRowResult;

    public CsvReader(IParser parser,
                     ITableBuilder tableBuilder,
                     IColumnRangeBuilder columnRangeBuilder,
                     TextEncoding textEncoding,
                     Stream stream,
                     bool hasColumnHeadings) {
        var encoding = Encoding.GetEncoding(textEncoding.CodePage);
        
        _parser = parser;
        _tableBuilder = tableBuilder;
        _columnRangeBuilder = columnRangeBuilder;
        _hasColumnHeadings = hasColumnHeadings;
        _streamReader = new StreamReader(stream, encoding, leaveOpen: true);

        var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture);
        csvConfiguration.Encoding = encoding;
        csvConfiguration.HasHeaderRecord = hasColumnHeadings;
        csvConfiguration.BadDataFound = null;
        csvConfiguration.ReadingExceptionOccurred = OnReadingExceptionOccurred;
        csvConfiguration.ShouldSkipRecord = r => r.Record.None(f => f.HasValue());
        csvConfiguration.TrimOptions = TrimOptions.Trim | TrimOptions.InsideQuotes;

        _csv = new CsvHelper.CsvReader(_streamReader, csvConfiguration);
        
        if (hasColumnHeadings) {
            _csv.Read();
            _csv.ReadHeader();
            _columnHeadings = _csv.HeaderRecord.Select(x => x.HasValue() ? x.Trim() : null).ToList();
            ColumnCount = _columnHeadings.Count;
            ReadRow(true);
        } else {
            ReadRow(true);
            ColumnCount = Row.GetColumnCount();
        }
    }

    public int ColumnCount { get; }

    public IReadOnlyList<string> GetColumnHeadings() {
        if (!_hasColumnHeadings) {
            throw new Exception($"Cannot call {nameof(GetColumnHeadings)} when {nameof(_hasColumnHeadings)} is false");
        }

        return _columnHeadings;
    }

    public bool ReadRow() => ReadRow(false);
    
    public ITable ReadTable(string name) {
        var tableBuilderActions = GetTableBuilderActions();

        var tableBuilder = _tableBuilder.Untyped(name);

        while (ReadRow()) {
            foreach (var action in tableBuilderActions) {
                action(tableBuilder);
            }
            
            tableBuilder.NextRow();
        }

        var table = tableBuilder.Build();

        return table;
    }

    public void Skip(int rows) {
        for (var i = 0; i < rows; i++) {
            _csv.Read();
        }
    }

    public void Dispose() {
        _csv?.Dispose();
        _streamReader?.Dispose();
    }

    private int GetColumnIndex(string heading) {
        if (!_hasColumnHeadings) {
            throw new Exception($"Cannot retrieve columns by heading as {nameof(_hasColumnHeadings)} is false");
        }

        heading = heading.Trim();
        
        var result = _columnIndexMap.GetOrAdd(heading, () => {
            var index = _columnHeadings.IndexOf(x => string.Equals(x,
                                                                   heading,
                                                                   StringComparison.InvariantCultureIgnoreCase));

            return index;
        });

        return result;
    }

    public CsvRow Row { get; private set; }
    
    public event EventHandler<CsvErrorEventArgs> OnRowError;

    private bool OnReadingExceptionOccurred(ReadingExceptionOccurredArgs args) {
        return true;
    }

    private bool ReadRow(bool firstRead) {
        bool result;
        
        if (_firstRowResult.HasValue()) {
            result = _firstRowResult.Value;
            
            _firstRowResult = null;
        } else {
            result = _csv.Read();

            if (result) {
                Row = new CsvRow(_parser,
                                 _csv.Parser.Row,
                                 () => _csv.Parser.Count,
                                 GetColumnIndex,
                                 index => {
                                     try {
                                         return (true, _csv.GetField(index));
                                     } catch {
                                         return (false, null);
                                     }
                                 });

                Row.OnError += RaiseError;
            } else {
                Row = null;
            }

            if (firstRead) {
                _firstRowResult = result;
            }
        }

        return result;
    }
    
    private IReadOnlyList<Action<IUntypedTableBuilder>> GetTableBuilderActions() {
        var tableBuilderActions = new List<Action<IUntypedTableBuilder>>();
        
        if (_hasColumnHeadings) {
            foreach (var columnHeading in GetColumnHeadings()) {
                var columnRange = _columnRangeBuilder.String<string>().Title(columnHeading).Build();
                
                tableBuilderActions.Add(b => b.AddValue(columnRange, Row.GetRawField(columnHeading)));
            }
        } else {
            for (var i = 0; i < ColumnCount; i++) {
                var columnNumber = i + 1;
                var columnRange = _columnRangeBuilder.String<string>().Title(columnNumber.ToString()).Build();
                
                tableBuilderActions.Add(b => b.AddValue(columnRange, Row.GetRawField(columnNumber)));
            }
        }

        return tableBuilderActions;
    }

    private void RaiseError(object sender, CsvErrorEventArgs e) {
        var handler = OnRowError;
        handler?.Invoke(this, e);
    }
}
