using Microsoft.AspNetCore.Builder;

namespace N3O.Umbraco.Extensions {
    public static class ApplicationBuilderExtensions {
        public static IApplicationBuilder UseOpenApiWithUI(this IApplicationBuilder app) {
            app.UseOpenApi();
            app.UseSwaggerUi3(opt => {
                opt.DocExpansion = "list";
                opt.OperationsSorter = "alpha";
                opt.TagsSorter = "alpha";
            });

            return app;
        }
    }
}
