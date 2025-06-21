using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Extensions;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class UmbracoFundDimensionsReqMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<FundStructureContent, UmbracoFundStructureReq>((_, _) => new UmbracoFundStructureReq(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(FundStructureContent src, UmbracoFundStructureReq dest, MapperContext ctx) {
        dest.Dimension1 = src.FundDimension1.IfNotNull(ctx.Map<FundDimension1Content, UmbracoFundDimensionReq>);
        dest.Dimension2 = src.FundDimension2.IfNotNull(ctx.Map<FundDimension2Content, UmbracoFundDimensionReq>);
        dest.Dimension3 = src.FundDimension3.IfNotNull(ctx.Map<FundDimension3Content, UmbracoFundDimensionReq>);
        dest.Dimension4 = src.FundDimension4.IfNotNull(ctx.Map<FundDimension4Content, UmbracoFundDimensionReq>);
    }
}
