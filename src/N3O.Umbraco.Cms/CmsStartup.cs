using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Web.Common.ApplicationBuilder;
using Umbraco.Extensions;

namespace N3O.Umbraco {
    public abstract class CmsStartup {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        protected CmsStartup(IWebHostEnvironment webHostEnvironment, IConfiguration configuration) {
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services) {
            Composer.WebHostEnvironment = _webHostEnvironment;
        
            services.AddUmbraco(_webHostEnvironment, _configuration)
                    .AddBackOffice()
                    .AddWebsite()
                    .AddComposers()
                    .AddContentment(opt => {
                        opt.DisableTelemetry = true;
                    })
                    .Build();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsProduction()) {
                app.UseHsts();
            } else {
                app.UseDeveloperExceptionPage();
                app.UseOpenApiWithUI();
            }

            //ConfigureStaticFiles(app);

            app.UseUmbraco()
               .WithMiddleware(u => {
                   u.UseBackOffice();
                   u.UseWebsite();

                   ConfigureMiddleware(u);
               })
               .WithEndpoints(u => {
                   u.UseInstallerEndpoints();
                   u.UseBackOfficeEndpoints();
                   u.UseWebsiteEndpoints();

                   u.RunExtensions();

                   ConfigureEndpoints(u);
               });
        }

        private void ConfigureStaticFiles(IApplicationBuilder app) {
            var provider = new FileExtensionContentTypeProvider();
            provider.Mappings[".pdf"] = "application/pdf";

            ConfigureFileExtensions(provider);

            var options = new StaticFileOptions();
            options.ContentTypeProvider = provider;
            
            app.UseStaticFiles(options);
        }

        protected virtual void ConfigureEndpoints(IUmbracoEndpointBuilderContext umbraco) { }
        protected virtual void ConfigureFileExtensions(FileExtensionContentTypeProvider provider) { }
        protected virtual void ConfigureMiddleware(IUmbracoApplicationBuilderContext umbraco) { }
    }
}
