using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Uploader.Models;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Content;

[UmbracoContent(CrowdfundingConstants.SignInModalSettings.Alias)]
public class SignInModalSettingsContent : UmbracoContent<SignInModalSettingsContent> {
    public string Title => GetValue(x => x.Title);
    public string LogoText => GetValue(x => x.LogoText);
    public string AboveButtonText => GetValue(x => x.AboveButtonText);
    public string ButtonLabel => GetValue(x => x.ButtonLabel);
    public FileUpload LogoSvg => GetValue(x => x.LogoSvg);
    public IEnumerable<SignInModalItemElement> Items => GetNestedAs(x => x.Items);
}