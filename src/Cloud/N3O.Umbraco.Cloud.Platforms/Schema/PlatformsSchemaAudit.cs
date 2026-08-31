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
// reads as empty forever rather than throwing. The aliases the seeder owns are checked here so that failure
// is loud once, at boot, instead of silent in every request.
// Only those aliases are reported. A class bound to a type that reaches a site some other way is not a gap
// this package can close, and a site that legitimately does not have that type would otherwise be told so on
// every boot forever, which is the fastest way to teach a reader to ignore the log.
// The check stops at the content type. Whether a given class property is read out of the content or computed
// in its getter is not visible to reflection, and the two are otherwise identical, so a property level check
// here cannot tell a genuinely missing property from a computed one. The seeder declares every property it
// owns, which makes it the authority on that question
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
