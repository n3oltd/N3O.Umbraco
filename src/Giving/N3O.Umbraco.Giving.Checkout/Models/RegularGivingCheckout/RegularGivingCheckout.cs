using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Payments.Models;
using NodaTime;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Giving.Checkout.Models {
    public class RegularGivingCheckout : Value {
        public RegularGivingCheckout(IEnumerable<Allocation> allocations,
                                     DayOfMonth collectionDay,
                                     Credential credential,
                                     LocalDate? firstCollectionDate,
                                     Money total) {
            Allocations = allocations.OrEmpty().ToList();
            CollectionDay = collectionDay;
            Credential = credential;
            FirstCollectionDate = firstCollectionDate;
            Total = total;
        }

        public IEnumerable<Allocation> Allocations { get; }
        public DayOfMonth CollectionDay { get; }
        public Credential Credential { get; }
        public LocalDate? FirstCollectionDate { get; }
        public Money Total { get; }
    }
}