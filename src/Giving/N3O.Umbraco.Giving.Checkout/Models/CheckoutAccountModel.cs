using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.TaxRelief.Lookups;

namespace N3O.Umbraco.Giving.Checkout.Models {
    public class CheckoutAccountModel {
        public CheckoutAccountModel(DataEntrySettings dataEntrySettings, TaxReliefScheme taxReliefScheme) {
            DataEntrySettings = dataEntrySettings;
            TaxReliefScheme = taxReliefScheme;
        }

        public DataEntrySettings DataEntrySettings { get; }
        public TaxReliefScheme TaxReliefScheme { get; }
    }
}