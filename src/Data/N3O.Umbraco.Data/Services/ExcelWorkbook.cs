using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Extensions;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Data;

public class ExcelWorkbook : IExcelWorkbook {
    private readonly IColumnVisibility _columnVisibility;
    private readonly List<ExcelWorksheetWriter> _worksheetsWriters = new();
    private string _password;
    private bool _formatAsTable = true;

    public ExcelWorkbook(IColumnVisibility columnVisibility) {
        _columnVisibility = columnVisibility;
    }

    public void AddWorksheet(IExcelTable table) {
        var worksheetWriter = new ExcelWorksheetWriter(table, _columnVisibility);

        _worksheetsWriters.Add(worksheetWriter);
    }

    public void FormatAsTable(bool enabled) {
        _formatAsTable = enabled;
    }

    public void PasswordProtect(string password) {
        _password = password;
    }

    public async Task SaveAsync(Stream stream, CancellationToken cancellationToken = default) {
        using (var package = new ExcelPackage()) {
            if (_password.HasValue()) {
                package.Encryption.IsEncrypted = true;
                package.Encryption.Password = _password;
            }
        
            foreach (var writer in _worksheetsWriters) {
                writer.Write(package.Workbook.Worksheets, _formatAsTable);
            }

            var packageBytes = await package.GetAsByteArrayAsync(cancellationToken);
        
            stream.Write(packageBytes);
        }
    }
}
