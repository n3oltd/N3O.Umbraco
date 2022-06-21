using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Parameters;

public class FluentParametersBuilder : IFluentParametersBuilder {
    private readonly Dictionary<string, string> _parameters = new(StringComparer.InvariantCultureIgnoreCase);

    public IFluentParametersBuilder Add<TNamedParameter>(TNamedParameter namedParameter)
        where TNamedParameter : INamedParameter {
        return Add(namedParameter.Name, namedParameter.ToString());
    }

    public IFluentParametersBuilder Add<TNamedParameter>(string value)
        where TNamedParameter : INamedParameter, new() {
        var namedParameter = NamedParameter.Create(typeof(TNamedParameter));

        return Add(namedParameter.Name, value);
    }

    public IFluentParametersBuilder Add(string name, string value) {
        _parameters[name] = value;

        return this;
    }

    public IReadOnlyDictionary<string, string> Build() {
        return _parameters;
    }
}
