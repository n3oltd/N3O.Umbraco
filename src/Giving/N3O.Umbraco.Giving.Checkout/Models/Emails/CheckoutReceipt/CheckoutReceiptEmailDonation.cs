using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Checkout.Models {
    public class CheckoutReceiptEmailDonation {
        public CheckoutReceiptEmailDonation(string title,
                                            string totalText,
                                            string paymentMethod,
                                            IReadOnlyList<CheckoutReceiptEmailAllocation> allocations) {
            Title = title;
            TotalText = totalText;
            PaymentMethod = paymentMethod;
            Allocations = allocations;
        }

        public string Title { get; }
        public string TotalText { get; }
        public string PaymentMethod { get; }
        public IReadOnlyList<CheckoutReceiptEmailAllocation> Allocations { get; }
    }
}