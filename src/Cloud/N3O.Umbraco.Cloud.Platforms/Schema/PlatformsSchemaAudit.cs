using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.ContentTypes;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace N3O.Umbraco.Cloud.Platforms;

// GetValue returns default when a property is absent, so a content class bound to a type that does not exist
// reads as empty forever rather than throwing. Reporting that once at boot is the only signal there is.
// Only the aliases the seeder owns are reported: a type that reaches a site some other way is not a gap this
// package can close, and logging it on every boot forever only teaches a reader to ignore the log
public class PlatformsSchemaAudit : IPlatformsSchemaAudit {
    private readonly IContentTypeEditor _contentTypeEditor;
    private readonly IPlatformsContentTypeSeeder _contentTypeSeeder;

    public PlatformsSchemaAudit(IContentTypeEditor contentTypeEditor,
                                IPlatformsContentTypeSeeder contentTypeSeeder) {
        _contentTypeEditor = contentTypeEditor;
        _contentTypeSeeder = contentTypeSeeder;
    }

    public IReadOnlyList<string> FindGaps() {
        var gaps = new List<string>();
        var owned = _contentTypeSeeder.Aliases;

        foreach (var contentClass in GetContentClasses()) {
            var alias = AliasHelper.ContentTypeAlias(contentClass);

            if (owned.Contains(alias, StringComparer.InvariantCultureIgnoreCase) &&
                _contentTypeEditor.Find(alias) == null) {
                gaps.Add($"{contentClass.Name} binds content type {alias.Quote()} which does not exist");
            }
        }

        return gaps;
    }

    private IReadOnlyList<Type> GetContentClasses() {
        return typeof(PlatformsSchemaAudit).Assembly
                                           .GetTypes()
                                           .Where(x => x.GetCustomAttribute<UmbracoContentAttribute>() != null)
                                           .OrderBy(x => x.Name)
                                           .ToList();
    }
}
