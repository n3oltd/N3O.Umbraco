using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.Models.PublishedContent;
using RedirectsConstants = N3O.Umbraco.Constants.Redirects;

namespace N3O.Umbraco.Redirects.Validation;

public class RedirectContentValidator : ContentValidator  {
    private readonly IContentHelper _contentHelper;

    public RedirectContentValidator(IContentHelper contentHelper) : base(contentHelper) {
        _contentHelper = contentHelper;
    }
    public override bool IsValidator(ContentProperties content) {
        return content.ContentTypeAlias.EqualsInvariant(RedirectsConstants.ContentType);
    }

    public override void Validate(ContentProperties content) {
        var externalUrl = content.GetPropertyValueByAlias<string>(RedirectsConstants.Properties.LinkExternalUrl);
        var linkContent = _contentHelper.GetMultiNodeTreePickerValue<IPublishedContent>(content,
                                                                                        RedirectsConstants.Properties.LinkContent);

        if (externalUrl == null && linkContent == null) {
            ErrorResult("Either content or an external URL must be specified");
        }

        if (externalUrl.HasValue() && linkContent.HasValue()) {
            ErrorResult("Content and an external URL cannot both be specified");
        }
    }
}