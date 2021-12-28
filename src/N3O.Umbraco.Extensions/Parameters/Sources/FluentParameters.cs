using System.Collections.Generic;

namespace N3O.Umbraco.Parameters {
    public class FluentParameters : IFluentParameters {
        private readonly Dictionary<string, string> _parameters = new();

        public IFluentParameters Add<TNamedParameter>(TNamedParameter namedParameter)
            where TNamedParameter : INamedParameter {
            return Add(namedParameter.Name, namedParameter.ToString());
        }

        public IFluentParameters Add<TNamedParameter>(string value)
            where TNamedParameter : INamedParameter, new() {
            var namedParameter = NamedParameter.Create(typeof(TNamedParameter));

            return Add(namedParameter.Name, value);
        }

        public IFluentParameters Add(string name, string value) {
            _parameters[name] = value;

            return this;
        }

        public IReadOnlyDictionary<string, string> GetData() {
            return _parameters;
        }

        public long Order => 1;
    }
}