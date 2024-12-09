using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using System.Linq;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Storage.Azure;

[ComposeAfter(typeof(StorageComposer))]
public class AzureStorageComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddSingleton<IVolume, AzureVolume>();
        
        var storageConfigured = builder.Config
                                       .GetSection("umbraco")
                                       .GetChildren()
                                       .Any(x => x.Key.EqualsInvariant("storage") &&
                                                 x.GetChildren().Any(c => c.Key.EqualsInvariant("azureBlob")));

        if (storageConfigured) {
            builder.AddAzureBlobMediaFileSystem();
            builder.AddAzureBlobImageSharpCache();
        }
    }
}
