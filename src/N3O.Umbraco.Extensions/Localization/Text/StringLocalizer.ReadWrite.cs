using AsyncKeyedLock;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;

namespace N3O.Umbraco.Localization;

public class ReadWriteStringLocalizer : ContentStringLocalizer {
    private readonly ILocalizationSettingsAccessor _localizationSettingsAccessor;
    private readonly IContentService _contentService;
    private readonly IContentTypeService _contentTypeService;
    private readonly ICoreScopeProvider _coreScopeProvider;
    private readonly ConcurrentDictionary<string, int> _cache = new();
    private string _defaultCultureCode;
    
    public ReadWriteStringLocalizer(ILocalizationSettingsAccessor localizationSettingsAccessor,
                                    IContentCache contentCache,
                                    IContentService contentService,
                                    IContentTypeService contentTypeService,
                                    ICoreScopeProvider coreScopeProvider,
                                    AsyncKeyedLocker<string> locker)
        : base(contentCache, localizationSettingsAccessor, locker) {
        _localizationSettingsAccessor = localizationSettingsAccessor;
        _contentService = contentService;
        _contentTypeService = contentTypeService;
        _coreScopeProvider = coreScopeProvider;
    }

    protected override void OnFlush() {
        _cache.Clear();
    }

    protected override string GetText(string folderName, string name, string text) {
        var folderId = GetOrCreateFolderId(folderName);
        var textContainerId = GetOrCreateTextContainerId(folderId, name);
        var textResource = GetOrCreateResource(textContainerId, text);

        return textResource.Value;
    }

    private int GetOrCreateFolderId(string folderName) {
        var cacheKey = GetIntCacheKey(nameof(GetOrCreateFolderId), folderName);

        return _cache.GetOrAddAtomic(cacheKey, () => {
            var folderId = GetContentId(TextContainerFolderAlias, x => x.Name.EqualsInvariant(folderName));

            if (folderId == null) {
                folderId = Locker.ExecuteLocked(() => CreateFolder(folderName), folderName);
            }

            return folderId.GetValueOrThrow();
        });
    }

    private int CreateFolder(string name) {
        var content = _contentService.Create<TextContainerFolderContent>(name, TextSettingsContent.Id);
        
        using var scope = _coreScopeProvider.CreateCoreScope(autoComplete: true);
                    
        using (_ = scope.Notifications.Suppress()) {
            _contentService.SaveAndPublish(content);

            scope.Complete();
        }

        return content.Id;
    }

    private int GetOrCreateTextContainerId(int folderId, string name) {
        var cacheKey = GetIntCacheKey(nameof(GetOrCreateTextContainerId), folderId, name);

        return _cache.GetOrAddAtomic(cacheKey, () => {
            name = NormalizeContainerName(name);

            var parent = _contentService.GetById(folderId);
            
            var containerId = GetContentId(TextContainerAlias,
                                           x => x.Name.EqualsInvariant(name) && parent?.Id == folderId);

            containerId = Locker.ExecuteLocked(() => EnsureContainerExistsForAllCultures(name, folderId, containerId),
                                               folderId,
                                               name);

            return containerId.GetValueOrThrow();
        });
    }

    private int EnsureContainerExistsForAllCultures(string name, int folderId, int? containerId) {
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

    private TextResource GetOrCreateResource(int containerId, string text) {
        return Locker.ExecuteLocked(() => {
            var containerContent = _contentService.GetById(containerId);
            var shouldSave = EnsureResourceExistsForEachCulture(containerContent, text);
        
            if (shouldSave) {
                lock (this) {
                    using var scope = _coreScopeProvider.CreateCoreScope(autoComplete: true);
                    
                    using (_ = scope.Notifications.Suppress()) {
                        _contentService.SaveAndPublish(containerContent);

                        scope.Complete();
                    }
                }
            }

            var culture = containerContent.ContentType.VariesByCulture() ? LocalizationSettings.CultureCode : null;

            var resources = GetTextResources(containerContent, culture);

            return resources.Single(x => x.Source.EqualsInvariant(text));
            
        }, containerId);
    }

    private bool EnsureResourceExistsForEachCulture(IContent containerContent, string text) {
        var shouldSave = false;

        if (containerContent.ContentType.VariesByCulture()) {
            var localizationSettings = _localizationSettingsAccessor.GetSettings();
            
            foreach (var culture in localizationSettings.AllCultureCodes) {
                var resources = GetTextResources(containerContent, culture);
                
                shouldSave |= CreateResourceIfNotExists(containerContent, resources, text, culture);
            }
        } else {
            var resources = GetTextResources(containerContent);

            shouldSave |= CreateResourceIfNotExists(containerContent, resources, text, null);
        }

        return shouldSave;
    }

    private bool CreateResourceIfNotExists(IContent containerContent,
                                           IEnumerable<TextResource> existingResources,
                                           string text,
                                           string culture) {
        var resources = existingResources.OrEmpty().ToList();
        var resource = resources.SingleOrDefault(x => x.Source.EqualsInvariant(text));
        
        if (!resource.HasValue()) {
            resource = new TextResource();
            resource.Source = text;
        
            resources.Add(resource);
            resources.Sort((x, y) => StringComparer.InvariantCultureIgnoreCase.Compare(x.Source, y.Source));
    
            var json = JsonConvert.SerializeObject(resources);
            
            containerContent.SetValue(ResourcesAlias, json, culture);

            return true;
        } else {
            return false;
        }
    }

    private int? GetContentId(string contentTypeAlias, Func<IContent, bool> predicate) {
        var contents = _contentService.GetDescendantsForContentOfType(_contentTypeService,
                                                                     _coreScopeProvider,
                                                                     TextSettingsContent.Id,
                                                                     contentTypeAlias).ToList();

        return contents.SingleOrDefault(predicate)?.Id;
    }
    
    private string GetIntCacheKey(params object[] values) {
        var newValues = values.OrEmpty().Concat(DefaultCultureCode).ToArray();

        return CacheKey.Generate<ReadWriteStringLocalizer>(newValues);
    }
    
    private IEnumerable<TextResource> GetTextResources(IContent content, string culture = null) {
        var propertyValue = content.GetValue<string>(AliasHelper<TextContainerContent>.PropertyAlias(x => x.Resources),
                                                     culture);

        if (propertyValue.HasValue()) {
            return JsonConvert.DeserializeObject<IEnumerable<TextResource>>(propertyValue);
        } else {
            return [];
        }
    }
    
    private string DefaultCultureCode {
        get {
            _defaultCultureCode ??= _localizationSettingsAccessor.GetSettings().DefaultCultureCode;

            return _defaultCultureCode;
        }
    }
}
