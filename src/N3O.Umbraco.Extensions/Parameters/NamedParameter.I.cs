namespace N3O.Umbraco.Parameters;

public interface INamedParameter<TValue> : INamedParameter {
    new TValue Value { get; set; }
}

public interface INamedParameter {
    string Name { get; }
    object Value { get; }
}

internal interface INamedParameterFromString {
    void FromString(string value);
}
