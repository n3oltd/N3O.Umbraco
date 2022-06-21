using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Storage.Services;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Storage;

public class StorageComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddOpenApiDocument(StorageConstants.ApiName);
        
        builder.Services.AddSingleton<IVolume, DiskVolume>();
    }
}
