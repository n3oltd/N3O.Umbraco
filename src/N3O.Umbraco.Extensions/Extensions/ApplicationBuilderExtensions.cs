using Microsoft.AspNetCore.Builder;

namespace N3O.Umbraco.Extensions;

public static class ApplicationBuilderExtensions {
    public static IApplicationBuilder UseOpenApiWithUI(this IApplicationBuilder app) {
        if (OpenApi.IsEnabled()) {
            app.UseOpenApi();
            
            app.UseSwaggerUi(opt => {
                opt.DocExpansion = "list";
                opt.OperationsSorter = "alpha";
                opt.TagsSorter = "alpha";
            });
        }

        return app;
    }
}
