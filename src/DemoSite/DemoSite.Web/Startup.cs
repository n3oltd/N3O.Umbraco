using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using N3O.Umbraco;

namespace DemoSite.Web {
    public class Startup : CmsStartup {
        public Startup(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
            : base(webHostEnvironment, configuration) { }
    }
}