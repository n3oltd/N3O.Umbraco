using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace N3O.Umbraco.Hosting {
    public class OurMvcBinderOptions : IConfigureOptions<MvcOptions> {
        public void Configure(MvcOptions options) {
            options.ModelBinderProviders.Insert(0, new OurBodyModelBinderProvider());
        }
    }
}
