using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Accounts.Content;
using N3O.Umbraco.Giving.Accounts.Models;
using N3O.Umbraco.Giving.Accounts.Queries;
using N3O.Umbraco.Mediator;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Accounts.Handlers {
    public class GetConsentOptionsHandler : IRequestHandler<GetConsentOptionsQuery, None, IEnumerable<ConsentOption>> {
        private readonly IContentCache _contentCache;

        public GetConsentOptionsHandler(IContentCache contentCache) {
            _contentCache = contentCache;
        }
        
        public Task<IEnumerable<ConsentOption>> Handle(GetConsentOptionsQuery req, CancellationToken cancellationToken) {
            var res = new List<ConsentOption>();

            foreach (var content in _contentCache.All<ConsentOptionContent>()) {
                res.Add(new ConsentOption(content.Channel, content.Categories, content.Statement));
            }

            return Task.FromResult<IEnumerable<ConsentOption>>(res);
        }
    }
}