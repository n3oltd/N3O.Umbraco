using Newtonsoft.Json.Serialization;
using System.Text.RegularExpressions;

namespace N3O.Umbraco.Forex.Currencylayer;

public class SnakeCasePropertyNamesContractResolver : DefaultContractResolver {
    private readonly Regex _converter = new Regex(@"((?<=[a-z])(?<b>[A-Z])|(?<=[^_])(?<b>[A-Z][a-z]))");

    protected override string ResolvePropertyName(string propertyName) {
        return _converter.Replace(propertyName, "_${b}").ToLowerInvariant();
    }
}
