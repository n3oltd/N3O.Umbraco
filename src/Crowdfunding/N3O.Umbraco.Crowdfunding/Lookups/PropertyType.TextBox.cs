using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Extensions;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Lookups;

public class TextBoxPropertyType : PropertyType<TextBoxValueReq> {
    public TextBoxPropertyType() : base("textBox") { }

    protected override Task UpdatePropertyAsync(IContentPublisher contentPublisher,
                                                string alias,
                                                TextBoxValueReq data) {
        contentPublisher.Content.TextBox(alias).Set(data.Value);

        return Task.CompletedTask;
    }
}