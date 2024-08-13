using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Extensions;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;
using UmbracoPropertyEditors = Umbraco.Cms.Core.Constants.PropertyEditors;

namespace N3O.Umbraco.Crowdfunding.Lookups;

public class NumericPropertyType : PropertyType<NumericValueReq> {
    public NumericPropertyType()
        : base("numeric",
               (ctx, src, dest) => dest.Numeric = ctx.Map<IPublishedProperty, NumericValueRes>(src),
               UmbracoPropertyEditors.Aliases.Decimal) { }

    protected override Task UpdatePropertyAsync(IContentBuilder contentBuilder,
                                                string alias,
                                                NumericValueReq data) {
        contentBuilder.Numeric(alias).SetDecimal(data.Value);

        return Task.CompletedTask;
    }
}