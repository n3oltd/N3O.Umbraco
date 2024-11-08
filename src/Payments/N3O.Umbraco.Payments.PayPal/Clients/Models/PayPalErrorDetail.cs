namespace N3O.Umbraco.Payments.PayPal.Clients.Models;

public class PayPalErrorDetail {
    public string Field { get; set; }
    public string Value { get; set; }
    public string Location { get; set; }
    public string Issue { get; set; }
    public string Description { get; set; }
}