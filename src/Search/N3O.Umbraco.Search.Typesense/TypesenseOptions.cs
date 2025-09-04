using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Search.Typesense.Models;
using System.Globalization;
using Typesense.Setup;

namespace N3O.Umbraco.Search.Typesense;

public class TypesenseOptions : IConfigureOptions<Config> {
    private readonly IConfiguration _configuration;

    public TypesenseOptions(IConfiguration configuration) {
        _configuration = configuration;
    }
    
    public void Configure(Config options) {
        var typesenseSettings = GetTypesenseSettings();

        if (typesenseSettings.ApiKey.HasValue() && typesenseSettings.Node.HasValue() && typesenseSettings.Port.HasValue()) {
            options.ApiKey = typesenseSettings.ApiKey;
            options.Nodes = [
                new Node(typesenseSettings.Node, typesenseSettings.Port.ToString(CultureInfo.InvariantCulture), "https")
            ];
        }
    }
    
    private TypesenseSettings GetTypesenseSettings() {
        var typesenseSettings = new TypesenseSettings();
        
        _configuration.GetSection(TypesenseConstants.Configuration.Section).Bind(typesenseSettings);

        return typesenseSettings;
    }
}