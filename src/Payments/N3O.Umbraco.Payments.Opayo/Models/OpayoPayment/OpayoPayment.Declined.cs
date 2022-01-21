namespace N3O.Umbraco.Payments.Opayo.Models {
    public partial class OpayoPayment {
        public void Declined(string declineReason) {
            DeclineReason = declineReason;
            IsDeclined = true;
        }
    }
}