using AsyncKeyedLock;
using Humanizer;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Extensions;

namespace N3O.Umbraco.Localization;

public class StringLocalizer : IStringLocalizer {
    private const string EnglishUS = "en-US";
    
    private static readonly ConcurrentDictionary<string, Guid> GuidCache = new ();
    private static readonly ConcurrentDictionary<string, string> StringCache = new ();
    private static readonly string ResourcesAlias = AliasHelper<TextContainerContent>.PropertyAlias(x => x.Resources);
    private static readonly string TextContainerAlias = AliasHelper<TextContainerContent>.ContentTypeAlias();
    private static readonly string TextContainerFolderAlias = AliasHelper<TextContainerFolderContent>.ContentTypeAlias();
    private static readonly string TextSettingsContentAlias = AliasHelper<TextSettingsContent>.ContentTypeAlias();
    private readonly IContentService _contentService;
    private readonly IUmbracoContextAccessor _umbracoContextAccessor;
    private readonly AsyncKeyedLocker<string> _locker;
    private readonly IVariationContextAccessor _variationContextAccessor;

    public StringLocalizer(IContentService contentService,
                           IUmbracoContextAccessor umbracoContextAccessor,
                           AsyncKeyedLocker<string> locker,
                           IVariationContextAccessor variationContextAccessor) {
        _contentService = contentService;
        _umbracoContextAccessor = umbracoContextAccessor;
        _locker = locker;
        _variationContextAccessor = variationContextAccessor;
    }

    public void Flush(IEnumerable<string> aliases) {
        if (aliases.ContainsAny(new[] { TextContainerAlias, TextContainerFolderAlias, TextSettingsContentAlias }, true)) {
            GuidCache.Clear();
            StringCache.Clear();
        }
    }

    public string Get(string folder, string name, string text) {
        return Lock(() => {
            var cacheKey = GetCacheKey(nameof(Get), folder, name, text);

            return StringCache.GetOrAdd(cacheKey, _ => {
                var folderId = GetOrCreateFolderId(folder);
                var dictionaryId = GetOrCreateTextContainerId(folderId, name);
                var textResource = CreateOrUpdateResource(dictionaryId, text);

                return textResource.Value;
            });
        });
    }

    private Guid GetOrCreateFolderId(string folder) {
        var cacheKey = GetCacheKey(nameof(GetOrCreateFolderId), folder);

        return GuidCache.GetOrAdd(cacheKey, _ => {
            var folderId = Run(u => GetEnglishUSByContentType(u, TextContainerFolderAlias))
                          .SingleOrDefault(x => x.Name.EqualsInvariant(folder))?.Key;

            if (folderId == null) {
                folderId = CreateFolder(folder);
            }

            return folderId.Value;
        });
    }

    private Guid CreateFolder(string name) {
        var textSettings = Run(u => GetEnglishUSByContentType(u, TextSettingsContentAlias)).SingleOrDefault();

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
            
            var container = Run(u => GetEnglishUSByContentType(u, TextContainerAlias))
                            .SingleOrDefault(x => x.Name(_variationContextAccessor, EnglishUS).EqualsInvariant(name) &&
                                                  x.Parent.Key == folderId);

            Guid containerId;

            if (container != null && container.IsInvariantOrHasCulture(CurrentCultureCode)) {
                containerId = container.Key;
            } else {
                containerId = CreateContainerOrAddCulture(container, name, folderId);
            }

            return containerId;
        });
    }

    private Guid CreateContainerOrAddCulture(IPublishedContent container, string name, Guid folderId) {
        var content = container == null 
                          ? _contentService.Create<TextContainerContent>(name, folderId) 
                          : _contentService.GetById(container.Id);

        if (content.ContentType.VariesByCulture()) {
            content.SetCultureName(name, EnglishUS);
            content.SetCultureName(name, CurrentCultureCode);
        }

        _contentService.SaveAndPublish(content);

        return content.Key;
    }

    private TextResource CreateOrUpdateResource(Guid containerId, string text) {
        var container = Run(u => u.GetContentCache().GetById(containerId).As<TextContainerContent>());

        var resources = container.Resources.OrEmpty().ToList();

        var resource = resources.SingleOrDefault(x => x.Source.EqualsInvariant(text));

        if (resource == null) {
            resource = new TextResource();
            resource.Source = text;

            resources.Add(resource);

            resources.Sort((x, y) => x.Source.CompareInvariant(y.Source));

            var json = JsonConvert.SerializeObject(resources);
            var content = _contentService.GetById(container.Content().Id);

            if (content.ContentType.VariesByCulture()) {
                content.SetValue(ResourcesAlias, json, CurrentCultureCode);
            } else {
                content.SetValue(ResourcesAlias, json);
            }

            _contentService.SaveAndPublish(content);
        }

        return resource;
    }
    
    private IEnumerable<IPublishedContent> GetEnglishUSByContentType(IUmbracoContextAccessor umbracoContextAccessor,
                                                                     string contentTypeAlias) {
        return umbracoContextAccessor.GetContentCache()
                                     .GetAtRoot()
                                     .SelectMany(x => x.DescendantsOrSelfOfType(_variationContextAccessor,
                                                                                contentTypeAlias,
                                                                                EnglishUS));
    }

    private T Lock<T>(Func<T> action) {
        using (_locker.Lock(LockKey.Generate<StringLocalizer>())) {
            try {
                var result = action();

                return result;
            } catch {
                return default;
            }
        }
    }

    private T Run<T>(Func<IUmbracoContextAccessor, T> func) {
        return func(_umbracoContextAccessor);
    }

    private static string GetCacheKey(params object[] values) {
        var newValues = CurrentCultureCode.Yield().Concat(values).ToArray();
        
        return CacheKey.Generate<StringComparer>(newValues);
    }
    
    private static string CurrentCultureCode => Thread.CurrentThread.CurrentCulture.IetfLanguageTag;
}
