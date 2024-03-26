using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Utilities;

public class CacheKeyBuilder {
    private readonly List<string> _values = new();

    private CacheKeyBuilder() { }
    
    public CacheKeyBuilder Append(string s) {
        _values.Add(s);

        return this;
    }

    public string Build() {
        return _values.ExceptNull().Select(x => x.Trim()).Join("_").Sha1();
    }
    
    public static CacheKeyBuilder Create() {
        return new CacheKeyBuilder();
    }
}