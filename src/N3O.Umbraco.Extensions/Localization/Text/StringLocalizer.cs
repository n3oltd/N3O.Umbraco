using Humanizer;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Locks;
using N3O.Umbraco.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Localization {
    public class StringLocalizer : IStringLocalizer {
        private static readonly ConcurrentDictionary<string, Guid> GuidCache = new ();
        private static readonly ConcurrentDictionary<string, string> StringCache = new ();
        private static readonly string ResourcesAlias = AliasHelper<TextContainerContent>.PropertyAlias(x => x.Resources);
        private static readonly string TextContainerAlias = AliasHelper<TextContainerContent>.ContentTypeAlias();

        private readonly IContentService _contentService;
        private readonly IContentCache _contentCache;
        private readonly ILock _lock;

        public StringLocalizer(IContentService contentService, IContentCache contentCache, ILock @lock) {
            _contentService = contentService;
            _contentCache = contentCache;
            _lock = @lock;
        }

        public void Flush(IEnumerable<string> aliases) {
            if (aliases.ContainsAny(new[] { ResourcesAlias, TextContainerAlias }, true)) {
                Lock<None>(() => {
                    GuidCache.Clear();
                    StringCache.Clear();
                    
                    return None.Empty;
                });
            }
        }

        public string Get(string folder, string name, string text) {
            return Lock(() => {
                var cacheKey = CacheKey.Generate<StringLocalizer>(nameof(Get), folder, name, text);

                return StringCache.GetOrAdd(cacheKey, _ => {
                    var folderId = GetOrCreateFolderId(folder);
                    var dictionaryId = GetOrCreateTextContainerId(folderId, name);
                    var textResource = CreateOrUpdateResource(dictionaryId, text);

                    return textResource.Value;
                });
            });
        }

        private Guid GetOrCreateFolderId(string folder) {
            var cacheKey = CacheKey.Generate<StringLocalizer>(nameof(GetOrCreateFolderId), folder);

            return GuidCache.GetOrAdd(cacheKey, _ => {
                var textFolderId = _contentCache.Single<TextFolderContent>(x => x.Content.Name.EqualsInvariant(folder))
                                                ?.Content
                                                .Key;

                if (textFolderId == null) {
                    textFolderId = CreateFolder(folder);
                }

                return textFolderId.Value;
            });
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
            var cacheKey = CacheKey.Generate<StringLocalizer>(nameof(GetOrCreateTextContainerId), folderId, name);

            return GuidCache.GetOrAdd(cacheKey, _ => {
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
            });
        }

        private Guid CreateContainer(string name, Guid folderId) {
            var content = _contentService.Create<TextContainerContent>(name, folderId);

            _contentService.SaveAndPublish(content);

            return content.Key;
        }

        private TextResource CreateOrUpdateResource(Guid containerId, string text) {
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
        }

        private T Lock<T>(Func<T> action) {
            return _lock.LockAsync(nameof(StringLocalizer), () => {
                var result = action();

                return Task.FromResult(result);
            }).GetAwaiter().GetResult();
        }
    }
}