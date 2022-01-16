using Humanizer;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;

namespace N3O.Umbraco.Localization {
    public class StringLocalizer : IStringLocalizer {
        private static readonly string ResourcesAlias = AliasHelper<TextContainerContent>.PropertyAlias(x => x.Resources);
        private static readonly string TextContainerAlias = AliasHelper<TextContainerContent>.ContentTypeAlias();
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(30);
        private static readonly object Lock = new();

        private readonly IAppPolicyCache _appCache;
        private readonly IContentService _contentService;
        private readonly IContentCache _contentCache;

        public StringLocalizer(IAppPolicyCache appAppCache, IContentService contentService, IContentCache contentCache) {
            _appCache = appAppCache;
            _contentService = contentService;
            _contentCache = contentCache;
        }

        public void Flush(IEnumerable<string> aliases) {
            if (aliases.Contains(TextContainerAlias, true)) {
                _appCache.ClearByKey(nameof(StringLocalizer));   
            }
        }

        public string Get(string folder, string name, string text) {
            var cacheKey = nameof(StringLocalizer) + nameof(Get) + folder + name + text;

            return _appCache.GetCacheItem(cacheKey, () => {
                lock (Lock) {
                    var folderId = GetOrCreateFolderId(folder);
                    var dictionaryId = GetOrCreateTextContainerId(folderId, name);
                    var textResource = CreateOrUpdateResource(dictionaryId, text);

                    return textResource.Value;
                }
            }, CacheDuration, true);
        }

        private Guid GetOrCreateFolderId(string folder) {
            var cacheKey = nameof(StringLocalizer) + nameof(GetOrCreateFolderId) + folder;

            return _appCache.GetCacheItem(cacheKey, () => {
                var textFolderId = _contentCache.Single<TextFolderContent>(x => x.Content.Name.EqualsInvariant(folder))
                                                ?.Content
                                                .Key;

                if (textFolderId == null) {
                    textFolderId = CreateFolder(folder);
                }

                return textFolderId.Value;
            }, CacheDuration, true);
        }

        private Guid CreateFolder(string name) {
            var textSettings = _contentCache.Single<TextSettingsContent>();

            if (textSettings == null) {
                throw new Exception($"Could not find {nameof(TextSettingsContent)} content");
            }

            var content = _contentService.Create<TextFolderContent>(name, textSettings.Content.Id);

            _contentService.SaveAndPublish(content);

            return content.Key;
        }

        private Guid GetOrCreateTextContainerId(Guid folderId, string name) {
            var cacheKey = nameof(StringLocalizer) + nameof(GetOrCreateTextContainerId) + folderId + name;

            return _appCache.GetCacheItem(cacheKey, () => {
                if (name.Contains("\\")) {
                    name = name.Split('\\').Last();
                }

                name = name.Pascalize();

                var containerId = _contentCache.Single<TextContainerContent>(t => t.Content.Name.EqualsInvariant(name) &&
                                                                                  t.Content.Parent.Key == folderId)
                                               ?.Content
                                               .Key;

                if (containerId == null) {
                    containerId = CreateContainer(name, folderId);
                }

                return containerId.Value;
            }, CacheDuration, true);
        }

        private Guid CreateContainer(string name, Guid folderId) {
            var content = _contentService.Create<TextContainerContent>(name, folderId);

            _contentService.SaveAndPublish(content);

            return content.Key;
        }

        private TextResource CreateOrUpdateResource(Guid containerId, string text) {
            var cacheKey = nameof(StringLocalizer) + nameof(CreateOrUpdateResource) + containerId + text;

            return _appCache.GetCacheItem(cacheKey, () => {
                var container = _contentCache.Single<TextContainerContent>(x => x.Content.Key == containerId);

                var resources = container.Resources.OrEmpty().ToList();

                var resource = resources.SingleOrDefault(x => x.Source.EqualsInvariant(text));

                if (resource == null) {
                    resource = new TextResource();
                    resource.Source = text;

                    resources.Add(resource);

                    resources.Sort((x, y) => x.Source.CompareInvariant(y.Source));

                    var json = JsonConvert.SerializeObject(resources);
                    var content = _contentService.GetById(container.Content.Id);

                    content.SetValue(ResourcesAlias,json);

                    _contentService.SaveAndPublish(content);
                }

                return resource;
            }, CacheDuration, true);
        }
    }
}