namespace N3O.Umbraco.Webhooks.Transforms;

public interface IWebhookTransform {
    object Apply(object body);
    bool IsTransform(object body);
}
