namespace N3O.Umbraco.Giving.Checkout.Models;

public class CheckoutPledge : Value {
    public CheckoutPledge(string pledgeUrl) {
        PledgeUrl = pledgeUrl;
    }
    
    public string PledgeUrl { get; }
}