namespace N3O.Umbraco.Webhooks.Models;

public class DispatchWebhookReq {
    public string Url { get; set; }
    public object Body { get; set; }
}
