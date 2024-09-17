using N3O.Umbraco.Content;
using N3O.Umbraco.Uploader.Models;

namespace N3O.Umbraco.Crowdfunding.Content;

public class SignInModalItemElement : UmbracoElement<SignInModalItemElement> {
    public FileUpload Icon => GetValue(x => x.Icon);
    public string Text => GetValue(x => x.Text);
    public string Title => GetValue(x => x.Title);
}