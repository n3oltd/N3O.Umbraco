using Microsoft.Extensions.Logging;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Entities;
using N3O.Umbraco.Giving.Checkout.Entities;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Models;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.ChangeFeeds;

public class CheckoutChangeFeed : ChangeFeed<Checkout> {
    private readonly ICrowdfundingWriter _crowdfundingQ;
    private readonly IClock _clock;
    
    public CheckoutChangeFeed(ILogger<CheckoutChangeFeed> logger,
                              ICrowdfundingWriter crowdfundingQ,
                              IClock clock) : base(logger) {
        _crowdfundingQ = crowdfundingQ;
        _clock = clock;
    }
    
    protected override Task ProcessChangeAsync(EntityChange<Checkout> entityChange) {
        if (entityChange.Operation == EntityOperations.Update) {
            if (entityChange.SessionEntity.Donation.Allocations.Any(x => x.HasCrowdfundingData())) {
                Process(entityChange, x => x.Donation.IsComplete, async c => {
                    await CommitAsync(GivingTypes.Donation, c);
                });
            }
            
            if (entityChange.SessionEntity.RegularGiving.Allocations.Any(x => x.HasCrowdfundingData())) {
                Process(entityChange, x => x.RegularGiving.IsComplete, async c => {
                    await CommitAsync(GivingTypes.RegularGiving, c);
                });
            }
        }
        
        return Task.CompletedTask;
    }

    private async Task CommitAsync(GivingType givingType, Checkout checkout) {
        var crowdfundingAllocations = GetCrowdfundingAllocations(givingType, checkout);
        
        foreach (var crowdfundingAllocation in crowdfundingAllocations) {
            var crowdfundingData = crowdfundingAllocation.GetCrowdfundingData();

            await _crowdfundingQ.AppendAsync(checkout.Reference.Text,
                                             _clock.GetCurrentInstant(),
                                             crowdfundingData.CampaignId,
                                             crowdfundingData.TeamId,
                                             crowdfundingData.TeamName,
                                             crowdfundingData.PageId,
                                             crowdfundingData.Anonymous,
                                             crowdfundingData.PageUrl,
                                             crowdfundingData.Comment,
                                             crowdfundingAllocation.Value,
                                             checkout.Account?.Email?.Address);
        }

        await _crowdfundingQ.CommitAsync();
    }

    private IReadOnlyList<Allocation> GetCrowdfundingAllocations(GivingType givingType, Checkout checkout) {
        var crowdfundingAllocations = new List<Allocation>();

        if (givingType == GivingTypes.Donation) {
            crowdfundingAllocations = checkout.Donation.Allocations.Where(x => x.HasCrowdfundingData()).ToList();
        } else if (givingType == GivingTypes.RegularGiving) {
            crowdfundingAllocations = checkout.RegularGiving.Allocations.Where(x => x.HasCrowdfundingData()).ToList();
        }

        return crowdfundingAllocations;
    }

    private void Process(EntityChange<Checkout> entityChange,
                         Func<Checkout, bool> getComplete,
                         Action<Checkout> action) {
        var isComplete = getComplete(entityChange.SessionEntity);
        var wasComplete = getComplete(entityChange.DatabaseEntity);

        if (isComplete && !wasComplete) {
            var checkout = entityChange.SessionEntity;
            
            action(checkout);
        }
    }
}