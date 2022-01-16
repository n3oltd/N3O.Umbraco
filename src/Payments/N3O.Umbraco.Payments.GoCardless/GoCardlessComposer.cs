﻿using GoCardless;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Content;
using N3O.Umbraco.Payments.GoCardless.Content;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Payments.GoCardless {
    public class GoCardlessComposer : Composer {
        public override void Compose(IUmbracoBuilder builder) {
            builder.Services.AddTransient<GoCardlessClient>(serviceProvider => {
                var contentCache = serviceProvider.GetRequiredService<IContentCache>();
                var webHostEnvironment = serviceProvider.GetRequiredService<IWebHostEnvironment>();
                var settings = contentCache.Single<GoCardlessSettingsContent>();

                GoCardlessClient goCardlessClient;

                if (webHostEnvironment.IsProduction()) {
                    goCardlessClient = GoCardlessClient.Create(settings.ProductionAccessToken,
                                                               GoCardlessClient.Environment.LIVE);
                } else {
                    goCardlessClient = GoCardlessClient.Create(settings.SandboxAccessToken,
                                                               GoCardlessClient.Environment.SANDBOX);
                }

                return goCardlessClient;
            });
        }
    }
}