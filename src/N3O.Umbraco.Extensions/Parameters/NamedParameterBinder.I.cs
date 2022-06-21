using System;

namespace N3O.Umbraco.Parameters;

public interface INamedParameterBinder {
    object Bind(Type namedParameterType);
}
