using N3O.Umbraco.Data.Models;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Data.Services {
    public interface IExcelWorkbook {
        void AddWorksheet(IExcelTable table);
        void FormatAsTable(bool enabled);
        void PasswordProtect(string password);
        Task SaveAsync(Stream stream, CancellationToken cancellationToken = default);
    }
}