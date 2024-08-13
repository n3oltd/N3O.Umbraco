using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Extensions;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;
using UmbracoPropertyEditors = Umbraco.Cms.Core.Constants.PropertyEditors;

namespace N3O.Umbraco.Crowdfunding.Lookups;

public class RawPropertyType : PropertyType<RawValueReq> {
    public RawPropertyType()
        : base("raw",
               (ctx, src, dest) => dest.Raw = ctx.Map<IPublishedProperty, RawValueRes>(src),
               UmbracoPropertyEditors.Aliases.TinyMce) { }

    protected override Task UpdatePropertyAsync(IContentBuilder contentBuilder,
                                                string alias,
                                                RawValueReq data) {
        contentBuilder.Raw(alias).Set(data.Value);

        return Task.CompletedTask;
    }
}