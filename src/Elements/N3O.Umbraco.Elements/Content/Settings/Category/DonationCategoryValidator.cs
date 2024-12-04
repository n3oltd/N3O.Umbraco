using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System.Collections.Generic;

namespace N3O.Umbraco.Elements.Content;

public class DonationCategoryValidator<TDonationCategoryContent> : ContentValidator {
    private readonly IContentLocator _contentLocator;

    private static readonly IEnumerable<string> Aliases = new[] {
        AliasHelper<TDonationCategoryContent>.ContentTypeAlias(),
    };
    
    protected DonationCategoryValidator(IContentHelper contentHelper, IContentLocator contentLocator) : base(contentHelper) {
        _contentLocator = contentLocator;
    }
    
    public override bool IsValidator(ContentProperties content) {
        return Aliases.Contains(content.ContentTypeAlias, true);
    }
    
    public override void Validate(ContentProperties content) {
        ValidateLevel(content);
    }

    private void ValidateLevel(ContentProperties content) {
        var categories = _contentLocator.Single(ElementsConstants.DonationCategories.Alias);

        if (categories.Id != content.ParentId && categories.Children.None(x => x.Id == content.ParentId)) {
            ErrorResult("Cannot add category more than two level deep");
        }
    }
}