using AsyncKeyedLock;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Extensions;

namespace N3O.Umbraco.Localization;

public class ReadOnlyStringLocalizer : ContentStringLocalizer {
    public ReadOnlyStringLocalizer(ILocalizationSettingsAccessor localizationSettingsAccessor,
                                   IContentCache contentCache,
                                   AsyncKeyedLocker<string> locker)
        : base(contentCache, localizationSettingsAccessor, locker) { }

    protected override string GetText(string folderName, string name, string text) {
        var folder = GetFolder(folderName);

        if (folder.HasValue()) {
            var textContainer = GetTextContainer(folder, name);

            if (textContainer.HasValue()) {
                var textResource = GetResource(textContainer, text);

                if (textResource.HasValue()) {
                    return textResource.Value;
                }
            }
        }
        
        return null;
    }

    private TextContainerFolderContent GetFolder(string folderName) {
        var folder = TextSettingsContent.Descendants()
                                        .FirstOrDefault(x => x.ContentType.Alias == TextContainerFolderAlias &&
                                                             x.Name.EqualsInvariant(folderName));

        return folder?.As<TextContainerFolderContent>();
    }

    private TextContainerContent GetTextContainer(TextContainerFolderContent textContainerFolderContent, string name) {
        name = NormalizeContainerName(name);
        
        var textContainer = textContainerFolderContent.Content()
                                                      .Descendants()
                                                      .SingleOrDefault(x => x.ContentType.Alias == TextContainerAlias &&
                                                                            x.Name.EqualsInvariant(name));
        
        return textContainer?.As<TextContainerContent>();
    }

    private TextResource GetResource(TextContainerContent textContainerContent, string text) {
        var resources = GetTextResources(textContainerContent);

        return resources.Single(x => x.Source.EqualsInvariant(text));
    }
    
    
    private IEnumerable<TextResource> GetTextResources(TextContainerContent textContainerContent) {
        return textContainerContent.OrEmpty(x => x.Resources);
    }
}
