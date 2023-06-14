using CsvHelper;
using CsvHelper.Configuration;
using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Data;

public class CsvWorkbook : ICsvWorkbook {
    private ITable _table;
    private bool _headers = true;
    private TextEncoding _encoding = TextEncodings.Utf8;

    public void AddTable(ITable table) {
        _table = table;
    }

    public void Encoding(TextEncoding encoding) {
        _encoding = encoding;
    }

    public void Headers(bool enabled) {
        _headers = enabled;
    }

    public async Task SaveAsync(Stream stream, CancellationToken cancellationToken = default) {
        var textEncoding = System.Text.Encoding.GetEncoding(_encoding.CodePage);

        await using (var memoryStream = new MemoryStream()) {
            await using (var writer = new StreamWriter(memoryStream, textEncoding)) {
                var configuration = GetCsvConfiguration();

                await using (var csv = new CsvWriter(writer, configuration)) {
                    if (_headers) {
                        await WriteHeadersAsync(csv, _table.Columns);
                    }

                    await WriteBodyAsync(csv, _table.Columns);
                }
            }

            var bytes = memoryStream.ToArray();

            stream.Write(textEncoding.GetPreamble());
            stream.Write(bytes);
        }
    }

    public async Task WriteTemplateAsync(IEnumerable<Column> columns,
                                         Stream stream,
                                         CancellationToken cancellationToken = default) {
        var textEncoding = System.Text.Encoding.GetEncoding(_encoding.CodePage);

        await using (var memoryStream = new MemoryStream()) {
            await using (var writer = new StreamWriter(memoryStream, textEncoding)) {
                var configuration = GetCsvConfiguration();

                await using (var csv = new CsvWriter(writer, configuration)) {
                    await WriteHeadersAsync(csv, columns);
                }
            }

            var bytes = memoryStream.ToArray();

            stream.Write(textEncoding.GetPreamble());
            stream.Write(bytes);
        }
    }

    private CsvConfiguration GetCsvConfiguration() {
        var configuration = new CsvConfiguration(CultureInfo.InvariantCulture);
        configuration.NewLine = "\r\n";
        configuration.TrimOptions = TrimOptions.Trim;
        configuration.ShouldQuote = args => Regex.IsMatch(args.Field ?? "",
                                                          @"[\s\n]",
                                                          RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

        return configuration;
    }

    private async Task WriteHeadersAsync(CsvWriter csv, IEnumerable<Column> columns) {
        foreach (var column in columns) {
            csv.WriteField(column.Title);
        }

        await csv.NextRecordAsync();
    }

    private async Task WriteBodyAsync(CsvWriter csv, IEnumerable<Column> columns) {
        for (var row = 0; row < _table.RowCount; row++) {
            foreach (var column in columns) {
                var cell = _table[column, row];

                var csvText = GetCsvText(column.Formatter, cell);

                csv.WriteField(csvText, csvText.ContainsAny(',', '\n'));
            }

            await csv.NextRecordAsync();
        }
    }

    private string GetCsvText(IFormatter formatter, Cell cell) {
        var csvValue = "";

        if (cell.HasValue(x => x.Value)) {
            if (cell.Type.IsAnyOf(Lookups.DataTypes.Date,
                                  Lookups.DataTypes.DateTime,
                                  Lookups.DataTypes.Decimal,
                                  Lookups.DataTypes.Integer,
                                  Lookups.DataTypes.Time)) {
                csvValue = cell.Type.ToInvariantText(cell.Value);
            } else {
                csvValue = cell.Type.ToText(formatter, cell.Value);
            }
        }

        return csvValue;
    }
}
