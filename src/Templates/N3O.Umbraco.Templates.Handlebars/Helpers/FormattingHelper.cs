using HandlebarsDotNet;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using N3O.Umbraco.Localization;
using System;

namespace N3O.Umbraco.Templates.Handlebars.Helpers;

public abstract class FormattingHelper<TValue> : Helper {
    protected FormattingHelper(ILogger logger,
                               IJsonProvider jsonProvider,
                               IFormatter formatter,
                               string name)
        : base(logger, jsonProvider, 2) {
        Name = name;
        Formatter = formatter;
    }

    public override string Name { get; }

    protected override void Execute(EncodedTextWriter writer, HandlebarsDotNet.Context context, HandlebarsArguments args) {
        var value = args.Get<TValue>(0);
        var formatSpecifier = args.Get<string>(1);
        
        var output = GenerateOutput(value, formatSpecifier);
    
        writer.Write(output);
    }

    protected virtual string Format(TValue value, string specifier) {
        return null;
    }

    private string GenerateOutput(TValue value, string formatSpecifier) {
        string output;

        try {
            output = this.CallMethod(formatSpecifier)
                         .WithParameter(typeof(TValue), value)
                         .Run<string>();
        } catch (MissingMethodException) {
            output = Format(value, formatSpecifier);
        }

        return output;
    }

    protected IFormatter Formatter { get; }
}
