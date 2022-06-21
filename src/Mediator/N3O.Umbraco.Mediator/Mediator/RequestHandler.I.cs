namespace N3O.Umbraco.Mediator;

public interface IRequestHandler<in TRequest, TModel, TResponse> : MediatR.IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TModel, TResponse> { }
