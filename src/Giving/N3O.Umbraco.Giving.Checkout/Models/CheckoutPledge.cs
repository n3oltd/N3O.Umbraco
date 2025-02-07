namespace N3O.Umbraco.Giving.Checkout.Models;

public class CheckoutPledge {
    public CheckoutPledge(string pledgeUrl) {
        PledgeUrl = pledgeUrl;
    }
    public string PledgeUrl { get; }
}