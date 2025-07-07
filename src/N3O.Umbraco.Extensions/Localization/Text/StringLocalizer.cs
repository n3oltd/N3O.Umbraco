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
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
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
    private readonly Lazy<IPublishedContentCache> _publishedContentCache;
    private readonly Lazy<IUmbracoContextAccessor> _umbracoContextAccessor;
    private readonly AsyncKeyedLocker<string> _locker;
    private string _defaultCultureCode;

    public StringLocalizer(ILocalizationSettingsAccessor localizationSettingsAccessor,
                           Lazy<IContentService> contentService,
                           Lazy<IPublishedContentCache> publishedContentCache,
                           Lazy<IUmbracoContextAccessor> umbracoContextAccessor,
                           AsyncKeyedLocker<string> locker) {
        _localizationSettingsAccessor = localizationSettingsAccessor;
        _contentService = contentService;
        _publishedContentCache = publishedContentCache;
        _umbracoContextAccessor = umbracoContextAccessor;
        _locker = locker;
    }

    public void Flush(IEnumerable<string> aliases) {
        if (aliases.ContainsAny(new[] { TextContainerAlias, TextContainerFolderAlias, TextSettingsContentAlias }, true)) {
            //GuidCache.Clear();
            StringCache.Clear();
        }
    }

    public string Get(string folder, string name, string text) {
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
            var folderId = Run(u => AllContentWithAlias(u, TextContainerFolderAlias)).SingleOrDefault(x => x.Name.EqualsInvariant(folder))?.Key;

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
        var textSettings = Run(u => AllContentWithAlias(u, TextSettingsContentAlias)).SingleOrDefault();

        if (textSettings == null) {
            throw new Exception($"Could not find {nameof(TextSettingsContent)} content");
        }

        var content = _contentService.Value.Create<TextContainerFolderContent>(name, textSettings.Id);

        _contentService.Value.SaveAndPublish(content);
        
        _publishedContentCache.Value.WaitForContentToAppearInCache(content);

        return content.Key;
    }

    private Guid GetOrCreateTextContainerId(Guid folderId, string name) {
        var cacheKey = GetGuidCacheKey(nameof(GetOrCreateTextContainerId), folderId, name);

        return GuidCache.GetOrAdd(cacheKey, _ => {
            if (name.Contains("\\")) {
                name = name.Split('\\').Last();
            }

            name = name.Pascalize();
            
            var container = Run(u => AllContentWithAlias(u, TextContainerAlias)).SingleOrDefault(x => x.Name.EqualsInvariant(name) && x.Parent?.Key == folderId);

            if (container == null) {
                container = Lock(() => {
                    container = EnsureContainerExistsForEachCulture(name, folderId);

                    return container;
                }, folderId, name);
            }

            return container.Key;
        });
    }

    private IPublishedContent EnsureContainerExistsForEachCulture(string name, Guid folderId) {
        var containerContent = _contentService.Value.Create<TextContainerContent>(name, folderId);

        if (containerContent.ContentType.VariesByCulture()) {
            var localizationSettings = _localizationSettingsAccessor.GetSettings();
            
            foreach (var culture in localizationSettings.AllCultureCodes) {
                containerContent.SetCultureName(name, culture);
            }
        }
        
        _contentService.Value.SaveAndPublish(containerContent);
        
        _publishedContentCache.Value.WaitForContentToAppearInCache(containerContent);

        var publishedContainer = Run(u => u.GetContentCache().GetById(containerContent.Key));
        
        return publishedContainer;
    }

    private TextResource GetOrCreateResource(Guid containerId, string text) {
        var publishedContainer = Run(u => u.GetContentCache().GetById(containerId));
        
        EnsureResourceExistsForEachCulture(publishedContainer, text);
        
        var variationContext = new VariationContext(LocalizationSettings.CultureCode);
        var textContainerForCurrentCulture = Run(u => u.GetContentCache().GetById(containerId).As<TextContainerContent>(variationContext));
        
        return textContainerForCurrentCulture.Resources.Single(x => x.Source.EqualsInvariant(text));
    }

    private void EnsureResourceExistsForEachCulture(IPublishedContent publishedContainer, string text) {
        if (publishedContainer.ContentType.VariesByCulture()) {
            var localizationSettings = _localizationSettingsAccessor.GetSettings();
            
            foreach (var culture in localizationSettings.AllCultureCodes) {
                var variationContext = new VariationContext(culture);
                var publishedContentForCulture = publishedContainer.As<TextContainerContent>(variationContext);
                
                CreateResourceIfNotExists(publishedContentForCulture, text, culture);
            }
        } else {
            CreateResourceIfNotExists(publishedContainer.As<TextContainerContent>(), text, null);
        }
    }

    private void CreateResourceIfNotExists(TextContainerContent containerContent, string text, string culture) {
        var resources = containerContent.Resources.OrEmpty().ToList();
        var resource = resources.SingleOrDefault(x => x.Source.EqualsInvariant(text));
        
        if (!resource.HasValue()) {
             var content = Lock(() => {
                resource = new TextResource();
                resource.Source = text;
            
                resources.Add(resource);
        
                var json = JsonConvert.SerializeObject(resources);
                var content = _contentService.Value.GetById(containerContent.Content().Id);

                content.SetValue(ResourcesAlias, json, culture);
                
                _contentService.Value.SaveAndPublish(content, culture: culture);
                
                _publishedContentCache.Value.WaitForContentToAppearInCache(content);

                return content;
             }, containerContent.Content().Key, text);
        }
    }

    private IEnumerable<IPublishedContent> AllContentWithAlias(IUmbracoContextAccessor umbracoContextAccessor,
                                                               string contentTypeAlias) {
        return umbracoContextAccessor.GetContentCache()
                                     .GetAtRoot()
                                     .SelectMany(x => x.DescendantsOrSelfOfType(contentTypeAlias,
                                                                                culture: DefaultCultureCode));
    }

    private T Lock<T>(Func<T> action, params object[] values) {
        using (_locker.Lock(LockKey.Generate<StringLocalizer>(values))) {
            var result = action();

            return result;
        }
    }

    private T Run<T>(Func<IUmbracoContextAccessor, T> func) {
        return func(_umbracoContextAccessor.Value);
    }

    private string GetCacheKey(params object[] values) {
        var newValues = values.OrEmpty().Concat(LocalizationSettings.CultureCode).ToArray();
        
        return CacheKey.Generate<StringLocalizer>(newValues);
    }
    
    private string GetGuidCacheKey(params object[] values) {
        var newValues = values.OrEmpty().Concat(DefaultCultureCode).ToArray();
        
        return CacheKey.Generate<StringLocalizer>(newValues);
    }

    private string DefaultCultureCode {
        get {
            _defaultCultureCode ??= _localizationSettingsAccessor.GetSettings().DefaultCultureCode;
            
            return _defaultCultureCode;
        }
    }
    
    public static IStringLocalizer Instance { get; } = StaticServiceProvider.Instance.GetRequiredService<IStringLocalizer>();
}
