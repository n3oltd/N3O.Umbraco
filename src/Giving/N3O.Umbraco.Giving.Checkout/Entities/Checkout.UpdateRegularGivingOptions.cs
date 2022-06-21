using N3O.Umbraco.Giving.Checkout.Models;

namespace N3O.Umbraco.Giving.Checkout.Entities;

public partial class Checkout {
    public void UpdateRegularGivingOptions(IRegularGivingOptions options) {
        RegularGiving = RegularGiving.UpdateOptions(options);
    }
}
