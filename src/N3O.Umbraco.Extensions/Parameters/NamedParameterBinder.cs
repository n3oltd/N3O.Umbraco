using System;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Parameters;

public class NamedParameterBinder : INamedParameterBinder {
    private readonly IEnumerable<IParameterDataSource> _parameterDataSources;

    public NamedParameterBinder(IEnumerable<IParameterDataSource> parameterDataSources) {
        _parameterDataSources = parameterDataSources.OrderByDescending(x => x.Order).ToList();
    }

    public object Bind(Type namedParameterType) {
        var namedParameter = NamedParameter.Create(namedParameterType);

        foreach (var parameterDataSource in _parameterDataSources) {
            var parameterData = parameterDataSource.GetData();

            if (parameterData.ContainsKey(namedParameter.Name)) {
                ((INamedParameterFromString) namedParameter).FromString(parameterData[namedParameter.Name]);
                
                break;
            }
        }

        return namedParameter;
    }
}
