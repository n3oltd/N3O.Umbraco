using Microsoft.Extensions.Options;
using N3O.Umbraco.Content;
using N3O.Umbraco.Search.Typesense.Content.Settings;
using N3O.Umbraco.Search.Typesense.Models;
using Typesense.Setup;

namespace N3O.Umbraco.Search.Typesense;

public class TypesenseOptions : IConfigureOptions<Config> {
    private readonly IContentCache _contentCache;

    public TypesenseOptions(IContentCache contentCache) {
        _contentCache = contentCache;
    }
    
    public void Configure(Config options) {
        var typesenseSettings = GetTypesenseSettings();

        options.ApiKey = typesenseSettings.ApiKey;
        options.Nodes = new[] {
            new Node(typesenseSettings.Node, "8108", "https")
        };
    }
    
    private TypesenseSettings GetTypesenseSettings() {
        var settingsContent = _contentCache.Single<TypesenseSettingsContent>();
    
        if (settingsContent != null) {
            return new TypesenseSettings(settingsContent.ApiKey, settingsContent.Node);
        }

        return null;
    }
}