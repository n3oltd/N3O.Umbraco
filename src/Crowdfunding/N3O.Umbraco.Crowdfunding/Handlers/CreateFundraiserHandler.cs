using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Validation;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public class CreateFundraiserHandler : IRequestHandler<CreateFundraiserCommand, CreateFundraiserReq, string> {
    private readonly ICrowdfundingHelper _crowdfundingHelper;

    public CreateFundraiserHandler(ICrowdfundingHelper crowdfundingHelper) {
        _crowdfundingHelper = crowdfundingHelper;
    }
    
    public async Task<string> Handle(CreateFundraiserCommand req, CancellationToken cancellationToken) {
        var result = await _crowdfundingHelper.CreateFundraiserAsync(req);

        if (result.Success) {
            return result.Url;
        } else {
            throw new ValidationException(ValidationFailure.WithMessage(null, result.Error));
        }
    }
}