using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Data;

public interface IExcelWorkbook {
    ExcelWorksheetWriter AddWorksheet(string name);
    void PasswordProtect(string password);
    Task SaveAsync(Stream stream, CancellationToken cancellationToken = default);
}
