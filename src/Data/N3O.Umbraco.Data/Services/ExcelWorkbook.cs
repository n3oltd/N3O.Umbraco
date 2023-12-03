using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Data;

public class ExcelWorkbook : IExcelWorkbook {
    private readonly IFormatter _formatter;
    private readonly List<ExcelWorksheetWriter> _worksheetsWriters = new();
    private string _password;

    public ExcelWorkbook(IFormatter formatter) {
        _formatter = formatter;
    }

    public ExcelWorksheetWriter AddWorksheet(string name) {
        var worksheetWriter = new ExcelWorksheetWriter(_formatter);

        _worksheetsWriters.Add(worksheetWriter);
        
        worksheetWriter.SetSheetName(name);
        
        return worksheetWriter;
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
                writer.Write(package.Workbook.Worksheets);
            }

            var packageBytes = await package.GetAsByteArrayAsync(cancellationToken);
        
            stream.Write(packageBytes);
        }
    }
}
