using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Extensions;
using System.Linq;
using Umbraco.Cms.Core.Mapping;
using PlatformsFundDimensionSelector = N3O.Umbraco.Cloud.Platforms.Clients.FundDimensionSelector;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public abstract class UmbracoFundDimensionReqMapping<T> : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<FundDimensionContent<T>, UmbracoFundDimensionReq>((_, _) => new UmbracoFundDimensionReq(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(FundDimensionContent<T> src, UmbracoFundDimensionReq dest, MapperContext ctx) {
        dest.Browsable = src.Browsable;
        dest.Searchable = src.Searchable;
        
        dest.View = new PublishedFundDimensionView();
        dest.View.Selector = src.Selector.ToEnum<PlatformsFundDimensionSelector>();

        if (src.Selector == FundDimensionSelectors.Toggle) {
            var toggleValueElement = src.ToggleValue.Single();
            
            dest.View.Toggle = new PublishedFundDimensionToggleOptions();
            dest.View.Toggle.Label = toggleValueElement.Label;
            dest.View.Toggle.OffValue = toggleValueElement.OffValue;
            dest.View.Toggle.OnValue = toggleValueElement.OnValue;
        }
    }
}

public class UmbracoFundDimension1ReqMapping : UmbracoFundDimensionReqMapping<FundDimension1Content> { }
public class UmbracoFundDimension2ReqMapping : UmbracoFundDimensionReqMapping<FundDimension2Content> { }
public class UmbracoFundDimension3ReqMapping : UmbracoFundDimensionReqMapping<FundDimension3Content> { }
public class UmbracoFundDimension4ReqMapping : UmbracoFundDimensionReqMapping<FundDimension4Content> { }