using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.Options;
using N3O.Umbraco.Content;
using Typesense.Setup;

namespace N3O.Umbraco.Search.Typesense.Content.Settings;

public class TypesenseSettingsContent : UmbracoContent<TypesenseSettingsContent> {
    public string ApiKey => GetValue(x => x.ApiKey);
    public string Node => GetValue(x => x.Node);
}
