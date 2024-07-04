using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Extensions;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Lookups;

public class DateTimePropertyType : PropertyType<DateTimeValueReq> {
    public DateTimePropertyType() : base("dateTime") { }

    protected override Task UpdatePropertyAsync(IContentPublisher contentPublisher,
                                                string alias,
                                                DateTimeValueReq data) {
        contentPublisher.Content.DateTime(alias).SetDateTime(data.Value);

        return Task.CompletedTask;
    }
}