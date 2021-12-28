using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Mediator.Extensions;

public static class MediatorExtensions {
    public static Task<None> SendAsync<TRequest, TModel>(this IMediator mediator,
                                                         TModel model,
                                                         CancellationToken cancellationToken = default)
        where TRequest : Request<TModel, None> {
        return mediator.SendAsync<TRequest, TModel, None>(model, cancellationToken);
    }

    public static Task<None> SendAsync<TRequest>(this IMediator mediator, CancellationToken cancellationToken = default)
        where TRequest : Request<None, None> {
        return mediator.SendAsync<TRequest, None, None>(None.Empty, cancellationToken);
    }
}