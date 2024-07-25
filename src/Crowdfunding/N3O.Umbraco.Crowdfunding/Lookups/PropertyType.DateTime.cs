using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Extensions;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;
using UmbracoPropertyEditors = Umbraco.Cms.Core.Constants.PropertyEditors;

namespace N3O.Umbraco.Crowdfunding.Lookups;

public class DateTimePropertyType : PropertyType<DateTimeValueReq> {
    public DateTimePropertyType()
        : base("dateTime",
               (ctx, src, dest) => dest.DateTime = ctx.Map<IPublishedProperty, DateTimeValueRes>(src),
               UmbracoPropertyEditors.Aliases.DateTime) { }

    protected override Task UpdatePropertyAsync(IContentPublisher contentPublisher,
                                                string alias,
                                                DateTimeValueReq data) {
        contentPublisher.Content.DateTime(alias).SetDateTime(data.Value);

        return Task.CompletedTask;
    }
}