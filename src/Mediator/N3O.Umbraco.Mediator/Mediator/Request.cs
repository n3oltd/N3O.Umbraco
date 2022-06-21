namespace N3O.Umbraco.Mediator;

public abstract class Request<TModel, TResponse> : IRequest<TModel, TResponse> {
    public TModel Model { get; set; }

    object IModel.Model {
        get => Model;

        set => Model = (TModel) value;
    }
}
