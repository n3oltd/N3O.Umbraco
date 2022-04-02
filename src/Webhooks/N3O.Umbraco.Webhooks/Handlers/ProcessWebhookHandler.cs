using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Utilities;
using N3O.Umbraco.Webhooks.Attributes;
using N3O.Umbraco.Webhooks.Commands;
using N3O.Umbraco.Webhooks.Receivers;
using N3O.Umbraco.Webhooks.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Extensions;

namespace N3O.Umbraco.Webhooks.Handlers {
    public class ProcessWebhookHandler : IRequestHandler<ProcessWebhookCommand, Payload, None> {
        private static readonly Dictionary<string, Type> Receivers = new(StringComparer.InvariantCultureIgnoreCase);
        
        private readonly IServiceProvider _serviceProvider;
        
        static ProcessWebhookHandler() {
            var receiverTypes = OurAssemblies.GetTypes(t => t.IsConcreteClass() &&
                                                            t.ImplementsInterface<IWebhookReceiver>() &&
                                                            t.HasAttribute<WebhookReceiverAttribute>());

            foreach (var receiverType in receiverTypes) {
                Receivers[receiverType.GetCustomAttribute<WebhookReceiverAttribute>(true).EventId] = receiverType;
            }
        }

        public ProcessWebhookHandler(IServiceProvider serviceProvider) {
            _serviceProvider = serviceProvider;
        }
        
        public async Task<None> Handle(ProcessWebhookCommand req, CancellationToken cancellationToken) {
            var receiverType = Receivers.GetOrDefault(req.Model.EventId);

            if (receiverType == null) {
                throw new Exception($"No webhook receiver found for ID {req.Model.EventId.Quote()}");
            }

            var receiver = (IWebhookReceiver) _serviceProvider.GetRequiredService(receiverType);

            await receiver.HandleAsync(req.Model, cancellationToken);
            
            return None.Empty;
        }
    }
}
