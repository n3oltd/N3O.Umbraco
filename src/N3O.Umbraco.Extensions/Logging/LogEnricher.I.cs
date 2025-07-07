using System.Collections.Generic;

namespace N3O.Umbraco.Logging;

public interface ILogEnricher {
    IReadOnlyDictionary<string, string> GetContextData();
    IReadOnlyDictionary<string, string> GetTags();
}