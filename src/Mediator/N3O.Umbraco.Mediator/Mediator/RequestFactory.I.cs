using System;

namespace N3O.Umbraco.Mediator;

public interface IRequestFactory {
    IModel Create(Type requestType);
    TRequest Create<TRequest, TModel, TResponse>(TModel model) where TRequest : Request<TModel, TResponse>;
}
