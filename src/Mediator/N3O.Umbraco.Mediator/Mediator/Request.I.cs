namespace N3O.Umbraco.Mediator;

public interface IRequest<TModel, out TResponse> :
    IModel<TModel>,
    IResponse<TResponse>,
    MediatR.IRequest<TResponse> {
    new TModel Model { get; set; }
}
