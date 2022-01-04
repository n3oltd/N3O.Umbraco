using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Parameters {
    public class NamedParameterBinder : INamedParameterBinder {
        private readonly IEnumerable<IParameterDataSource> _parameterDataSources;

        public NamedParameterBinder(IEnumerable<IParameterDataSource> parameterDataSources) {
            _parameterDataSources = parameterDataSources.OrderByDescending(x => x.Order).ToList();
        }

        public object Bind(Type namedParameterType) {
            var namedParameter = NamedParameter.Create(namedParameterType);

            foreach (var parameterDataSource in _parameterDataSources) {
                var parameterData = parameterDataSource.GetData();
                var parameterEntry = parameterData.FirstOrDefault(x => x.Key.EqualsInvariant(namedParameter.Name));

                if (parameterEntry.HasValue()) {
                    ((INamedParameterFromString) namedParameter).FromString(parameterEntry.Value);

                    break;
                }
            }

            return namedParameter;
        }
    }
}