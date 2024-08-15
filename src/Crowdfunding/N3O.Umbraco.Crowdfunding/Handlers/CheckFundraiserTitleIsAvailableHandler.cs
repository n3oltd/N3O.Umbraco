using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crowdfunding.Queries;
using N3O.Umbraco.Mediator;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public class CheckFundraiserTitleIsAvailableHandler :
    IRequestHandler<CheckFundraiserTitleIsAvailableQuery, CreateFundraiserReq, bool> {
    private readonly ICrowdfundingHelper _crowdfundingHelper;

    public CheckFundraiserTitleIsAvailableHandler(ICrowdfundingHelper crowdfundingHelper) {
        _crowdfundingHelper = crowdfundingHelper;
    }
    
    public Task<bool> Handle(CheckFundraiserTitleIsAvailableQuery req, CancellationToken cancellationToken) {
        var res = _crowdfundingHelper.IsFundraiserTitleAvailable(req.Model.Title);

        return Task.FromResult(res);
    }
}