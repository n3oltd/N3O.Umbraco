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
    private static readonly ConcurrentDictionary<string, Guid> GuidCache = new();
    private static readonly ConcurrentDictionary<string, string> StringCache = new();
    private static readonly string ResourcesAlias = AliasHelper<TextContainerContent>.PropertyAlias(x => x.Resources);
    private static readonly string TextContainerAlias = AliasHelper<TextContainerContent>.ContentTypeAlias();
    private static readonly string TextContainerFolderAlias = AliasHelper<TextContainerFolderContent>.ContentTypeAlias();
    private static readonly string TextSettingsContentAlias = AliasHelper<TextSettingsContent>.ContentTypeAlias();
    
    private readonly ILocalizationSettingsAccessor _localizationSettingsAccessor;
    private readonly Lazy<IContentService> _contentService;
    private readonly Lazy<IContentTypeService> _contentTypeService;
    private readonly Lazy<ICoreScopeProvider> _coreScopeProvider;
    private readonly AsyncKeyedLocker<string> _locker;
    private string _defaultCultureCode;
    private IContent _textSettingsContent;

    public StringLocalizer(ILocalizationSettingsAccessor localizationSettingsAccessor,
                           Lazy<IContentService> contentService,
                           Lazy<ICoreScopeProvider> coreScopeProvider,
                           AsyncKeyedLocker<string> locker,
                           Lazy<IContentTypeService> contentTypeService) {
        _localizationSettingsAccessor = localizationSettingsAccessor;
        _contentService = contentService;
        _coreScopeProvider = coreScopeProvider;
        _locker = locker;
        _contentTypeService = contentTypeService;
    }

    public void Flush(IEnumerable<string> aliases) {
        if (aliases.ContainsAny(new[] { TextContainerAlias, TextContainerFolderAlias, TextSettingsContentAlias }, true)) {
            GuidCache.Clear();
            StringCache.Clear();
        }
    }

    public string Get(string folder, string name, string text) {
        if (!_localizationSettingsAccessor.GetSettings().AllCultureCodes.Contains(LocalizationSettings.CultureCode)) {
            return text;
        }
        
        return Lock(() => {
            try {
                var cacheKey = GetCacheKey(nameof(Get), folder, name, text);

                return StringCache.GetOrAdd(cacheKey, _ => {
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

    private Guid GetOrCreateFolderId(string folder) {
        var cacheKey = GetGuidCacheKey(nameof(GetOrCreateFolderId), folder);

        return GuidCache.GetOrAdd(cacheKey, _ => {
            var folderId = AllTextContainersOrFoldersWithAlias(TextContainerFolderAlias).SingleOrDefault(x => x.Name.EqualsInvariant(folder))?.Key;

            if (folderId == null) {
                folderId = Lock(() => {
                    folderId = CreateFolder(folder);

                    return folderId;
                }, folder);
            }

            return folderId.Value;
        });
    }

    private Guid CreateFolder(string name) {
        if (TextSettingsContent == null) {
            throw new Exception($"Could not find {nameof(TextSettingsContent)} content");
        }

        var content = _contentService.Value.Create<TextContainerFolderContent>(name, TextSettingsContent.Id);

        _contentService.Value.SaveAndPublish(content);

        return content.Key;
    }

    private Guid GetOrCreateTextContainerId(Guid folderId, string name) {
        var cacheKey = GetGuidCacheKey(nameof(GetOrCreateTextContainerId), folderId, name);

        return GuidCache.GetOrAdd(cacheKey, _ => {
            if (name.Contains("\\")) {
                name = name.Split('\\').Last();
            }

            name = name.Pascalize();

            var parent = _contentService.Value.GetById(folderId);
            
            var container = AllTextContainersOrFoldersWithAlias(TextContainerAlias).SingleOrDefault(x => x.Name.EqualsInvariant(name) && parent?.Key == folderId);

            if (container == null) {
                container = Lock(() => {
                    container = EnsureContainerExistsForEachCulture(name, folderId);

                    return container;
                }, folderId, name);
            }

            return container.Key;
        });
    }

    private IContent EnsureContainerExistsForEachCulture(string name, Guid folderId) {
        var containerContent = _contentService.Value.Create<TextContainerContent>(name, folderId);

        if (containerContent.ContentType.VariesByCulture()) {
            var localizationSettings = _localizationSettingsAccessor.GetSettings();
            
            foreach (var culture in localizationSettings.AllCultureCodes) {
                containerContent.SetCultureName(name, culture);
            }
        }
        
        _contentService.Value.SaveAndPublish(containerContent);
        
        return containerContent;
    }

    private TextResource GetOrCreateResource(Guid containerId, string text) {
        var containerContent = _contentService.Value.GetById(containerId);
        
        EnsureResourceExistsForEachCulture(containerContent, text);
        
        var resources = GetTextResources(containerContent, LocalizationSettings.CultureCode);
        
        return resources.Single(x => x.Source.EqualsInvariant(text));
    }

    private void EnsureResourceExistsForEachCulture(IContent containerContent, string text) {
        if (containerContent.ContentType.VariesByCulture()) {
            var localizationSettings = _localizationSettingsAccessor.GetSettings();
            
            foreach (var culture in localizationSettings.AllCultureCodes) {
                var resources = GetTextResources(containerContent, culture);
                
                CreateResourceIfNotExists(containerContent, resources, text, culture);
            }
        } else {
            var resources = GetTextResources(containerContent);
                
            CreateResourceIfNotExists(containerContent, resources, text, null);
        }
    }

    private void CreateResourceIfNotExists(IContent containerContent, IEnumerable<TextResource> existingResources, string text, string culture) {
        var resources = existingResources.OrEmpty().ToList();
        var resource = resources.SingleOrDefault(x => x.Source.EqualsInvariant(text));
        
        if (!resource.HasValue()) {
             var content = Lock(() => {
                resource = new TextResource();
                resource.Source = text;
            
                resources.Add(resource);
        
                var json = JsonConvert.SerializeObject(resources);
                var content = _contentService.Value.GetById(containerContent.Id);

                content.SetValue(ResourcesAlias, json, culture);
                
                _contentService.Value.SaveAndPublish(content, culture: culture);

                return content;
             }, containerContent.Key, text);
        }
    }

    private IEnumerable<IContent> AllTextContainersOrFoldersWithAlias(string contentTypeAlias) {
        var contents = _contentService.Value.GetDescendantsForContentOfType(_contentTypeService.Value,
                                                                            _coreScopeProvider.Value,
                                                                            TextSettingsContent,
                                                                            contentTypeAlias)
                                      .ToList();
        
        return contents;
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
    
    private string GetGuidCacheKey(params object[] values) {
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
    
    private IContent TextSettingsContent {
        get {
            if (_textSettingsContent == null) {
                var rootSettings =  _contentService.Value.GetRootSettings();

                _textSettingsContent = _contentService.Value.GetDescendantsForContentOfType(_contentTypeService.Value,
                                                                                            _coreScopeProvider.Value,
                                                                                            rootSettings,
                                                                                            TextSettingsContentAlias)
                                                      .Single();
            }
            
            return _textSettingsContent;
        }
    }
    
    public static IStringLocalizer Instance { get; } = StaticServiceProvider.Instance.GetRequiredService<IStringLocalizer>();
}
