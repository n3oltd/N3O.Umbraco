using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace N3O.Umbraco.Localization;

public class TextContainerContentValidator : ContentValidator {
    private static readonly string TextContainerAlias = AliasHelper<TextContainerContent>.ContentTypeAlias();
    private static readonly string ResourcesAlias = AliasHelper<TextContainerContent>.PropertyAlias(x => x.Resources);
    private static readonly Regex PlaceholderRegex = new(@"\{\{|\}\}|\{(\d+)\}", RegexOptions.Compiled);

    public TextContainerContentValidator(IContentHelper contentHelper) : base(contentHelper) { }

    public override bool IsValidator(ContentProperties content) {
        return content.ContentTypeAlias.EqualsInvariant(TextContainerAlias);
    }

    public override void Validate(ContentProperties content) {
        var json = content.GetPropertyValueByAlias<string>(ResourcesAlias);

        if (!json.HasValue()) {
            return;
        }

        var resources = JsonConvert.DeserializeObject<IEnumerable<TextResource>>(json).OrEmpty();

        foreach (var resource in resources) {
            if (resource.Custom.HasValue()) {
                var sourcePlaceholders = GetPlaceholders(resource.Source);
                var customPlaceholders = GetPlaceholders(resource.Custom);

                if (!sourcePlaceholders.SetEquals(customPlaceholders)) {
                    var expected = sourcePlaceholders.Any()
                                       ? string.Join(", ", sourcePlaceholders.OrderBy(x => x).Select(x => "{" + x + "}"))
                                       : "no placeholders";

                    ErrorResult($"The translation for {resource.Source.Quote()} must use the same placeholders as " +
                                $"the original text ({expected})");
                }
            }
        }
    }

    private static HashSet<string> GetPlaceholders(string text) {
        var placeholders = new HashSet<string>();

        foreach (Match match in PlaceholderRegex.Matches(text.Or(string.Empty))) {
            if (match.Groups[1].Success) {
                placeholders.Add(match.Groups[1].Value);
            }
        }

        return placeholders;
    }
}
