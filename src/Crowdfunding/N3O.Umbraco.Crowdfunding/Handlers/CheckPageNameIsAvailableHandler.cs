using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crowdfunding.Queries;
using N3O.Umbraco.Mediator;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public class CheckPageNameIsAvailableHandler : IRequestHandler<CheckPageNameIsAvailableQuery, CreatePageReq, bool> {
    private readonly IFundraisingPages _fundraisingPages;

    public CheckPageNameIsAvailableHandler(IFundraisingPages fundraisingPages) {
        _fundraisingPages = fundraisingPages;
    }
    
    public Task<bool> Handle(CheckPageNameIsAvailableQuery req, CancellationToken cancellationToken) {
        var res = _fundraisingPages.IsPageNameAvailable(req.Model.Name);

        return Task.FromResult(res);
    }
}