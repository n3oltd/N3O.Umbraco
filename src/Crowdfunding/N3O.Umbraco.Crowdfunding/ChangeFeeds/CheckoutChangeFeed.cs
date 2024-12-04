using Microsoft.Extensions.Logging;
using N3O.Umbraco.Accounts.Extensions;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crm.Lookups;
using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Extensions;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crowdfunding.NamedParameters;
using N3O.Umbraco.Email;
using N3O.Umbraco.Email.Extensions;
using N3O.Umbraco.Entities;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Giving.Checkout.Entities;
using N3O.Umbraco.Json;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Scheduler;
using N3O.Umbraco.TaxRelief.Lookups;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.ChangeFeeds;

public class CheckoutChangeFeed : ChangeFeed<Checkout> {
    private readonly IClock _clock;
    private readonly IBackgroundJob _backgroundJob;
    private readonly IFormatter _formatter;
    private readonly IContributionRepository _contributionRepository;
    private readonly IJsonProvider _jsonProvider;
    private readonly IContentLocator _contentLocator;
    private readonly IEmailBuilder _emailBuilder;
    
    public CheckoutChangeFeed(ILogger<CheckoutChangeFeed> logger,
                              IBackgroundJob backgroundJob,
                              IClock clock,
                              IFormatter formatter,
                              IContributionRepository contributionRepository,
                              IJsonProvider jsonProvider,
                              IContentLocator contentLocator,
                              IEmailBuilder emailBuilder) 
        : base(logger) {
        _contributionRepository = contributionRepository;
        _backgroundJob = backgroundJob;
        _jsonProvider = jsonProvider;
        _clock = clock;
        _formatter = formatter;
        _contentLocator = contentLocator;
        _emailBuilder = emailBuilder;
    }
    
    protected override async Task ProcessChangeAsync(EntityChange<Checkout> entityChange) {
        if (entityChange.Operation.IsAnyOf(EntityOperations.Update)) {
            await ProcessAsync(entityChange,
                               x => x.Donation.Allocations,
                               x => x.Donation.IsComplete,
                               GivingTypes.Donation);
            
            await ProcessAsync(entityChange,
                               x => x.RegularGiving.Allocations,
                               x => x.RegularGiving.IsComplete,
                               GivingTypes.RegularGiving);
        }
    }

    private async Task ProcessAsync(EntityChange<Checkout> checkout,
                                    Func<Checkout, IEnumerable<Allocation>> getAllocations,
                                    Func<Checkout, bool> getComplete,
                                    GivingType givingType) {
        var isComplete = getComplete(checkout.SessionEntity);
        var wasComplete = getComplete(checkout.DatabaseEntity);

        if (isComplete && !wasComplete) {
            var allocations = getAllocations(checkout.SessionEntity);

            foreach (var allocation in allocations.Where(x => x.HasCrowdfunderData())) {
                await RecordContributionAsync(givingType, checkout.SessionEntity, allocation);
                
                RefreshCrowdfunderStatistics(allocation);
                
                QueueEmail(checkout, allocation);
            }
            
            await _contributionRepository.CommitAsync();
        }
    }

    private async Task RecordContributionAsync(GivingType givingType, Checkout checkout, Allocation allocation) {
        var crowdfunderData = allocation.GetCrowdfunderData(_jsonProvider);

        await _contributionRepository.AddOnlineContributionAsync(checkout.Reference.Text,
                                                                 _clock.GetCurrentInstant(),
                                                                 crowdfunderData,
                                                                 checkout.Account?.Email?.Address,
                                                                 checkout.Account?.GetName(_formatter),
                                                                 checkout.Account?.TaxStatus == TaxStatuses.Payer,
                                                                 givingType,
                                                                 allocation);
    }

    private void RefreshCrowdfunderStatistics(Allocation allocation) {
        var crowdfunderData = allocation.GetCrowdfunderData(_jsonProvider);

        CrowdfunderDebouncer.Debounce(crowdfunderData.Id, crowdfunderData.Type, EnqueueRecalculateContributionsTotal);
    }
    
    private void EnqueueRecalculateContributionsTotal(Guid id, CrowdfunderType type) {
        _backgroundJob.Enqueue<RecalculateContributionTotalsCommand>($"{nameof(RecalculateContributionTotalsCommand).Replace("Command", "")} {id.ToString()}",
                                                                     p => { 
                                                                         p.Add<ContentId>(id.ToString());
                                                                         p.Add<CrowdfunderTypeId>(type.Id);
                                                                     });
    }
    
    private void QueueEmail(EntityChange<Checkout> checkout, Allocation allocation) {
        var crowdfunderData = allocation.GetCrowdfunderData(_jsonProvider);

        if (crowdfunderData.Type == CrowdfunderTypes.Fundraiser) {
            var fundraiser = _contentLocator.ById<FundraiserContent>(crowdfunderData.Id);
            var template = _contentLocator.ById<FundraiserContributionReceivedTemplateContent>(crowdfunderData.Id);

            var model = new FundraiserContributionReceivedViewModel(new FundraiserContentViewModel(fundraiser),
                                                                    checkout.SessionEntity,
                                                                    allocation,
                                                                    crowdfunderData);
                
            _emailBuilder.QueueTemplate(template, model.Fundraiser.FundraiserEmail, model);
        }
    }
}