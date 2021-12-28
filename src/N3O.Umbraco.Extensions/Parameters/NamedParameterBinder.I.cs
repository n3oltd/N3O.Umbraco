using System;

namespace N3O.Umbraco.Parameters {
    internal interface INamedParameterBinder {
        object Bind(Type namedParameterType);
    }
}