using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace N3O.Umbraco.Hosting;

public interface IConfigurationBuilderExtension {
    void Run(IConfigurationBuilder configurationBuilder, WebHostBuilderContext context);
}
