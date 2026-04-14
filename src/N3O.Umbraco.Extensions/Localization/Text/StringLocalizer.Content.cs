using AsyncKeyedLock;
using Humanizer;
using N3O.Umbraco.Content;
using N3O.Umbraco.Dev;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Localization;

public abstract class ContentStringLocalizer : IStringLocalizer {
    protected static readonly string ResourcesAlias = AliasHelper<TextContainerContent>.PropertyAlias(x => x.Resources);
    protected static readonly string TextContainerAlias = AliasHelper<TextContainerContent>.ContentTypeAlias();
    protected static readonly string TextContainerFolderAlias = AliasHelper<TextContainerFolderContent>.ContentTypeAlias();
    private static readonly string TextSettingsContentAlias = AliasHelper<TextSettingsContent>.ContentTypeAlias();
    
    private readonly ConcurrentDictionary<string, string> _stringCache = new();
    private readonly ILocalizationSettingsAccessor _localizationSettingsAccessor;
    private IPublishedContent _textSettingsContent;

    protected ContentStringLocalizer(IContentCache contentCache,
                                     ILocalizationSettingsAccessor localizationSettingsAccessor,
                                     AsyncKeyedLocker<string> locker) {
        _localizationSettingsAccessor = localizationSettingsAccessor;

        ContentCache = contentCache;
        Locker = locker;
    }

    public void Flush(IEnumerable<string> aliases) {
        if (aliases.ContainsAny([TextContainerAlias, TextContainerFolderAlias, TextSettingsContentAlias], true)) {
            _stringCache.Clear();

            OnFlush();
        }
    }

    public string Get(string folderName, string name, string text) {
        if (DevFlags.IsSet(DevFlags.DisableTextLocalization)) {
            return text;
        }

        if (!_localizationSettingsAccessor.GetSettings().AllCultureCodes.Contains(LocalizationSettings.CultureCode)) {
            return text;
        }
        
        return Locker.ExecuteLocked(() => {
            try {
                var cacheKey = GetCacheKey(nameof(GetText), folderName, name, text);

                return _stringCache.GetOrAddAtomic(cacheKey, () => GetText(folderName, name, text) ?? text);
            } catch {
                return text;
            }
        });
    }
    
    protected  virtual void OnFlush() { }

    protected abstract string GetText(string folderName, string name, string text);

    private string GetCacheKey(params object[] values) {
        var newValues = values.OrEmpty().Concat(LocalizationSettings.CultureCode).ToArray();

        return CacheKey.Generate<ContentStringLocalizer>(newValues);
    }
    
    protected string NormalizeContainerName(string name) {
        if (name.Contains("\\")) {
            name = name.Split('\\').Last();
        }

        name = name.Pascalize();

        return name;
    }

    protected IPublishedContent TextSettingsContent {
        get {
            _textSettingsContent ??= ContentCache.Single(TextSettingsContentAlias);

            if (_textSettingsContent == null) {
                throw new Exception($"Could not find {nameof(TextSettingsContent)} content");
            }

            return _textSettingsContent;
        }
    }

    protected IContentCache ContentCache { get; }
    protected AsyncKeyedLocker<string> Locker { get; }
}
