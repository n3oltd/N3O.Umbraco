using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Data.Services {
    public interface ICsvWorkbook {
        void AddTable(ITable table);
        void Encoding(TextEncoding encoding);
        void Headers(bool enabled);
        Task SaveAsync(Stream stream, CancellationToken cancellation = default);
        Task WriteTemplateAsync(IEnumerable<TemplateColumn> templateColumns,
                                Stream stream,
                                CancellationToken cancellationToken = default);
    }
}