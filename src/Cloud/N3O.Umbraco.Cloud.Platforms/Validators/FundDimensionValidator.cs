using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;

namespace N3O.Umbraco.Cloud.Platforms.Validators;

public class FundDimensionValidator : ContentValidator {
    private readonly IContentHelper _contentHelper;
    
    private static readonly string FundDimension1 = AliasHelper<PlatformsFundDimension1>.ContentTypeAlias();
    private static readonly string FundDimension2 = AliasHelper<PlatformsFundDimension2>.ContentTypeAlias();
    private static readonly string FundDimension3 = AliasHelper<PlatformsFundDimension3>.ContentTypeAlias();
    private static readonly string FundDimension4 = AliasHelper<PlatformsFundDimension4>.ContentTypeAlias();
    
    private static readonly string FundDimensionSelector = AliasHelper<IPlatformsFundDimension>.PropertyAlias(x => x.Selector);
    private static readonly string ToggleValue = AliasHelper<IPlatformsFundDimension>.PropertyAlias(x => x.ToggleValue);
    
    public FundDimensionValidator(IContentHelper contentHelper) : base(contentHelper) {
        _contentHelper = contentHelper;
    }
    
    public override bool IsValidator(ContentProperties content) {
        return content.ContentTypeAlias.IsAnyOf(FundDimension1, FundDimension2, FundDimension3, FundDimension4);
    }
    
    public override void Validate(ContentProperties content) {
        var fundDimensionSelector = _contentHelper.GetDataListValue<FundDimensionSelector>(content, FundDimensionSelector);
        var toggleValue = _contentHelper.GetBlockList(content.GetElementsPropertyByAlias(ToggleValue));

        if (fundDimensionSelector == FundDimensionSelectors.Dropdown && toggleValue.HasValue()) {
            ErrorResult("Toggle value cannot be specified for dropdown fund dimension selector");
        }
        
        if (fundDimensionSelector == FundDimensionSelectors.Toggle && !toggleValue.HasValue()) {
            ErrorResult("Toggle value must be specified for toggle fund dimension selector");
        }
    }
}