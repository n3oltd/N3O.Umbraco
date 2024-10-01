using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Lookups;
using N3O.Umbraco.Mediator;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public class PublishFundraiserHandler : IRequestHandler<PublishFundraiserCommand, None, None> {
    private readonly IContentService _contentService;

    public PublishFundraiserHandler(IContentService contentService) {
        _contentService = contentService;
    }

    public async Task<None> Handle(PublishFundraiserCommand req, CancellationToken cancellationToken) {
        var fundraiser = req.FundraiserId.Run(_contentService.GetById, true);
        
        fundraiser.SetValue(CrowdfundingConstants.Crowdfunder.Properties.Status, CrowdfunderStatuses.Active.Name);
        fundraiser.SetValue(CrowdfundingConstants.Crowdfunder.Properties.ToggleStatus, true);

        _contentService.SaveAndPublish(fundraiser);

        return None.Empty;
    }
}