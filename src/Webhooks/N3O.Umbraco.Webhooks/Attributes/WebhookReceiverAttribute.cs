using System;

namespace N3O.Umbraco.Webhooks.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public class WebhookReceiverAttribute : Attribute {
    public WebhookReceiverAttribute(string hookId) {
        HookId = hookId;
    }

    public string HookId { get; }
}
