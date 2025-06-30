using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System.Linq;
using Umbraco.Extensions;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Settings.FundStructure.Alias)]
public class FundStructureContent : UmbracoContent<FundStructureContent> {
    public FundDimension1Content FundDimension1 => GetFundDimension<FundDimension1Content>();
    public FundDimension2Content FundDimension2 => GetFundDimension<FundDimension2Content>();
    public FundDimension3Content FundDimension3 => GetFundDimension<FundDimension3Content>();
    public FundDimension4Content FundDimension4 => GetFundDimension<FundDimension4Content>();

    private T GetFundDimension<T>() {
        var fundDimension = Content().ChildrenOfType(AliasHelper<T>.ContentTypeAlias())
                                     .OrEmpty()
                                     .As<T>()
                                     .SingleOrDefault();

        return fundDimension;
    }
}