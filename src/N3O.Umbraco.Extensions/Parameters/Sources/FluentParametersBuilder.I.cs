using System.Collections.Generic;

namespace N3O.Umbraco.Parameters {
    public interface IFluentParametersBuilder {
        public IFluentParametersBuilder Add<TNamedParameter>(TNamedParameter namedParameter)
            where TNamedParameter : INamedParameter;

        public IFluentParametersBuilder Add<TNamedParameter>(string value)
            where TNamedParameter : INamedParameter, new();

        public IFluentParametersBuilder Add(string name, string value);

        IReadOnlyDictionary<string, string> Build();
    }
}