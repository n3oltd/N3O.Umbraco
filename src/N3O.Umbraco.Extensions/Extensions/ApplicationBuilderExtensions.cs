using Microsoft.AspNetCore.Builder;

namespace N3O.Umbraco.Extensions {
    public static class ApplicationBuilderExtensions {
        public static IApplicationBuilder UseOpenApiWithUI(this IApplicationBuilder app) {
            app.UseOpenApi();
            app.UseSwaggerUi3();

            return app;
        }
    }
}
