using AsyncKeyedLock;
using Humanizer;
using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;

namespace N3O.Umbraco.Localization;

public class StringLocalizer : IStringLocalizer {
    private static readonly string ResourcesAlias = AliasHelper<TextContainerContent>.PropertyAlias(x => x.Resources);
    private static readonly string TextContainerAlias = AliasHelper<TextContainerContent>.ContentTypeAlias();
    private static readonly string TextContainerFolderAlias = AliasHelper<TextContainerFolderContent>.ContentTypeAlias();
    private static readonly string TextSettingsContentAlias = AliasHelper<TextSettingsContent>.ContentTypeAlias();
    
    private readonly ConcurrentDictionary<string, int> _intCache = new();
    private readonly ConcurrentDictionary<string, string> _stringCache = new();
    private readonly ILocalizationSettingsAccessor _localizationSettingsAccessor;
    private readonly IContentService _contentService;
    private readonly IContentTypeService _contentTypeService;
    private readonly ICoreScopeProvider _coreScopeProvider;
    private readonly AsyncKeyedLocker<string> _locker;
    private string _defaultCultureCode;
    private int? _textSettingsContentId;

    public StringLocalizer(ILocalizationSettingsAccessor localizationSettingsAccessor,
                           IContentService contentService,
                           IContentTypeService contentTypeService,
                           ICoreScopeProvider coreScopeProvider,
                           AsyncKeyedLocker<string> locker) {
        _localizationSettingsAccessor = localizationSettingsAccessor;
        _contentService = contentService;
        _contentTypeService = contentTypeService;
        _coreScopeProvider = coreScopeProvider;
        _locker = locker;
    }

    public void Flush(IEnumerable<string> aliases) {
        if (aliases.ContainsAny([TextContainerAlias, TextContainerFolderAlias, TextSettingsContentAlias], true)) {
            _intCache.Clear();
            _stringCache.Clear();
        }
    }

    public string Get(string folder, string name, string text) {
        if (!_localizationSettingsAccessor.GetSettings().AllCultureCodes.Contains(LocalizationSettings.CultureCode)) {
            return text;
        }
        
        return Lock(() => {
            try {
                var cacheKey = GetCacheKey(nameof(Get), folder, name, text);

                return _stringCache.GetOrAddAtomic(cacheKey, () => {
                    var folderId = GetOrCreateFolderId(folder);
                    var textContainerId = GetOrCreateTextContainerId(folderId, name);
                    var textResource = GetOrCreateResource(textContainerId, text);

                    return textResource.Value;
                });
            } catch {
                return text;
            }
        });
    }

    private int GetOrCreateFolderId(string folder) {
        var cacheKey = GetIntCacheKey(nameof(GetOrCreateFolderId), folder);

        return _intCache.GetOrAddAtomic(cacheKey, () => {
            var folderId = GetContentId(TextContainerFolderAlias, x => x.Name.EqualsInvariant(folder));

            if (folderId == null) {
                folderId = Lock(() => CreateFolder(folder), folder);
            }

            return folderId.GetValueOrThrow();
        });
    }

    private int CreateFolder(string name) {
        var content = _contentService.Create<TextContainerFolderContent>(name, TextSettingsContentId);

        _contentService.SaveAndPublish(content);

        return content.Id;
    }

    private int GetOrCreateTextContainerId(int folderId, string name) {
        var cacheKey = GetIntCacheKey(nameof(GetOrCreateTextContainerId), folderId, name);

        return _intCache.GetOrAddAtomic(cacheKey, () => {
            if (name.Contains("\\")) {
                name = name.Split('\\').Last();
            }

            name = name.Pascalize();

            var parent = _contentService.GetById(folderId);
            
            var containerId = GetContentId(TextContainerAlias,
                                           x => x.Name.EqualsInvariant(name) && parent?.Id == folderId);

            containerId = Lock(() => EnsureContainerExistsForAllCultures(name, folderId, containerId), folderId, name);

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
            _contentService.SaveAndPublish(containerContent);
        }
        
        return containerContent.Id;
    }

    private TextResource GetOrCreateResource(int containerId, string text) {
        return Lock(() => {
            var containerContent = _contentService.GetById(containerId);
            var shouldSave = EnsureResourceExistsForEachCulture(containerContent, text);
        
            if (shouldSave) {
                lock (this) {
                    _contentService.SaveAndPublish(containerContent);
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
                                                                     TextSettingsContentId,
                                                                     contentTypeAlias).ToList();

        return contents.SingleOrDefault(predicate)?.Id;
    }

    private void Lock(Action action, params object[] values) {
        Lock<None>(() => {
            action();
            
            return None.Empty;
        }, values);
    }

    private T Lock<T>(Func<T> action, params object[] values) {
        using (_locker.Lock(LockKey.Generate<StringLocalizer>(values))) {
            var result = action();

            return result;
        }
    }

    private string GetCacheKey(params object[] values) {
        var newValues = values.OrEmpty().Concat(LocalizationSettings.CultureCode).ToArray();
        
        return CacheKey.Generate<StringLocalizer>(newValues);
    }
    
    private string GetIntCacheKey(params object[] values) {
        var newValues = values.OrEmpty().Concat(DefaultCultureCode).ToArray();
        
        return CacheKey.Generate<StringLocalizer>(newValues);
    }
    
    private IEnumerable<TextResource> GetTextResources(IContent content, string culture = null) {
        var propertyValue = content.GetValue<string>(AliasHelper<TextContainerContent>.PropertyAlias(x => x.Resources), culture);

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
    
    private int TextSettingsContentId {
        get {
            _textSettingsContentId = _contentService.GetSettingContent(_contentTypeService,
                                                                       _coreScopeProvider,
                                                                       TextSettingsContentAlias)
                                                    ?.Id;

            if (_textSettingsContentId == null) {
                throw new Exception($"Could not find {nameof(TextSettingsContent)} content");
            }
            
            return _textSettingsContentId.GetValueOrThrow();
        }
    }
    
    public static IStringLocalizer Instance { get; } = StaticServiceProvider.Instance.GetRequiredService<IStringLocalizer>();
}
