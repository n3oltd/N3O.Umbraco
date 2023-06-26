using N3O.Umbraco.Newsletters.SendGrid.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Newsletters.SendGrid;

public interface IMarketingApiClient : IApiClient {
    Task AddOrUpdateContactAsync(string email,
                                 string listId,
                                 IReadOnlyDictionary<string, object> reservedFields,
                                 IReadOnlyDictionary<string, object> customFields);
    Task<IReadOnlyList<FieldDefinition>> GetAllFieldDefinitionsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SendGridList>> GetAllListsAsync(CancellationToken cancellationToken = default);
    Task<SendGridList> GetListAsync(string listId, CancellationToken cancellationToken = default);
    Task<RemoveContactResult> RemoveContactAsync(string email, string listId);
}