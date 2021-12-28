using Microsoft.Extensions.Hosting;

namespace N3O.Umbraco.Hosting {
    public interface IHostBuilderExtension {
        void Run(IHostBuilder hostBuilder);
    }
}