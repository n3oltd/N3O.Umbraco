using Microsoft.Extensions.DependencyInjection;
using System;

namespace N3O.Umbraco.Mediator {
    public class RequestFactory : IRequestFactory {
        private readonly IServiceProvider _serviceProvider;

        public RequestFactory(IServiceProvider serviceProvider) {
            _serviceProvider = serviceProvider;
        }

        public IModel Create(Type requestType) {
            var request = (IModel) _serviceProvider.GetRequiredService(requestType);

            return request;
        }

        public TRequest Create<TRequest, TModel, TResponse>(TModel model) where TRequest : Request<TModel, TResponse> {
            var request = (TRequest) Create(typeof(TRequest));

            request.Model = model;

            return request;
        }
    }
}