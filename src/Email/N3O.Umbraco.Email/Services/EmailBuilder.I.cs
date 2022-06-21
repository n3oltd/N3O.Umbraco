namespace N3O.Umbraco.Email;

public interface IEmailBuilder {
    IFluentEmailBuilder<T> Create<T>();
}
