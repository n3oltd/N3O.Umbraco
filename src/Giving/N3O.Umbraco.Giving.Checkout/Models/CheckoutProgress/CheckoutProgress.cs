using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Checkout.Lookups;
using N3O.Umbraco.Lookups;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Giving.Checkout.Models;

public class CheckoutProgress : Value {
    [JsonConstructor]
    public CheckoutProgress(CheckoutStage currentStage,
                            IEnumerable<CheckoutStage> requiredStages,
                            IEnumerable<CheckoutStage> remainingStages) {
        CurrentStage = currentStage;
        RequiredStages = requiredStages.OrEmpty().ToList();
        RemainingStages = remainingStages.OrEmpty().ToList();
    }

    public CheckoutProgress(ILookups lookups, Entities.Checkout checkout) {
        var allStages = lookups.GetAll<CheckoutStage>();
        var requiredStages = allStages.Where(x => x.IsRequired(checkout)).OrderBy(x => x.Order).ToList();

        CurrentStage = requiredStages.First();
        RequiredStages = requiredStages;
        RemainingStages = new List<CheckoutStage>(requiredStages);
    }

    public CheckoutStage CurrentStage { get; }
    public IEnumerable<CheckoutStage> RequiredStages { get; }
    public IEnumerable<CheckoutStage> RemainingStages { get; }

    public CheckoutProgress NextStage() {
        var remaining = RemainingStages.Except(CurrentStage).ToList();
        var current = remaining.FirstOrDefault();
        
        return new CheckoutProgress(current, RequiredStages, remaining);
    }
}
