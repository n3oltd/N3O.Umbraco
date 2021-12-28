using Umbraco.Cms.Web.Common.ApplicationBuilder;

namespace N3O.Umbraco.Hosting {
    public interface IUmbracoEndpointExtension {
        void Run(IUmbracoEndpointBuilderContext u);
    }
}