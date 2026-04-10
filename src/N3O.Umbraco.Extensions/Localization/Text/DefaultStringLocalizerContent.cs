using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;

namespace N3O.Umbraco.Localization;

public class DefaultStringLocalizerContent : IStringLocalizerContent {
    private readonly IContentService _contentService;
    private readonly ICoreScopeProvider _coreScopeProvider;
    private readonly ILocalizationSettingsAccessor _localizationSettingsAccessor;

    public DefaultStringLocalizerContent(IContentService contentService,
                                         ICoreScopeProvider coreScopeProvider,
                                         ILocalizationSettingsAccessor localizationSettingsAccessor) {
        _contentService = contentService;
        _coreScopeProvider = coreScopeProvider;
        _localizationSettingsAccessor = localizationSettingsAccessor;
    }

    public int CreateFolder(string name, int parentId) {
        var content = _contentService.Create<TextContainerFolderContent>(name, parentId);
        
        using var scope = _coreScopeProvider.CreateCoreScope(autoComplete: true);
                    
        using (_ = scope.Notifications.Suppress()) {
            _contentService.SaveAndPublish(content);

            scope.Complete();
        }

        return content.Id;
    }

    public int CreateTextContainer(string name, int folderId, int? containerId) {
        var containerContent = containerId.HasValue()
                                   ? _contentService.GetById(containerId.GetValueOrThrow())
                                   : _contentService.Create<TextContainerContent>(name, folderId);

        var shouldSave = !containerId.HasValue();

        if (containerContent.ContentType.VariesByCulture()) {
            var localizationSettings = _localizationSettingsAccessor.GetSettings();

            foreach (var culture in localizationSettings.AllCultureCodes) {
                if (!containerContent.IsCultureAvailable(culture)) {
                    containerContent.SetCultureName(name, culture);

                    shouldSave = true;
                }
            }
        }

        if (shouldSave) {
            using var scope = _coreScopeProvider.CreateCoreScope(autoComplete: true);
                    
            using (_ = scope.Notifications.Suppress()) {
                _contentService.SaveAndPublish(containerContent);

                scope.Complete();
            }
        }
        
        return containerContent.Id;
    }
}