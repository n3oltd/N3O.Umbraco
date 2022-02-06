using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Checkout.Lookups;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Giving.Checkout.Models {
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
            var requiredStages = new List<CheckoutStage>();
            
            requiredStages.Add(CheckoutStages.Account);
            
            if (checkout.Donation.IsRequired) {
                requiredStages.Add(CheckoutStages.Donation);
            }
            
            if (checkout.RegularGiving.IsRequired) {
                requiredStages.Add(CheckoutStages.RegularGiving);
            }
            
            requiredStages = requiredStages.OrderBy(x => x.Order).ToList();

            CurrentStage = requiredStages.First();
            RequiredStages = requiredStages;
            RemainingStages = new List<CheckoutStage>(requiredStages);
        }

        public CheckoutStage CurrentStage { get; }
        public IEnumerable<CheckoutStage> RequiredStages { get; }
        public IEnumerable<CheckoutStage> RemainingStages { get; }

        public CheckoutProgress NextStage() {
            var remaining = RemainingStages.Except(CurrentStage).ToList();
            var current = RemainingStages.FirstOrDefault();
            
            return new CheckoutProgress(current, RequiredStages, remaining);
        }
    }
}