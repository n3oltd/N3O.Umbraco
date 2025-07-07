using System.Collections.Generic;

namespace N3O.Umbraco.Logging;

public abstract class LogEnricher : ILogEnricher {
    private static readonly IReadOnlyDictionary<string, string> Empty = new Dictionary<string, string>();

    public virtual IReadOnlyDictionary<string, string> GetContextData() => Empty;
    
    public virtual IReadOnlyDictionary<string, string> GetTags() => Empty;
}