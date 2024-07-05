using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Extensions;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Lookups;

public class TextareaPropertyType : PropertyType<TextareaValueReq> {
    public TextareaPropertyType() : base("textarea") { }

    protected override Task UpdatePropertyAsync(IContentPublisher contentPublisher,
                                                string alias,
                                                TextareaValueReq data) {
        contentPublisher.Content.Textarea(alias).Set(data.Value);

        return Task.CompletedTask;
    }
}