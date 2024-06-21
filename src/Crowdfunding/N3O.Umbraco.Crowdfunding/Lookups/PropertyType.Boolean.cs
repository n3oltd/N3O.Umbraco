using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Extensions;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Lookups;

public class BooleanPropertyType : PropertyType<BooleanValueReq> {
    public BooleanPropertyType() : base("boolean") { }

    protected override Task UpdatePropertyAsync(IContentPublisher contentPublisher,
                                                string alias,
                                                BooleanValueReq data) {
        contentPublisher.Content.Boolean(alias).Set(data.Value.GetValueOrThrow());

        return Task.CompletedTask;
    }
}