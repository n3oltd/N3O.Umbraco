using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Utilities;
using N3O.Umbraco.Webhooks.Attributes;
using N3O.Umbraco.Webhooks.Commands;
using N3O.Umbraco.Webhooks.Models;
using N3O.Umbraco.Webhooks.Receivers;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Extensions;

namespace N3O.Umbraco.Webhooks.Handlers;

public class ProcessWebhookHandler : IRequestHandler<ProcessWebhookCommand, WebhookPayload, None> {
    private static readonly Dictionary<string, Type> Receivers = new(StringComparer.InvariantCultureIgnoreCase);
    
    private readonly IServiceProvider _serviceProvider;
    
    static ProcessWebhookHandler() {
        var receiverTypes = OurAssemblies.GetTypes(t => t.IsConcreteClass() &&
                                                        t.ImplementsInterface<IWebhookReceiver>() &&
                                                        t.HasAttribute<WebhookReceiverAttribute>());

        foreach (var receiverType in receiverTypes) {
            var attributes = receiverType.GetCustomAttributes<WebhookReceiverAttribute>(false);

            foreach (var attribute in attributes) {
                Receivers[attribute.HookId] = receiverType;
            }
        }
    }

    public ProcessWebhookHandler(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
    }
    
    public async Task<None> Handle(ProcessWebhookCommand req, CancellationToken cancellationToken) {
        var receiverType = Receivers.GetOrDefault(req.Model.HookId);

        if (receiverType == null) {
            throw new Exception($"No receiver found for hook ID {req.Model.HookId.Quote()}");
        }

        var receiver = (IWebhookReceiver) _serviceProvider.GetRequiredService(receiverType);

        await receiver.HandleAsync(req.Model, cancellationToken);
        
        return None.Empty;
    }
}
