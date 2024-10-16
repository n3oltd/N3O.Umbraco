﻿using Microsoft.Extensions.Logging;
using N3O.Umbraco.Accounts.Extensions;
using N3O.Umbraco.Crowdfunding.Extensions;
using N3O.Umbraco.Entities;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Checkout.Entities;
using N3O.Umbraco.Giving.Extensions;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Models;
using N3O.Umbraco.Json;
using N3O.Umbraco.Localization;
using N3O.Umbraco.TaxRelief.Lookups;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.ChangeFeeds;

public class CheckoutChangeFeed : ChangeFeed<Checkout> {
    private readonly IClock _clock;
    private readonly IFormatter _formatter;
    private readonly IContributionRepository _contributionRepository;
    private readonly IJsonProvider _jsonProvider;
    private readonly ICrowdfunderRepository _crowdfunderRepository;
    
    public CheckoutChangeFeed(ILogger<CheckoutChangeFeed> logger,
                              IClock clock,
                              IFormatter formatter,
                              IContributionRepository contributionRepository,
                              IJsonProvider jsonProvider,
                              ICrowdfunderRepository crowdfunderRepository) 
        : base(logger) {
        _contributionRepository = contributionRepository;
        _jsonProvider = jsonProvider;
        _crowdfunderRepository = crowdfunderRepository;
        _clock = clock;
        _formatter = formatter;
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

            foreach (var allocation in allocations.Where(x => x.HasExtensionDataFor(CrowdfundingConstants.Allocations.Extensions.Key))) {
                await CommitAsync(givingType, checkout.SessionEntity, allocation);
            }
            
            RefreshCrowdfunderContributions(allocations);
        }
    }

    private async Task CommitAsync(GivingType givingType, Checkout checkout, Allocation allocation) {
        var crowdfunderData = allocation.GetCrowdfunderData(_jsonProvider);

        await _contributionRepository.AddOnlineContributionAsync(checkout.Reference.Text,
                                                                 _clock.GetCurrentInstant(),
                                                                 crowdfunderData,
                                                                 checkout.Account?.Email?.Address,
                                                                 checkout.Account?.GetName(_formatter),
                                                                 checkout.Account?.TaxStatus == TaxStatuses.Payer,
                                                                 givingType,
                                                                 allocation);

        await _contributionRepository.CommitAsync();
    }

    private void RefreshCrowdfunderContributions(IEnumerable<Allocation> allocations) {
        var crowdfunders = allocations.Where(x => x.HasCrowdfunderData())
                                      .Select(x => x.GetCrowdfunderData(_jsonProvider))
                                      .GroupBy(x => (x.Id, x.Type));
            
        crowdfunders.Do(x => _crowdfunderRepository.QueueRecalculateContributionsTotal(x.Key.Id, x.Key.Type));
    }
}