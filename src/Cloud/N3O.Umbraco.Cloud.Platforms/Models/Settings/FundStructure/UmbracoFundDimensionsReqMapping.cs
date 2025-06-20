using MuslimHands.Website.Connect.Clients;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class UmbracoFundDimensionsReqMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<PlatformsFundStructure, UmbracoFundStructureReq>((_, _) => new UmbracoFundStructureReq(), Map);
    }

    // Umbraco.Code.MapAll -Id -Name
    private void Map(PlatformsFundStructure src, UmbracoFundStructureReq dest, MapperContext ctx) {
        var dimension1 = src.Child<PlatformsFundDimension1>();
        var dimension2 = src.Child<PlatformsFundDimension2>();
        var dimension3 = src.Child<PlatformsFundDimension3>();
        var dimension4 = src.Child<PlatformsFundDimension4>();

        if (dimension1.HasValue()) {
            dest.Dimension1 = ctx.Map<IPlatformsFundDimension, UmbracoFundDimensionReq>(dimension1);
        }
        
        if (dimension2.HasValue()) {
            dest.Dimension2 = ctx.Map<IPlatformsFundDimension, UmbracoFundDimensionReq>(dimension2);
        }
        
        if (dimension3.HasValue()) {
            dest.Dimension3 = ctx.Map<IPlatformsFundDimension, UmbracoFundDimensionReq>(dimension3);
        }
        
        if (dimension4.HasValue()) {
            dest.Dimension4 = ctx.Map<IPlatformsFundDimension, UmbracoFundDimensionReq>(dimension4);
        }
    }
}
