using MediatR;
using N3O.Umbraco.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Mediator {
    public class Mediator : IMediator {
        private readonly MediatR.IMediator _mediatr;
        private readonly IRequestFactory _requestFactory;

        public Mediator(MediatR.IMediator mediatr, IRequestFactory requestFactory) {
            _mediatr = mediatr;
            _requestFactory = requestFactory;
        }

        public Task<None> SendAsync<TRequest, TModel>(TModel model, CancellationToken cancellationToken = default)
            where TRequest : Request<TModel, None> {
            var request = _requestFactory.Create<TRequest, TModel, None>(model);

            return _mediatr.Send(request, cancellationToken);
        }

        public Task<TResponse> SendAsync<TRequest, TModel, TResponse>(TModel model,
                                                                      CancellationToken cancellationToken = default)
            where TRequest : Request<TModel, TResponse> {
            var request = _requestFactory.Create<TRequest, TModel, TResponse>(model);

            return _mediatr.Send(request, cancellationToken);
        }

        public async Task<object> SendAsync(Type requestType,
                                            Type responseType,
                                            object model,
                                            CancellationToken cancellationToken = default) {
            var request = _requestFactory.Create(requestType);

            request.Model = model;

            var interfaceType = typeof(IRequest<>).MakeGenericType(responseType);

            var task = _mediatr.CallMethod(nameof(MediatR.IMediator.Send))
                               .OfGenericType(responseType)
                               .WithParameter(interfaceType, request)
                               .WithParameter(typeof(CancellationToken), cancellationToken)
                               .RunAsync();

            await task;
            
            return task.GetResult();
        }
    }
}