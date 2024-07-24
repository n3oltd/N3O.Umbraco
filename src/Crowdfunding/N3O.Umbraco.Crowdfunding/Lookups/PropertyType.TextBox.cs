using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Extensions;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;
using UmbracoPropertyEditors = Umbraco.Cms.Core.Constants.PropertyEditors;

namespace N3O.Umbraco.Crowdfunding.Lookups;

public class TextBoxPropertyType : PropertyType<TextBoxValueReq> {
    public TextBoxPropertyType()
        : base("textBox",
               (ctx, src, dest) => dest.TextBox = ctx.Map<IPublishedProperty, TextBoxValueRes>(src),
               UmbracoPropertyEditors.Aliases.TextBox) { }

    protected override Task UpdatePropertyAsync(IContentPublisher contentPublisher,
                                                string alias,
                                                TextBoxValueReq data) {
        contentPublisher.Content.TextBox(alias).Set(data.Value);

        return Task.CompletedTask;
    }
}