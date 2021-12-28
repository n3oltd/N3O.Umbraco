using Microsoft.Extensions.Logging;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Templates.Handlebars.Helpers;

public class FormatBoolHelper : FormattingHelper<bool?> {
    public FormatBoolHelper(ILogger logger, IJsonProvider jsonProvider, IFormatter formatter)
        : base(logger, jsonProvider, formatter, "format_bool") { }

    protected override string Format(bool? value, string specifier) {
        if (value.HasValue() && specifier.HasValue()) {
            if (specifier.ToLowerInvariant().IsAnyOf("yes/no", "y/n")) {
                return value?.ToYesNoString(Formatter.Text);
            }
            
            if (specifier.ToLowerInvariant().IsAnyOf("true/false", "t/f")) {
                return value?.ToTrueFalseString(Formatter.Text);
            }
        }
        
        return base.Format(value, specifier);
    }
}
