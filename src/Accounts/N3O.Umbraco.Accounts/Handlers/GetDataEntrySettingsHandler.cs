using N3O.Umbraco.Accounts.Content;
using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Accounts.Queries;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Mediator;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Accounts.Handlers {
    public class GetDataEntrySettingsHandler : IRequestHandler<GetDataEntrySettingsQuery, None, DataEntrySettings> {
        private readonly IContentCache _contentCache;

        public GetDataEntrySettingsHandler(IContentCache contentCache) {
            _contentCache = contentCache;
        }
        
        public Task<DataEntrySettings> Handle(GetDataEntrySettingsQuery req, CancellationToken cancellationToken) {
            var dataEntrySettingsContent = _contentCache.Single<DataEntrySettingsContent>();
            var consentOptionsContent = _contentCache.All<ConsentOptionContent>();

            var settings = dataEntrySettingsContent.IfNotNull(x => x.ToDataEntrySettings(consentOptionsContent));

            return Task.FromResult(settings);
        }
    }
}