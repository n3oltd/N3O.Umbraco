using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Mediator;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public partial class CreateOrUpdateFundraiserHandlers :
    IRequestHandler<CreateFundraiserCommand, CreateFundraiserReq, string> {
    public async Task<string> Handle(CreateFundraiserCommand req, CancellationToken cancellationToken) {
        var fundraisersCollection = _contentLocator.Single(CrowdfundingConstants.Fundraisers.Alias);

        var contentPublisher =_contentEditor.New(Guid.NewGuid().ToString(),
                                                 fundraisersCollection.Key,
                                                 CrowdfundingConstants.Fundraiser.Alias);
         
        await PopulateFundraiserAsync(contentPublisher, req.Model);
        
        var publishResult = contentPublisher.SaveAndPublish();

        if (publishResult.Success) {
            return ViewEditFundraiserPage.Url(_contentLocator, publishResult.Content.Key);
        } else {
            throw ToValidationException(publishResult);
        }
    }
}