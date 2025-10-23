﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Cloud.Platforms.Hosting;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.Common.ApplicationBuilder;

namespace N3O.Umbraco.Cloud.Platforms;

public class PlatformsComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddOpenApiDocument(PlatformsConstants.BackOfficeApiName);

        builder.Services.AddSingleton<IPlatformsPageAccessor, PlatformsPageAccessor>();
        builder.Services.AddScoped<PlatformsTemplatesMiddleware>();
        
        RegisterAll(t => t.ImplementsInterface<IPreviewTagGenerator>(),
                    t => builder.Services.AddTransient(typeof(IPreviewTagGenerator), t));
        
        builder.Services.Configure<UmbracoPipelineOptions>(opt => {
            var filter = new UmbracoPipelineFilter(nameof(PlatformsTemplatesMiddleware));
            
            filter.PostPipeline = app => {
                var runtimeState = app.ApplicationServices.GetRequiredService<IRuntimeState>();

                if (runtimeState.Level == RuntimeLevel.Run) {
                    app.UseMiddleware<PlatformsTemplatesMiddleware>();
                }
            };

            opt.AddFilter(filter);
        });
    }
}