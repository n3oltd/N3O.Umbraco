using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using System;
using System.Linq;
using Umbraco.Cms.Core.Mapping;
using FundDimensionSelector = N3O.Umbraco.Cloud.Platforms.Clients.FundDimensionSelector;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public abstract class FundDimensionMapping<T> : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<FundDimensionContent<T>, UmbracoFundDimensionReq>((_, _) => new UmbracoFundDimensionReq(), Map);
    }

    // Umbraco.Code.MapAll -Id -Name
    private void Map(FundDimensionContent<T> src, UmbracoFundDimensionReq dest, MapperContext ctx) {
        dest.Browsable = src.Browsable;
        dest.Searchable = src.Searchable;
        
        dest.View = new PublishedFundDimensionView();
        dest.View.Selector = (FundDimensionSelector) Enum.Parse(typeof(FundDimensionSelector), src.Selector.Id, true);

        if (src.Selector == FundDimensionSelectors.Toggle) {
            var toggleValue = src.ToggleValue.Single();
            
            dest.View.Toggle = new PublishedFundDimensionToggleOptions();
        
            dest.View.Toggle.Label = toggleValue.Label;
            dest.View.Toggle.OffValue = toggleValue.OffValue;
            dest.View.Toggle.OnValue = toggleValue.OnValue;
        }
    }
}

public class FundDimension1Mapping : FundDimensionMapping<FundDimension1Content> { }
public class FundDimension2Mapping : FundDimensionMapping<FundDimension2Content> { }
public class FundDimension3Mapping : FundDimensionMapping<FundDimension3Content> { }
public class FundDimension4Mapping : FundDimensionMapping<FundDimension4Content> { }