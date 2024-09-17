using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Extensions;
using System.Threading.Tasks;
using UmbracoPropertyEditors = Umbraco.Cms.Core.Constants.PropertyEditors;

namespace N3O.Umbraco.Data.Lookups;

public class TextareaPropertyType : PropertyType<TextareaValueReq> {
    public TextareaPropertyType()
        : base("textarea",
               (ctx, src, dest) => dest.Textarea = ctx.Map<PublishedContentProperty, TextareaValueRes>(src),
               UmbracoPropertyEditors.Aliases.TextArea) { }

    protected override Task UpdatePropertyAsync(IContentBuilder contentBuilder,
                                                string alias,
                                                TextareaValueReq data) {
        contentBuilder.Textarea(alias).Set(data.Value);

        return Task.CompletedTask;
    }
}