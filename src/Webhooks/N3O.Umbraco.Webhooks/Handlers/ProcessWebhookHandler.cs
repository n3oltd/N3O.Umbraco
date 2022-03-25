using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Utilities;
using N3O.Umbraco.Webhooks.Attributes;
using N3O.Umbraco.Webhooks.Commands;
using N3O.Umbraco.Webhooks.Endpoints;
using N3O.Umbraco.Webhooks.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Extensions;

namespace N3O.Umbraco.Webhooks.Handlers {
    public class ProcessWebhookHandler : IRequestHandler<ProcessWebhookCommand, ReceivedWebhook, None> {
        private static readonly Dictionary<string, Type> Endpoints = new(StringComparer.InvariantCultureIgnoreCase);
        
        private readonly IServiceProvider _serviceProvider;
        
        static ProcessWebhookHandler() {
            var endpointTypes = OurAssemblies.GetTypes(t => t.IsConcreteClass() &&
                                                            t.ImplementsInterface<IWebhookEndpoint>() &&
                                                            t.HasAttribute<WebhookEndpointAttribute>());

            foreach (var endpointType in endpointTypes) {
                Endpoints[endpointType.GetCustomAttribute<WebhookEndpointAttribute>(true).EndpointId] = endpointType;
            }
        }

        public ProcessWebhookHandler(IServiceProvider serviceProvider) {
            _serviceProvider = serviceProvider;
        }
        
        public async Task<None> Handle(ProcessWebhookCommand req, CancellationToken cancellationToken) {
            var endpointType = Endpoints.GetOrDefault(req.Model.EndpointId);

            if (endpointType == null) {
                throw new Exception($"No webhook endpoint found for ID {req.Model.EndpointId.Quote()}");
            }

            var endpoint = (IWebhookEndpoint) _serviceProvider.GetRequiredService(endpointType);

            await endpoint.HandleAsync(req.Model, cancellationToken);
            
            return None.Empty;
        }
    }
}
