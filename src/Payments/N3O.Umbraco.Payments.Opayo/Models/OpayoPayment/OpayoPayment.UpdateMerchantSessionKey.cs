namespace N3O.Umbraco.Payments.Opayo.Models {
    public partial class OpayoPayment {
        public void UpdateMerchantSessionKey(string merchantSessionKey) {
            OpayoMerchantSessionKey = merchantSessionKey;
        }
    }
}