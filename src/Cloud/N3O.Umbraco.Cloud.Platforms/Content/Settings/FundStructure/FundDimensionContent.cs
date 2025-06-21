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
    
    public int Number => Index + 1;
    
    public abstract int Index { get; }
}

[UmbracoContent(PlatformsConstants.Settings.FundStructure.FundDimension1)]
public class FundDimension1Content : FundDimensionContent<FundDimension1Content> {
    public override int Index => 0;
}

[UmbracoContent(PlatformsConstants.Settings.FundStructure.FundDimension2)]
public class FundDimension2Content : FundDimensionContent<FundDimension2Content> {
    public override int Index => 1;
}

[UmbracoContent(PlatformsConstants.Settings.FundStructure.FundDimension3)]
public class FundDimension3Content : FundDimensionContent<FundDimension3Content> {
    public override int Index => 2;
}

[UmbracoContent(PlatformsConstants.Settings.FundStructure.FundDimension4)]
public class FundDimension4Content : FundDimensionContent<FundDimension4Content> {
    public override int Index => 3;
}