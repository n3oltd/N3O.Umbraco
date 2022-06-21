using System.Collections.Generic;

namespace N3O.Umbraco.Parameters;

public interface IParameterDataSource {
    long Order { get; }

    IReadOnlyDictionary<string, string> GetData();
}
