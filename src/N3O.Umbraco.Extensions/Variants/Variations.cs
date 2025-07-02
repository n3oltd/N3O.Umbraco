using N3O.Umbraco.Extensions;
using System.Collections.Concurrent;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Variants;

public class Variations : IVariations {
    private static readonly ConcurrentDictionary<string, ILanguage> LanguagesCache = new();
    
    private readonly ILocalizationService _localizationService;
    private readonly IVariationContextAccessor _variationContextAccessor;
    private ILanguage _defaultLanguage;

    public Variations(ILocalizationService localizationService, IVariationContextAccessor variationContextAccessor) {
        _localizationService = localizationService;
        _variationContextAccessor = variationContextAccessor;
    }

    public VariationContext CurrentContext => _variationContextAccessor.VariationContext;
    
    public string CurrentCulture => _variationContextAccessor.VariationContext?.Culture ?? DefaultLanguage.IsoCode;

    public ILanguage CurrentLanguage {
        get {
            return LanguagesCache.GetOrAddAtomic(CurrentCulture,
                                                 () => _localizationService.GetLanguageByIsoCode(CurrentCulture));

        }
    }

    public string DefaultCulture => DefaultLanguage.IsoCode;

    public ILanguage DefaultLanguage {
        get {
            _defaultLanguage ??= _localizationService.GetAllLanguages().Single(x => x.IsDefault);

            return _defaultLanguage;
        }
    }

    public static VariationContext CreateVariationContext(string culture = null, string segment = null) {
        return new VariationContext(culture, segment);
    }
}