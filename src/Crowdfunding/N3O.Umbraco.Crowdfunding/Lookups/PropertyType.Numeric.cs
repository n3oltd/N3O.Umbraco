using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Extensions;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Lookups;

public class NumericPropertyType : PropertyType<NumericValueReq> {
    public NumericPropertyType() : base("numeric") { }

    protected override Task UpdatePropertyAsync(IContentPublisher contentPublisher,
                                                string alias,
                                                NumericValueReq data) {
        contentPublisher.Content.Numeric(alias).SetDecimal(data.Value);

        return Task.CompletedTask;
    }
}