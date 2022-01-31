using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using System;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.Common.ApplicationBuilder;
using Umbraco.Cms.Web.Website.Controllers;

namespace N3O.Umbraco.Hosting {
    public class HostingComposer : Composer {
        public override void Compose(IUmbracoBuilder builder) {
            builder.Services.Configure<UmbracoRenderingDefaultsOptions>(c => {
                c.DefaultControllerType = typeof(PageController);
            });
        
            builder.Services.AddTransient<IConfigureOptions<MvcOptions>, OurMvcBinderOptions>();
            builder.Services.AddTransient<IConfigureOptions<MvcOptions>, OurCacheProfileOptions>();
            builder.Services.AddScoped<IActionLinkGenerator, ActionLinkGenerator>();
            builder.Services.AddScoped<StagingMiddleware>();

            builder.Services.AddOpenApiDocument("DevTools");
            
            builder.Services.Configure<UmbracoPipelineOptions>(opt => {
                AddStagingMiddleware(opt);
                ConfigureCors(opt);
            });

            RegisterRouteConstraints(builder);
        }

        private void AddStagingMiddleware(UmbracoPipelineOptions opt) {
            var filter = new UmbracoPipelineFilter("Staging");
            filter.PostPipeline = app => {
                var runtimeState = app.ApplicationServices.GetRequiredService<IRuntimeState>();

                if (runtimeState.Level == RuntimeLevel.Run) {
                    app.UseMiddleware<StagingMiddleware>();
                }
            };

            opt.AddFilter(filter);
        }
        
        private void ConfigureCors(UmbracoPipelineOptions opt) {
            var filter = new UmbracoPipelineFilter("CORS");
            filter.PostPipeline = app => {
                var webHostEnvironment = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();

                if (!webHostEnvironment.IsProduction()) {
                    app.UseCors(policy => policy.AllowAnyHeader()
                                                .AllowAnyOrigin()
                                                .AllowAnyMethod()
                                                .SetPreflightMaxAge(TimeSpan.FromMinutes(60)));
                }
            };

            opt.AddFilter(filter);
        }
        
        private void RegisterRouteConstraints(IUmbracoBuilder builder) {
            builder.Services.Configure<RouteOptions>(routeOptions => {
                routeOptions.ConstraintMap.Add("entityId", typeof(EntityIdRouteConstraint));
                routeOptions.ConstraintMap.Add("revisionId", typeof(RevisionIdRouteConstraint));
            });
        }
    }
}
