using N3O.Umbraco.Financial;

namespace N3O.Umbraco.Giving.Checkout.Entities {
    public partial class Checkout {
        public Money GetValue(int regularGivingMultiplier) {
            var donationAmount = Donation?.Total.Amount ?? 0m;
            var regularGivingAmount = (RegularGiving?.Total.Amount ?? 0m) * regularGivingMultiplier;
            var totalAmount = donationAmount + regularGivingAmount;

            return new Money(totalAmount, Currency);
        }
    }
}