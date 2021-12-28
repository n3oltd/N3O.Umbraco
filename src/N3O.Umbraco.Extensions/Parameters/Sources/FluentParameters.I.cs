namespace N3O.Umbraco.Parameters;

public interface IFluentParameters : IParameterDataSource {
    public IFluentParameters Add<TNamedParameter>(TNamedParameter namedParameter)
        where TNamedParameter : INamedParameter;

    public IFluentParameters Add<TNamedParameter>(string value)
        where TNamedParameter : INamedParameter, new();

    public IFluentParameters Add(string name, string value);
}