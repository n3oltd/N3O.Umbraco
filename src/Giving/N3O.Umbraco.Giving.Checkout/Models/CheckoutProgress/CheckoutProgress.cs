using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Checkout.Lookups;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Giving.Checkout.Models {
    public class CheckoutProgress : Value {
        [JsonConstructor]
        public CheckoutProgress(CheckoutStage current,
                                IEnumerable<CheckoutStage> required,
                                IEnumerable<CheckoutStage> remaining) {
            Current = current;
            Required = required.OrEmpty().ToList();
            Remaining = remaining.OrEmpty().ToList();
        }

        public CheckoutProgress(IEnumerable<CheckoutStage> required) {
            var requiredList = required.OrEmpty().OrderBy(x => x.Order).ToList();

            if (requiredList.None()) {
                throw new Exception("At least one stage is required");
            }
        
            Current = requiredList.First();
            Required = requiredList;
            Remaining = new List<CheckoutStage>(requiredList);
        }

        public CheckoutStage Current { get; }
        public IEnumerable<CheckoutStage> Required { get; }
        public IEnumerable<CheckoutStage> Remaining { get; }

        public CheckoutProgress Completed(CheckoutStage stage) {
            if (Required.Contains(stage)) {
                throw new Exception($"{stage} is not a required stage");
            }
            
            var remaining = Remaining.Except(stage).ToList();
            var current = Remaining.FirstOrDefault();
            
            return new CheckoutProgress(current, Required, remaining);
        }
    }
}