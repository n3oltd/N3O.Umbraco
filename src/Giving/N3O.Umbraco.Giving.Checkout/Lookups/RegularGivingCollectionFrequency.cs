using N3O.Umbraco.Lookups;
using NodaTime;

namespace N3O.Umbraco.Giving.Checkout.Lookups {
    public class RegularGivingFrequency : NamedLookup {
        public RegularGivingFrequency(string id, string name, int months) : base(id, name) {
            Months = months;
        }
        
        public int Months { get; }

        public LocalDate GetNextDueDate(LocalDate lastDueDate) {
            return lastDueDate.Plus(Period.FromMonths(Months));
        }
    }
    
    public class RegularGivingFrequencies : StaticLookupsCollection<RegularGivingFrequency> {
        public static RegularGivingFrequency Annually = new("annually", "Annually", 12);
        public static RegularGivingFrequency Monthly = new("monthly", "Monthly", 1);
        public static RegularGivingFrequency Quarterly = new("quarterly", "Quarterly", 3);
    }
}