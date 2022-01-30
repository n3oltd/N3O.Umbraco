using N3O.Umbraco.Giving.Accounts.Models;
using N3O.Umbraco.Mediator;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Accounts.Queries {
    public class GetConsentOptionsQuery : Request<None, IEnumerable<ConsentOption>> { }
}