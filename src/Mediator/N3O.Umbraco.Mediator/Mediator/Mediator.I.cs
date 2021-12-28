using System;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Mediator {
    public interface IMediator {
        Task<None> SendAsync<TRequest, TModel>(TModel model, CancellationToken cancellationToken = default)
            where TRequest : Request<TModel, None>;

        Task<TResponse> SendAsync<TRequest, TModel, TResponse>(TModel model, CancellationToken cancellationToken = default)
            where TRequest : Request<TModel, TResponse>;

        Task SendAsync(Type requestType, Type responseType, object model, CancellationToken cancellationToken = default);
    }
}
