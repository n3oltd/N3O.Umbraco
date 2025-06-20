using AsyncKeyedLock;
using N3O.Umbraco.Cloud.Engage.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Extensions;
using N3O.Umbraco.Crowdfunding.Handlers;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crowdfunding.NamedParameters;
using N3O.Umbraco.Email;
using N3O.Umbraco.Email.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Json;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Scheduler;
using N3O.Umbraco.TaxRelief.Lookups;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Events;

public class CheckoutUpdatedHandler : CheckoutEventHandler<CheckoutUpdatedEvent> {
    private readonly IClock _clock;
    private readonly IBackgroundJob _backgroundJob;
    private readonly IContributionRepository _contributionRepository;
    private readonly IJsonProvider _jsonProvider;
    private readonly IContentLocator _contentLocator;
    private readonly IEmailBuilder _emailBuilder;
    private readonly ICrowdfundingUrlBuilder _crowdfundingUrlBuilder;
    private readonly ILookups _lookups;

    public CheckoutUpdatedHandler(IBackgroundJob backgroundJob,
                                  IClock clock,
                                  IContributionRepository contributionRepository,
                                  IJsonProvider jsonProvider,
                                  IContentLocator contentLocator,
                                  IEmailBuilder emailBuilder,
                                  ICrowdfundingUrlBuilder crowdfundingUrlBuilder,
                                  AsyncKeyedLocker<string> asyncKeyedLocker,
                                  ILookups lookups)
        : base(asyncKeyedLocker) {
        _contributionRepository = contributionRepository;
        _backgroundJob = backgroundJob;
        _jsonProvider = jsonProvider;
        _clock = clock;
        _contentLocator = contentLocator;
        _emailBuilder = emailBuilder;
        _crowdfundingUrlBuilder = crowdfundingUrlBuilder;
        _lookups = lookups;
    }

    protected override async Task HandleEventAsync(CheckoutUpdatedEvent req, CancellationToken cancellationToken) {
        var reference = req.Model.Reference.Text;
        
        await ProcessItemsAsync(reference, req.Model, req.Model.Donation.CartItems, GivingTypes.Donation);
    }

    private async Task ProcessItemsAsync(string reference,
                                         WebhookCheckout checkout,
                                         IEnumerable<WebhookCartItem> cartItems,
                                         GivingType givingType) {
        foreach (var cartItem in cartItems.Where(x => x.HasCrowdfunderData())) {
            var crowdfunderData = cartItem.GetCrowdfunderData(_jsonProvider);

            await RecordContributionAsync(reference, givingType, cartItem, crowdfunderData, checkout.Account);
            
            RefreshCrowdfunderStatistics(crowdfunderData);
            
            QueueEmail(checkout, crowdfunderData, cartItem);
        }
    }

    private async Task RecordContributionAsync(string reference,
                                               GivingType givingType,
                                               WebhookCartItem cartItem,
                                               ICrowdfunderData crowdfunderData,
                                               WebhookCheckoutAccountInfo account) {
        var name = account.GetName(_lookups);
        
        await _contributionRepository.AddOnlineContributionAsync(reference,
                                                                 _clock.GetCurrentInstant(),
                                                                 crowdfunderData,
                                                                 account.Email?.Address,
                                                                 name,
                                                                 account.TaxStatus == TaxStatuses.Payer,
                                                                 cartItem.FundDimensions.Dimension1,
                                                                 cartItem.FundDimensions.Dimension2,
                                                                 cartItem.FundDimensions.Dimension3,
                                                                 cartItem.FundDimensions.Dimension4,
                                                                 cartItem.Value.ToMoney(_lookups),
                                                                 givingType,
                                                                 cartItem.GetSummary(),
                                                                 cartItem);
    }

    private void RefreshCrowdfunderStatistics(ICrowdfunderData crowdfunderData) {
        CrowdfunderDebouncer.Debounce(crowdfunderData.Id, crowdfunderData.Type, EnqueueRecalculateContributionsTotal);
    }

    private void EnqueueRecalculateContributionsTotal(Guid id, CrowdfunderType type) {
        _backgroundJob.Enqueue<RecalculateContributionTotalsCommand>($"{nameof(RecalculateContributionTotalsCommand).Replace("Command", "")} {id.ToString()}",
                                                                     p => {
                                                                         p.Add<ContentId>(id.ToString());
                                                                         p.Add<CrowdfunderTypeId>(type.Id);
                                                                     });
    }

    private void QueueEmail(WebhookCheckout checkout, CrowdfunderData crowdfunderData, WebhookCartItem cartItem) {
        if (crowdfunderData.Type == CrowdfunderTypes.Fundraiser) {
            var fundraiser = _contentLocator.ById<FundraiserContent>(crowdfunderData.Id);
            var template = _contentLocator.Single<FundraiserContributionReceivedTemplateContent>();

            var model = new FundraiserContributionReceivedViewModel(new FundraiserContentViewModel(_crowdfundingUrlBuilder, fundraiser),
                                                                    checkout,
                                                                    cartItem,
                                                                    crowdfunderData);

            _emailBuilder.QueueTemplate(template, model.Fundraiser.FundraiserEmail, model);
        }
    }
}