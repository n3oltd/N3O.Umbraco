namespace N3O.Umbraco.Giving.Webhooks {
    public interface IWebhookTransform {
        object Apply(object body);
        bool IsTransform(object body);
    }
}