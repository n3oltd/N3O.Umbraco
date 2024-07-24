using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Extensions;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;
using UmbracoPropertyEditors = Umbraco.Cms.Core.Constants.PropertyEditors;

namespace N3O.Umbraco.Crowdfunding.Lookups;

public class BooleanPropertyType : PropertyType<BooleanValueReq> {
    public BooleanPropertyType()
        : base("boolean",
               (ctx, src, dest) => dest.Boolean = ctx.Map<IPublishedProperty, BooleanValueRes>(src),
               UmbracoPropertyEditors.Aliases.Boolean) { }

    protected override Task UpdatePropertyAsync(IContentPublisher contentPublisher,
                                                string alias,
                                                BooleanValueReq data) {
        contentPublisher.Content.Boolean(alias).Set(data.Value.GetValueOrThrow());

        return Task.CompletedTask;
    }
}