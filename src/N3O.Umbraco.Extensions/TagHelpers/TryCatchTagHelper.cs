using Microsoft.AspNetCore.Razor.TagHelpers;
using N3O.Umbraco.Security;
using System;
using System.Threading.Tasks;

namespace N3O.Umbraco.TagHelpers;

[HtmlTargetElement("try-catch")]
public class TryCatchTagHelper : TagHelper {
    private readonly Lazy<IBackofficeUser> _backofficeUser;

    public TryCatchTagHelper(Lazy<IBackofficeUser> backofficeUser) {
        _backofficeUser = backofficeUser;
    }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
        output.TagName = null;

        try {
            var childContent = await output.GetChildContentAsync();

            output.Content.SetHtmlContent(childContent);
        } catch (Exception ex) {
            if (_backofficeUser.Value.IsLoggedIn()) {
                output.Content.SetHtmlContent($"<pre>{ex}</pre>");
            } else {
                output.SuppressOutput();
            }
        }
    }
}