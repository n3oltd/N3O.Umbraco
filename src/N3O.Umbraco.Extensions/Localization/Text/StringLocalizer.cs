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
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Extensions;
using Enumerable = System.Linq.Enumerable;

namespace N3O.Umbraco.Localization;

public class StringLocalizer : IStringLocalizer {
    private static readonly ConcurrentDictionary<string, Guid> GuidCache = new();
    private static readonly ConcurrentDictionary<string, string> StringCache = new();
    private static readonly string ResourcesAlias = AliasHelper<TextContainerContent>.PropertyAlias(x => x.Resources);
    private static readonly string TextContainerAlias = AliasHelper<TextContainerContent>.ContentTypeAlias();
    private static readonly string TextContainerFolderAlias = AliasHelper<TextContainerFolderContent>.ContentTypeAlias();
    private static readonly string TextSettingsContentAlias = AliasHelper<TextSettingsContent>.ContentTypeAlias();
    
    private readonly ILocalizationSettingsAccessor _localizationSettingsAccessor;
    private readonly IContentService _contentService;
    private readonly IUmbracoContextAccessor _umbracoContextAccessor;
    private readonly AsyncKeyedLocker<string> _locker;
    private string _defaultCultureCode;

    public StringLocalizer(ILocalizationSettingsAccessor localizationSettingsAccessor,
                           IContentService contentService, 
                           IUmbracoContextAccessor umbracoContextAccessor,
                           AsyncKeyedLocker<string> locker) {
        _localizationSettingsAccessor = localizationSettingsAccessor;
        _contentService = contentService;
        _umbracoContextAccessor = umbracoContextAccessor;
        _locker = locker;
    }

    public void Flush(IEnumerable<string> aliases) {
        if (aliases.ContainsAny(new[] { TextContainerAlias, TextContainerFolderAlias, TextSettingsContentAlias }, true)) {
            GuidCache.Clear();
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
                    var textResource = CreateOrUpdateResource(textContainerId, text);

                    return textResource.Value;
                });
            } catch {
                return text;
            }
        });
    }

    private Guid GetOrCreateFolderId(string folder) {
        var cacheKey = GetCacheKey(nameof(GetOrCreateFolderId), folder);

        return GuidCache.GetOrAdd(cacheKey, _ => {
            var folderId = Run(u => AllContentWithAlias(u, TextContainerFolderAlias)).SingleOrDefault(x => x.Name.EqualsInvariant(folder))?.Key;

            if (folderId == null) {
                folderId = CreateFolder(folder);
            }

            return folderId.Value;
        });
    }

    private Guid CreateFolder(string name) {
        var textSettings = Run(u => AllContentWithAlias(u, TextSettingsContentAlias)).SingleOrDefault();

        if (textSettings == null) {
            throw new Exception($"Could not find {nameof(TextSettingsContent)} content");
        }

        var content = _contentService.Create<TextContainerFolderContent>(name, textSettings.Id);

        _contentService.SaveAndPublish(content);

        return content.Key;
    }

    private Guid GetOrCreateTextContainerId(Guid folderId, string name) {
        var cacheKey = GetCacheKey(nameof(GetOrCreateTextContainerId), folderId, name);

        return GuidCache.GetOrAdd(cacheKey, _ => {
            if (name.Contains("\\")) {
                name = name.Split('\\').Last();
            }

            name = name.Pascalize();
            
            var container = Run(u => AllContentWithAlias(u, TextContainerAlias)).SingleOrDefault(x => x.Name.EqualsInvariant(name) && x.Parent?.Key == folderId);

            if (container == null) {
                container = CreateContainer(name, folderId);
            }
            
            Guid containerId;

            if (container.IsInvariantOrHasCulture(LocalizationSettings.CultureCode)) {
                containerId = container.Key;
            } else {
                containerId = AddCurrentCultureToContainer(container, name);
            }

            return containerId;
        });
    }

    private IPublishedContent CreateContainer(string name, Guid folderId) {
        var containerContent = _contentService.Create<TextContainerContent>(name, folderId);

        _contentService.SaveAndPublish(containerContent);

        var publishedContainer = Run(u => u.GetContentCache().GetById(containerContent.Key));
        
        return publishedContainer;
    }
    
    private Guid AddCurrentCultureToContainer(IPublishedContent container, string name) {
        var content = _contentService.GetById(container.Id);

        content.SetCultureName(name, LocalizationSettings.CultureCode);

        _contentService.SaveAndPublish(content);

        return content.Key;
    }

    private TextResource CreateOrUpdateResource(Guid containerId, string text) {
        var containerContent = Run(u => u.GetContentCache().GetById(containerId).As<TextContainerContent>());
        var resources = containerContent.Resources.OrEmpty().ToList();

        var resource = resources.SingleOrDefault(x => x.Source.EqualsInvariant(text));

        if (resource == null) {
            resource = CreateResource(containerId, text);
        }

        return resource;
    }

    private TextResource CreateResource(Guid containerId, string text) {
        var variationContext = new VariationContext(DefaultCultureCode);
        var containerContent = Run(u => u.GetContentCache().GetById(containerId).As<TextContainerContent>(variationContext));
        var resources = containerContent.Resources.OrEmpty().ToList();
        
        var resource = new TextResource();
        resource.Source = text;

        resources.Add(resource);

        resources.Sort((x, y) => x.Source.CompareInvariant(y.Source));

        var json = JsonConvert.SerializeObject(resources);
        var content = _contentService.GetById(containerContent.Content().Id);

        if (content.ContentType.VariesByCulture()) {
            content.SetValue(ResourcesAlias, json, DefaultCultureCode);
        } else {
            content.SetValue(ResourcesAlias, json);
        }

        _contentService.SaveAndPublish(content);

        return resource;
    }

    private IEnumerable<IPublishedContent> AllContentWithAlias(IUmbracoContextAccessor umbracoContextAccessor,
                                                               string contentTypeAlias) {
        return umbracoContextAccessor.GetContentCache()
                                     .GetAtRoot()
                                     .SelectMany(x => x.DescendantsOrSelfOfType(contentTypeAlias,
                                                                                culture: DefaultCultureCode));
    }

    private T Lock<T>(Func<T> action) {
        using (_locker.Lock(LockKey.Generate<StringLocalizer>())) {
            var result = action();

            return result;
        }
    }

    private T Run<T>(Func<IUmbracoContextAccessor, T> func) {
        return func(_umbracoContextAccessor);
    }

    private string GetCacheKey(params object[] values) {
        var newValues = values.OrEmpty().Concat(LocalizationSettings.CultureCode);
        
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
