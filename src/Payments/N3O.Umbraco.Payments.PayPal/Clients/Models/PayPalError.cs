using System.Collections.Generic;

namespace N3O.Umbraco.Payments.PayPal.Clients.Models;

public class PayPalError {
    public string Name { get; set; }
    public string Message { get; set; }
    public string DebugId { get; set; }
    public List<PayPalErrorDetail> Details { get; set; }
    public List<Link> Links { get; set; }
}