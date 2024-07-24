using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Extensions;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;
using UmbracoPropertyEditors = Umbraco.Cms.Core.Constants.PropertyEditors;

namespace N3O.Umbraco.Crowdfunding.Lookups;

public class TextareaPropertyType : PropertyType<TextareaValueReq> {
    public TextareaPropertyType()
        : base("textarea",
               (ctx, src, dest) => dest.Textarea = ctx.Map<IPublishedProperty, TextareaValueRes>(src),
               UmbracoPropertyEditors.Aliases.TextArea) { }

    protected override Task UpdatePropertyAsync(IContentPublisher contentPublisher,
                                                string alias,
                                                TextareaValueReq data) {
        contentPublisher.Content.Textarea(alias).Set(data.Value);

        return Task.CompletedTask;
    }
}