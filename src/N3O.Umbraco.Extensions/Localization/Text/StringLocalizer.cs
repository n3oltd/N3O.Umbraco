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
using Umbraco.Cms.Core.Web;

namespace N3O.Umbraco.Localization {
    public class StringLocalizer : IStringLocalizer {
        private static readonly ConcurrentDictionary<string, Guid> GuidCache = new ();
        private static readonly ConcurrentDictionary<string, string> StringCache = new ();
        private static readonly string ResourcesAlias = AliasHelper<TextContainerContent>.PropertyAlias(x => x.Resources);
        private static readonly string TextContainerAlias = AliasHelper<TextContainerContent>.ContentTypeAlias();
        private static readonly string TextContainerFolderAlias = AliasHelper<TextContainerFolderContent>.ContentTypeAlias();
        private static readonly string TextSettingsContentAlias = AliasHelper<TextSettingsContent>.ContentTypeAlias();

        private readonly IContentService _contentService;
        private readonly IUmbracoContextFactory _umbracoContextFactory;
        private readonly ILocker _locker;

        public StringLocalizer(IContentService contentService,
                               IUmbracoContextFactory umbracoContextFactory,
                               ILocker locker) {
            _contentService = contentService;
            _umbracoContextFactory = umbracoContextFactory;
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
                var folderId = Run(u => u.Content
                                         .GetByContentType(u.Content.GetContentType(TextContainerFolderAlias))
                                         .SingleOrDefault(x => x.Name.EqualsInvariant(folder))
                                         ?.Key);

                if (folderId == null) {
                    folderId = CreateFolder(folder);
                }

                return folderId.Value;
            });
        }

        private Guid CreateFolder(string name) {
            var textSettings = Run(u => u.Content
                                         .GetByContentType(u.Content.GetContentType(TextSettingsContentAlias))
                                         .SingleOrDefault());

            if (textSettings == null) {
                throw new Exception($"Could not find {nameof(TextSettingsContent)} content");
            }

            var content = _contentService.Create<TextContainerFolderContent>(name, textSettings.Id);

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
                
                var containerId = Run(u => u.Content
                                            .GetByContentType(u.Content.GetContentType(TextContainerAlias))
                                            .SingleOrDefault(x => x.Name.EqualsInvariant(name) &&
                                                                  x.Parent.Key == folderId)
                                            ?.Key);

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
            var container = Run(u => u.Content.GetById(containerId).As<TextContainerContent>());

            var resources = container.Resources.OrEmpty().ToList();

            var resource = resources.SingleOrDefault(x => x.Source.EqualsInvariant(text));

            if (resource == null) {
                resource = new TextResource();
                resource.Source = text;

                resources.Add(resource);

                resources.Sort((x, y) => x.Source.CompareInvariant(y.Source));

                var json = JsonConvert.SerializeObject(resources);
                var content = _contentService.GetById(container.Content().Id);

                content.SetValue(ResourcesAlias,json);

                _contentService.SaveAndPublish(content);
            }

            return resource;
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

        private T Run<T>(Func<IUmbracoContext, T> func) {
            using (var contextReference = _umbracoContextFactory.EnsureUmbracoContext()) {
                return func(contextReference.UmbracoContext);
            }
        }
    }
}