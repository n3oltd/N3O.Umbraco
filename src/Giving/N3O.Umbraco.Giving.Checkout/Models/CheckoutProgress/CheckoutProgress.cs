using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Checkout.Lookups;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Utilities;
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

    public CheckoutProgress(Entities.Checkout checkout) {
        var requiredStages = GetRequiredStages(checkout);

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

    private IEnumerable<CheckoutStage> GetRequiredStages(Entities.Checkout checkout) {
        var requiredStages = new List<CheckoutStage>();

        var additionalImplementation = OurAssemblies.GetTypes(t => t.IsConcreteClass() &&
                                                                   t.IsSubclassOf(typeof(StaticLookupsCollection<CheckoutStage>)) &&
                                                                   t.Namespace?.Equals(typeof(CheckoutStages).Namespace) == false)
                                                    .SingleOrDefault();

        if (additionalImplementation.HasValue()) {
            var additionalProperties = additionalImplementation.GetFields()
                                                               .Select(x => (CheckoutStage) x.GetValue(null))
                                                               .ToList();

            if (!checkout.Donation.IsRequired) {
                additionalProperties.Remove(additionalProperties.FirstOrDefault(x => x.Id == CheckoutStages.Donation.Id));
            }

            if (!checkout.RegularGiving.IsRequired) {
                additionalProperties.Remove(additionalProperties.FirstOrDefault(x => x.Id == CheckoutStages.RegularGiving.Id));
            }

            requiredStages.AddRange(additionalProperties);
        } else {
            requiredStages.Add(CheckoutStages.Account);

            if (checkout.Donation.IsRequired) {
                requiredStages.Add(CheckoutStages.Donation);
            }

            if (checkout.RegularGiving.IsRequired) {
                requiredStages.Add(CheckoutStages.RegularGiving);
            }
        }

        return requiredStages.OrderBy(x => x.Order).ToList();
    }
}
