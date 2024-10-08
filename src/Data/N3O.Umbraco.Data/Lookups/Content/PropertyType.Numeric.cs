using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Extensions;
using System.Threading.Tasks;
using UmbracoPropertyEditors = Umbraco.Cms.Core.Constants.PropertyEditors;

namespace N3O.Umbraco.Data.Lookups;

public class NumericPropertyType : PropertyType<NumericValueReq> {
    public NumericPropertyType()
        : base("numeric",
               (ctx, src, dest) => dest.Numeric = ctx.Map<PublishedContentProperty, NumericValueRes>(src),
               (ctx, src) => ctx.Map<ContentPropertyConfiguration, NumericConfigurationRes>(src),
               UmbracoPropertyEditors.Aliases.Decimal) { }

    protected override Task UpdatePropertyAsync(IContentBuilder contentBuilder,
                                                string alias,
                                                NumericValueReq data) {
        contentBuilder.Numeric(alias).SetDecimal(data.Value);

        return Task.CompletedTask;
    }
}