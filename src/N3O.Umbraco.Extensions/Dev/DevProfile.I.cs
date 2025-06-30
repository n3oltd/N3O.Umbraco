using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace N3O.Umbraco.Dev;

public interface IDevProfile {
    void Apply(IWebHostEnvironment webHostEnvironment, IConfiguration configuration);
    bool ShouldApply(IWebHostEnvironment webHostEnvironment, IConfiguration configuration);
}