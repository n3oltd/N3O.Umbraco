using Microsoft.Extensions.Logging;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Entities;
using N3O.Umbraco.Giving.Checkout.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.ChangeFeeds;

public class CheckoutChangeFeed : ChangeFeed<Checkout> {
    public CheckoutChangeFeed(ILogger<CheckoutChangeFeed> logger) : base(logger) { }
    
    protected override Task ProcessChangeAsync(EntityChange<Checkout> entityChange) {
        if (entityChange.SessionEntity.Donation.Allocations.Any(x => x.HasCrowdfundingData())) {
            if (entityChange.Operation == EntityOperations.Update) {
                Process(entityChange, x => x.Donation.IsComplete, x => {
                            
                });
            }
        }
        // TODO If the checkout is newly completed, we will see if any of the allocation lines
        // are for crowdfunding, and if they are we will extract those lines and insert them into the 
        // SQL table.

        return Task.CompletedTask;
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