using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Mediator;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public partial class CreateOrUpdateFundraiserHandlers :
    IRequestHandler<UpdateFundraiserGoalsCommand, FundraiserGoalsReq, None> {
    public Task<None> Handle(UpdateFundraiserGoalsCommand req, CancellationToken cancellationToken) {
        var fundraiser = req.ContentId.Run(_contentLocator.ById<FundraiserContent>, true);
        var contentPublisher =_contentEditor.ForExisting(fundraiser.Content().Key);

        PopulateFundraiserGoals(contentPublisher, req.Model.Items, fundraiser.Campaign);

        var publishResult = contentPublisher.SaveAndPublish();

        if (!publishResult.Success) {
            throw ToValidationException(publishResult);
        }

        return Task.FromResult(None.Empty);
    }
}