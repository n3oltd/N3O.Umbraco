using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using Newtonsoft.Json;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Json;

public class JsonComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddScoped<IJsonProvider, JsonProvider>();
        
        RegisterAll(t => t.IsSubclassOfType(typeof(JsonConverter)),
                    t => builder.Services.AddScoped(typeof(JsonConverter), t));
    }
}
