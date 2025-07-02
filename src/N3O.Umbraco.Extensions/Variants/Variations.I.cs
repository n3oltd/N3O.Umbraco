using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Variants;

public interface IVariations {
    VariationContext CurrentContext { get; }
    string CurrentCulture { get; }
    ILanguage CurrentLanguage { get; }
    string DefaultCulture { get; }
    ILanguage DefaultLanguage { get; }
}