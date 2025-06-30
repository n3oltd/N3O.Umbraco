using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System.Linq;

namespace N3O.Umbraco.Cloud.Platforms.Validators;

public class FundDimensionValidator : ContentValidator {
    private static readonly string FundDimension1Alias = AliasHelper<FundDimension1Content>.ContentTypeAlias();
    private static readonly string FundDimension2Alias = AliasHelper<FundDimension2Content>.ContentTypeAlias();
    private static readonly string FundDimension3Alias = AliasHelper<FundDimension3Content>.ContentTypeAlias();
    private static readonly string FundDimension4Alias = AliasHelper<FundDimension4Content>.ContentTypeAlias();
    private static readonly string SelectorAlias = AliasHelper<FundDimension1Content>.PropertyAlias(x => x.Selector);
    private static readonly string ToggleValueAlias = AliasHelper<FundDimension1Content>.PropertyAlias(x => x.ToggleValue);
    
    private readonly IContentHelper _contentHelper;
    
    public FundDimensionValidator(IContentHelper contentHelper) : base(contentHelper) {
        _contentHelper = contentHelper;
    }
    
    public override bool IsValidator(ContentProperties content) {
        return content.ContentTypeAlias.IsAnyOf(FundDimension1Alias,
                                                FundDimension2Alias,
                                                FundDimension3Alias,
                                                FundDimension4Alias);
    }
    
    public override void Validate(ContentProperties content) {
        var selector = _contentHelper.GetDataListValue<FundDimensionSelector>(content, SelectorAlias);
        var toggleValueElement = _contentHelper.GetBlockList(content.GetElementsPropertyByAlias(ToggleValueAlias))
                                               ?.SingleOrDefault()
                                               ?.Content
                                               .As<FundDimensionToggleValueElement>();

        if (selector == FundDimensionSelectors.Dropdown) {
            if (toggleValueElement.HasValue()) {
                ErrorResult("Toggle values cannot be specified for a dropdown dimension");
            }
        }
        
        if (selector == FundDimensionSelectors.Toggle) {
            if (!toggleValueElement.HasValue(x => x.Label) ||
                !toggleValueElement.HasValue(x => x.OnValue) ||
                !toggleValueElement.HasValue(x => x.OffValue)) {
                ErrorResult("A label, on and off values must all be specified for a toggle dimension");   
            }
        }
    }
}