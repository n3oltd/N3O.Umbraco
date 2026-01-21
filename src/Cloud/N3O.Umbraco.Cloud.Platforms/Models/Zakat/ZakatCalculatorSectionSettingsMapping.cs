using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class ZakatCalculatorSectionSettingsMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<ZakatCalculatorSectionSettingsContent, ZakatCalculatorSectionReq>((_, _) => new ZakatCalculatorSectionReq(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(ZakatCalculatorSectionSettingsContent src, ZakatCalculatorSectionReq dest, MapperContext ctx) {
        dest.Alias = src.Alias;
        dest.Name = src.Name;
        dest.Fields = src.Fields.Select(ctx.Map<ZakatCalculatorFieldSettingsContent, ZakatCalculatorFieldReq>).ToList();
        
        dest.Content = new RichTextContentReq();
        dest.Content.Html = src.Content?.ToHtmlString();
    }
}