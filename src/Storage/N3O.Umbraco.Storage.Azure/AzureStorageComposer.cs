using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Context;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Web.Common.ApplicationBuilder;
using Umbraco.StorageProviders.AzureBlob;

namespace N3O.Umbraco.Storage.Azure {
    public class AzureStorageComposer : Composer {
        public override void Compose(IUmbracoBuilder builder) {
            if (!WebHostEnvironment.IsDevelopment()) {
                builder.AddAzureBlobMediaFileSystem()
                       .AddCdnMediaUrlProvider();

                builder.Services.Configure<UmbracoPipelineOptions>(options => {
                    var filter = new UmbracoPipelineFilter("AzureStorage");
                    filter.Endpoints = app => app.UseMiddleware<AzureBlobFileSystemMiddleware>();
            
                    options.AddFilter(filter);
                });
            }
        }
    }
}