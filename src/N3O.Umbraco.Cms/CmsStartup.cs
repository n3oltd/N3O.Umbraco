using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Hosting;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Web.Common.ApplicationBuilder;
using Umbraco.Extensions;

namespace N3O.Umbraco;

public abstract class CmsStartup {
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IConfiguration _configuration;

    protected CmsStartup(IWebHostEnvironment webHostEnvironment, IConfiguration configuration) {
        _webHostEnvironment = webHostEnvironment;
        _configuration = configuration;

        EnvironmentSettings.LoadFromConfiguration(configuration);
    }

    public void ConfigureServices(IServiceCollection services) {
        Composer.WebHostEnvironment = _webHostEnvironment;

        services.AddUmbraco(_webHostEnvironment, _configuration)
                .AddBackOffice()
                .AddWebsite()
                .AddDeliveryApi()
                .AddComposers()
                .AddContentment(opt => opt.DisableTelemetry = true)
                .Build();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
        if (env.IsProduction()) {
            app.UseHsts();
        } else {
            app.UseDeveloperExceptionPage();
        }

        app.UseRewriter(GetRewriteOptions());
        
        var staticFileOptions = new StaticFileOptions();
        ConfigureStaticFiles(staticFileOptions);
        
        app.UseWhen(context => !context.Request.Path.StartsWithSegments("/media"),
                    appBuilder => appBuilder.UseStaticFiles());
        
        app.UseOpenApiWithUI();
        
        app.UseMiddleware<WellKnownFolderMiddleware>();

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
    protected virtual void ConfigureStaticFiles(StaticFileOptions staticFileOptions) { }

    private RewriteOptions GetRewriteOptions() {
        var canonicalDomain = EnvironmentSettings.GetValue("N3O_Canonical_Domain");
        var aliasDomains = EnvironmentSettings.GetValue("N3O_Alias_Domains");
        var options = new RewriteOptions();
        
        if (canonicalDomain.HasValue()) {
            options.Rules.Add(new CanonicalDomainRedirectRule(canonicalDomain, aliasDomains.Or("").Split('|')));
        }

        return options;
    }
}
