namespace N3O.Umbraco.Mediator {
    public interface IModel<TModel> : IModel {
        new TModel Model { get; set; }
    }

    public interface IModel {
        object Model { get; set; }
    }
}