using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Allocations.Lookups;

namespace N3O.Umbraco.Giving.Checkout.Entities;

public partial class Checkout {
    public Money GetValue() {
        var donationAmount = (Donation?.Total.Amount ?? 0m) * GivingTypes.Donation.ValueMultiplier;
        var regularGivingAmount = (RegularGiving?.Total.Amount ?? 0m) * GivingTypes.RegularGiving.ValueMultiplier;
        var totalAmount = donationAmount + regularGivingAmount;

        return new Money(totalAmount, Currency);
    }
}
