using Microsoft.Extensions.Logging;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Entities;
using N3O.Umbraco.Giving.Checkout.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.ChangeFeeds;

public class CheckoutChangeFeed : ChangeFeed<Checkout> {
    public CheckoutChangeFeed(ILogger<CheckoutChangeFeed> logger) : base(logger) { }
    
    protected override Task ProcessChangeAsync(EntityChange<Checkout> entityChange) {
        if (entityChange.SessionEntity.Donation.Allocations.Any(x => x.HasCrowdfundingData())) {
            
        }
        // TODO If the checkout is newly completed, we will see if any of the allocation lines
        // are for crowdfunding, and if they are we will extract those lines and insert them into the 
        // SQL table.

        return Task.CompletedTask;
    }
}