using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Allocations.Models;

namespace N3O.Umbraco.Cloud.Platforms.Content;

public class DesignatableElementContent<T> : UmbracoContent<T>
    where T : DesignatableElementContent<T> {
    public DesignationContent Designation => GetAs(x => x.Designation);
    public FundDimension1Value Dimension1 => GetValue(x => x.Dimension1);
    public FundDimension2Value Dimension2 => GetValue(x => x.Dimension2);
    public FundDimension3Value Dimension3 => GetValue(x => x.Dimension3);
    public FundDimension4Value Dimension4 => GetValue(x => x.Dimension4);
}