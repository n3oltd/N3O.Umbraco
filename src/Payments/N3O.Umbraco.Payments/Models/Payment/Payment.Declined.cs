namespace N3O.Umbraco.Payments.Models {
    public partial class Payment {
        public void Declined(string declineReason) {
            DeclineReason = declineReason;
            IsDeclined = true;
        }
    }
}