namespace N3O.Umbraco.Payments.Opayo.Models;

public partial class OpayoPayment {
    public void UpdateKeyAndVendorTxCode(string merchantSessionKey, string vendorTxCode) {
        OpayoMerchantSessionKey = merchantSessionKey;
        VendorTxCode = vendorTxCode;
    }
}
