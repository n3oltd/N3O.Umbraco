using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Extensions;
using System.Threading.Tasks;
using UmbracoPropertyEditors = Umbraco.Cms.Core.Constants.PropertyEditors;

namespace N3O.Umbraco.Data.Lookups;

public class DateTimePropertyType : PropertyType<DateTimeValueReq> {
    public DateTimePropertyType()
        : base("dateTime",
               (ctx, src, dest) => dest.DateTime = ctx.Map<PublishedContentProperty, DateTimeValueRes>(src),
               UmbracoPropertyEditors.Aliases.DateTime) { }

    protected override Task UpdatePropertyAsync(IContentBuilder contentBuilder,
                                                string alias,
                                                DateTimeValueReq data) {
        contentBuilder.DateTime(alias).SetDateTime(data.Value);

        return Task.CompletedTask;
    }
}