using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class ZakatCalculatorFieldSettingsMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<ZakatCalculatorFieldSettingsContent, ZakatCalculatorFieldReq>((_, _) => new ZakatCalculatorFieldReq(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(ZakatCalculatorFieldSettingsContent src, ZakatCalculatorFieldReq dest, MapperContext ctx) {
        dest.Classification = src.Classification.ToEnum<CalculatorFieldClassification>();
        dest.Type = src.Type.ToEnum<CalculatorFieldType>();
        dest.Alias = src.Alias;
        dest.Name = src.Name;
        dest.Tooltip = src.Tooltip;
        
        dest.Content = new RichTextContentReq();
        dest.Content.Html = src.Content.ToHtmlString();

        if (src.Metal.HasValue()) {
            dest.Metal = new ZakatCalculatorMetalFieldReq();
            dest.Metal.Metal = src.Metal.ToEnum<Clients.Metal>();
        }
    }
}