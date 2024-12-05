using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;

namespace N3O.Umbraco.Elements.Content;

public abstract class DonationCategoryValidator<TDonationCategoryContent> : ContentValidator {
    private static readonly string Alias = AliasHelper<TDonationCategoryContent>.ContentTypeAlias();
    
    protected DonationCategoryValidator(IContentHelper contentHelper) : base(contentHelper) { }
    
    public override bool IsValidator(ContentProperties content) {
        return Alias.EqualsInvariant(content.ContentTypeAlias);
    }

    public override void Validate(ContentProperties content) {
        if (content.Level > 4) {
            ErrorResult("Categories cannot be nested more than two levels deep");
        }

        ValidateCategory(content);
    }
    
    protected abstract void ValidateCategory(ContentProperties content);
}