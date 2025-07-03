using N3O.Umbraco.Extensions;
using Newtonsoft.Json;
using NodaTime;
using System.Collections.Generic;
using System.Globalization;

namespace N3O.Umbraco.Utilities;

public class CacheKeyBuilder {
    private readonly List<string> _values = [];

    private CacheKeyBuilder() { }
    
    public CacheKeyBuilder Append(string value) {
        if (value.HasValue()) {
            _values.Add(value.Trim().ToLowerInvariant());
        }

        return this;
    }
    
    public CacheKeyBuilder Append(int? value) {
        return Append(value?.ToString(CultureInfo.InvariantCulture));
    }

    public CacheKeyBuilder Append(long? value) {
        return Append(value?.ToString(CultureInfo.InvariantCulture));
    }
    
    public CacheKeyBuilder Append(float? value) {
        return Append(value?.ToString(CultureInfo.InvariantCulture));
    }
    
    public CacheKeyBuilder Append(decimal? value) {
        return Append(value?.ToString(CultureInfo.InvariantCulture));
    }
    
    public CacheKeyBuilder Append(bool? value) {
        return Append(value?.ToString(CultureInfo.InvariantCulture));
    }

    public CacheKeyBuilder Append(LocalDate? value) {
        return Append(value?.ToYearMonthDayString());
    }
    
    public CacheKeyBuilder Append(object obj) {
        return Append(obj.IfNotNull(JsonConvert.SerializeObject));
    }

    public string Build() {
        return _values.Join("_").Sha1();
    }
    
    public static CacheKeyBuilder Create() {
        return new CacheKeyBuilder();
    }
}