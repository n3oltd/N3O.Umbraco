using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Models;
using N3O.Umbraco.Payments.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Giving.Checkout.Models {
    public class RegularGivingCheckout : Value {
        [JsonConstructor]
        public RegularGivingCheckout(IEnumerable<Allocation> allocations,
                                     Credential credential,
                                     RegularGivingOptions options,
                                     Money total) {
            Allocations = allocations.OrEmpty().ToList();
            Credential = credential;
            Options = options;
            Total = total;
        }

        public RegularGivingCheckout(IEnumerable<Allocation> allocations, Currency currency)
            : this(allocations.OrEmpty(),
                   null,
                   null,
                   allocations.HasAny() ? allocations.Select(x => x.Value).Sum() : currency.Zero()) { }

        public IEnumerable<Allocation> Allocations { get; }
        public Credential Credential { get; }
        public RegularGivingOptions Options { get; }
        public Money Total { get; }

        [JsonIgnore]
        public bool IsComplete => IsRequired &&
                                  Options.HasValue() &&
                                  Credential?.IsSetUp == true;

        [JsonIgnore]
        public bool IsRequired => Allocations.HasAny();

        public RegularGivingCheckout UpdateOptions(IRegularGivingOptions options) {
            return new RegularGivingCheckout(Allocations, Credential, new RegularGivingOptions(options), Total);
        }

        public RegularGivingCheckout UpdateCredential(Credential credential) {
            return new RegularGivingCheckout(Allocations, credential, Options, Total);
        }
    }
}