using HandlebarsDotNet;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Templates.Handlebars.Extensions;
using NodaTime;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Templates.Handlebars.Helpers;

public class FormatDateHelper : Helper {
    public FormatDateHelper(ILogger<FormatDateHelper> logger, IJsonProvider jsonProvider, IFormatter formatter)
        : base(logger, jsonProvider, 2) {
        _formatter = formatter;
    }

    private static readonly IReadOnlyDictionary<string, string> FormatPatterns = new Dictionary<string, string> {
        { "d/m/y", "dd/MM/yyyy" },
        { "d-m-y", "dd-MM-yyyy" },
        { "m/d/y", "MM/dd/yyyy" },
        { "m-d-y", "MM-dd-yyyy" },
    };

    private readonly IFormatter _formatter;

    protected override void Execute(EncodedTextWriter writer, HandlebarsDotNet.Context context, HandlebarsArguments args) {
        if (!args.TryGet(0, out LocalDate? date) &&
            !args.TryGet<LocalDateTime?, LocalDate?>(0, x => x?.Date, out date) &&
            !args.TryGet<DateTime?, LocalDate?>(0, x => x?.ToLocalDate(), out date)) {
            throw new Exception($"Could not convert argument 0 to a {nameof(LocalDate)}");
        }
    
        var formatSpecifier = args.Get<string>(1);
        var output = Format(date, formatSpecifier);
    
        writer.Write(output);
    }

    private string Format(LocalDate? value, string specifier) {
        if (value.HasValue()) {
            if (FormatPatterns.ContainsKey(specifier)) {
                specifier = FormatPatterns[specifier];
            }
        
            return _formatter.DateTime.FormatDate(value, specifier);
        }

        return null;
    }

    public override string Name => "formatDate";
}
