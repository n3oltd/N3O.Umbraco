using N3O.Umbraco.Content;
using N3O.Umbraco.Crm.Lookups;
using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.Crowdfunding.Lookups;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Mediator;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public class RecalculateContributionTotalsHandler : IRequestHandler<RecalculateContributionTotalsCommand, None, None> {
    private readonly IContentLocator _contentLocator;
    private readonly ICrowdfunderRepository _crowdfunderRepository;
    private readonly ICrowdfunderRevisionRepository _crowdfunderRevisionRepository;
    private readonly ICrowdfundingNotifications _crowdfundingNotifications;
    private readonly ICrowdfundingUrlBuilder _crowdfundingUrlBuilder;

    public RecalculateContributionTotalsHandler(ICrowdfunderRepository crowdfunderRepository,
                                                ICrowdfunderRevisionRepository crowdfunderRevisionRepository,
                                                IContentLocator contentLocator,
                                                ICrowdfundingNotifications crowdfundingNotifications,
                                                ICrowdfundingUrlBuilder crowdfundingUrlBuilder) {
        _crowdfunderRepository = crowdfunderRepository;
        _crowdfunderRevisionRepository = crowdfunderRevisionRepository;
        _contentLocator = contentLocator;
        _crowdfundingNotifications = crowdfundingNotifications;
        _crowdfundingUrlBuilder = crowdfundingUrlBuilder;
    }

    public async Task<None> Handle(RecalculateContributionTotalsCommand req, CancellationToken cancellationToken) {
        await _crowdfunderRepository.RecalculateContributionsTotalAsync(req.ContentId.Value);
        await _crowdfunderRevisionRepository.AddGoalUpdatedOn(req.ContentId.Value);

        await SendFundraiserNotificationEmailAsync(req.ContentId.Value);

        return None.Empty;
    }

    private async Task SendFundraiserNotificationEmailAsync(Guid id) {
        var crowdfunder = await _crowdfunderRepository.FindCrowdfunderByIdAsync(id);

        if (crowdfunder.Type != CrowdfunderTypes.Fundraiser.Key) {
            return;
        }
        
        var fundraiser = _contentLocator.ById<FundraiserContent>(id);

        EnqueueFundraiserNotificationEmail(crowdfunder, fundraiser);
    }

    private void EnqueueFundraiserNotificationEmail(Crowdfunder crowdfunder, FundraiserContent fundraiser) {
        var fundraiserContentViewModel = new FundraiserContentViewModel(_crowdfundingUrlBuilder, fundraiser);
        var goalsTotalViewModel = new FundraiserGoalsTotalViewModel(crowdfunder.GoalsTotalBase,
                                                                    crowdfunder.ContributionsTotalBase);
        
        var model = new FundraiserNotificationViewModel(fundraiserContentViewModel, goalsTotalViewModel);
        
        if (crowdfunder.GoalsTotalBase == crowdfunder.ContributionsTotalBase) {
            _crowdfundingNotifications.Enqueue(FundraiserNotificationTypes.GoalsCompleted, model, fundraiser.Key);
        } else if (crowdfunder.GoalsTotalBase < crowdfunder.ContributionsTotalBase) {
            _crowdfundingNotifications.Enqueue(FundraiserNotificationTypes.GoalsExceeded, model, fundraiser.Key);
        }
    }
}