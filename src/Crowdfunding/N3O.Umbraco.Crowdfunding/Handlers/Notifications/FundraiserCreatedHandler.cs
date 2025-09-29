using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Lookups;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Mediator;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public class FundraiserCreatedHandler : IRequestHandler<FundraiserCreatedNotification, None, None> {
    private readonly IContentLocator _contentLocator;
    private readonly ICrowdfundingNotifications _crowdfundingNotifications;
    private readonly ICrowdfundingUrlBuilder _crowdfundingUrlBuilder;

    public FundraiserCreatedHandler(IContentLocator contentLocator,
                                    ICrowdfundingNotifications crowdfundingNotifications,
                                    ICrowdfundingUrlBuilder crowdfundingUrlBuilder) {
        _contentLocator = contentLocator;
        _crowdfundingNotifications = crowdfundingNotifications;
        _crowdfundingUrlBuilder = crowdfundingUrlBuilder;
    }

    public Task<None> Handle(FundraiserCreatedNotification req, CancellationToken cancellationToken) {
        var fundraiser = req.ContentId.Run(x => _contentLocator.ById<FundraiserContent>(x), true);
        
        var fundraiserContentViewModel = new FundraiserContentViewModel(_crowdfundingUrlBuilder, fundraiser);
        var model = new FundraiserNotificationViewModel(fundraiserContentViewModel, null);

        _crowdfundingNotifications.Enqueue(FundraiserNotificationTypes.FundraiserCreated, model, fundraiser.Key);

        return Task.FromResult(None.Empty);
    }
}