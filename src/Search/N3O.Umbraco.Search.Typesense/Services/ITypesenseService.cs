using System.Threading;
using System.Threading.Tasks;
using N3O.Umbraco.Search.Typesense.Models;

namespace N3O.Umbraco.Search.Typesense.Services;

public interface ITypesenseService
{
    Task UpsertAsync(SearchDocument searchDocument, CancellationToken cancellationToken);
    Task CheckConnectionAsync(CancellationToken cancellationToken);
}