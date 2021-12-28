using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Web.Common.ApplicationBuilder;
using Umbraco.Extensions;

namespace N3O.Umbraco;

public abstract class StartupBase {
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IConfiguration _configuration;

    protected StartupBase(IWebHostEnvironment webHostEnvironment, IConfiguration configuration) {
        _webHostEnvironment = webHostEnvironment;
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services) {
        Composer.WebHostEnvironment = _webHostEnvironment;
        
        services.AddOpenApiDocument()
                .AddUmbraco(_webHostEnvironment, _configuration)
                .AddBackOffice()
                .AddWebsite()
                .AddComposers()
                .Build();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
        if (env.IsDevelopment()) {
            app.UseDeveloperExceptionPage();
        }

        app.UseOpenApi();
        app.UseSwaggerUi3();

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

    protected virtual void ConfigureEndpoints(IUmbracoEndpointBuilderContext umbraco) { }
    protected virtual void ConfigureMiddleware(IUmbracoApplicationBuilderContext umbraco) { }
}
