using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crowdfunding.Queries;
using N3O.Umbraco.Mediator;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public class CheckFundraiserNameIsAvailableHandler :
    IRequestHandler<CheckFundraiserNameIsAvailableQuery, CreateFundraiserReq, bool> {
    private readonly ICrowdfundingHelper _crowdfundingHelper;

    public CheckFundraiserNameIsAvailableHandler(ICrowdfundingHelper crowdfundingHelper) {
        _crowdfundingHelper = crowdfundingHelper;
    }
    
    public Task<bool> Handle(CheckFundraiserNameIsAvailableQuery req, CancellationToken cancellationToken) {
        var res = _crowdfundingHelper.IsFundraiserNameAvailable(req.Model.Name);

        return Task.FromResult(res);
    }
}