using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Platforms.Content;

public abstract class FundDimensionContent<T> : UmbracoContent<FundDimensionContent<T>> {
    public bool Browsable => GetValue(x => x.Browsable);
    public bool Searchable => GetValue(x => x.Searchable);
    public FundDimensionSelector Selector => GetValue(x => x.Selector);
    public IEnumerable<FundDimensionToggleValueElement> ToggleValue => GetNestedAs(x => x.ToggleValue);
}

[UmbracoContent(PlatformsConstants.FundDimension1.Alias)]
public class FundDimension1Content : FundDimensionContent<FundDimension1Content> {
}

[UmbracoContent(PlatformsConstants.FundDimension2.Alias)]
public class FundDimension2Content : FundDimensionContent<FundDimension2Content> {
}

[UmbracoContent(PlatformsConstants.FundDimension3.Alias)]
public class FundDimension3Content : FundDimensionContent<FundDimension3Content> {
}

[UmbracoContent(PlatformsConstants.FundDimension4.Alias)]
public class FundDimension4Content : FundDimensionContent<FundDimension4Content> {
}