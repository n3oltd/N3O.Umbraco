using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Extensions;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Lookups;

public class RawPropertyType : PropertyType<RawValueReq> {
    public RawPropertyType() : base("raw") { }

    protected override Task UpdatePropertyAsync(IContentPublisher contentPublisher,
                                                string alias,
                                                RawValueReq data) {
        contentPublisher.Content.Raw(alias).Set(data.Value);

        return Task.CompletedTask;
    }
}