using CsvHelper;
using CsvHelper.Configuration;
using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.Extensions;
using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Data.Services {
    public class CsvWorkbook : ICsvWorkbook {
        private readonly IColumnRangeBuilder _columnRangeBuilder;
        private readonly IColumnVisibility _columnVisibility;
        private ITable _table;
        private bool _headers = true;
        private TextEncoding _encoding = TextEncodings.Utf8;

        public CsvWorkbook(IColumnRangeBuilder columnRangeBuilder, IColumnVisibility columnVisibility) {
            _columnRangeBuilder = columnRangeBuilder;
            _columnVisibility = columnVisibility;
        }

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
                        var visibleColumns = new List<Column>();

                        foreach (var column in _table.Columns) {
                            var isVisible = _columnVisibility.IsVisible(column);

                            if (isVisible) {
                                visibleColumns.Add(column);
                            }
                        }

                        if (_headers) {
                            await WriteHeadersAsync(csv, visibleColumns);
                        }

                        await WriteBodyAsync(csv, visibleColumns);
                    }
                }

                var bytes = memoryStream.ToArray();

                stream.Write(textEncoding.GetPreamble());
                stream.Write(bytes);
            }
        }

        public async Task WriteTemplateAsync(IEnumerable<TemplateColumn> templateColumns,
                                             Stream stream,
                                             CancellationToken cancellationToken = default) {
            var textEncoding = System.Text.Encoding.GetEncoding(_encoding.CodePage);

            await using (var memoryStream = new MemoryStream()) {
                await using (var writer = new StreamWriter(memoryStream, textEncoding)) {
                    var configuration = GetCsvConfiguration();

                    await using (var csv = new CsvWriter(writer, configuration)) {
                        var columns = new List<Column>();

                        foreach (var templateColumn in templateColumns.OrEmpty()) {
                            columns.AddRange(_columnRangeBuilder.GetColumns(templateColumn));
                        }

                        if (_headers) {
                            await WriteHeadersAsync(csv, columns);
                        }
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

            return configuration;
        }

        private async Task WriteHeadersAsync(CsvWriter csv, IReadOnlyList<Column> columns) {
            foreach (var column in columns) {
                csv.WriteField(column.Title);
            }

            await csv.NextRecordAsync();
        }

        private async Task WriteBodyAsync(CsvWriter csv, IReadOnlyList<Column> columns) {
            for (var row = 0; row < _table.RowCount; row++) {
                foreach (var column in columns) {
                    var cell = _table[column, row];

                    var csvText = GetCsvText(column, cell);

                    csv.WriteField(csvText);
                }

                await csv.NextRecordAsync();
            }
        }

        private string GetCsvText(Column column, Cell cell) {
            var csvValue = "";

            if (cell.Value.HasValue()) {
                csvValue = cell.Type.ConvertToText(column.Formatter, cell.Value);
            }

            return csvValue;
        }
    }
}