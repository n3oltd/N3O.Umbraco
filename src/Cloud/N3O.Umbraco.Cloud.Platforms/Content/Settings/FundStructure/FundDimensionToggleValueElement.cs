using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.FundStructure.Alias)]
public class FundDimensionToggleValueElement : UmbracoElement<FundDimensionToggleValueElement> {
    public string Label => GetValue(x => x.Label);
    public string OffValue => GetValue(x => x.OffValue);
    public string OnValue => GetValue(x => x.OnValue);
}