using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Redirects.Validation; 

public class RedirectValidator : ContentValidator {
    private readonly IContentHelper _contentHelper;

    public RedirectValidator(IContentHelper contentHelper) : base(contentHelper) {
        _contentHelper = contentHelper;
    }
    public override bool IsValidator(ContentProperties content) {
        return content.ContentTypeAlias.EqualsInvariant(ReadConstants.Redirect.ContentType);
    }

    public override void Validate(ContentProperties content) {
        var linkContent = _contentHelper.GetPickerValue<IPublishedContent>(content, ReadConstants.Redirect.Properties.LinkContent);
        var externalUrl = content.GetPropertyValueByAlias<string>(ReadConstants.Redirect.Properties.LinkExternalUrl);

        if (externalUrl == null && linkContent == null) {
            ErrorResult("Both link content and external url cannot be null");
        }

        if (externalUrl != null && linkContent != null) {
            ErrorResult("Only link content or external url can be specified");
        }
    }
}