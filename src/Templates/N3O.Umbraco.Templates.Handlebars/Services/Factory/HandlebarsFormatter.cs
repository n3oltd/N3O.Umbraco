using HandlebarsDotNet;
using HandlebarsDotNet.IO;
using System;
using IHandlebarsFormatter = HandlebarsDotNet.IO.IFormatter;

namespace N3O.Umbraco.Templates.Handlebars;

public class HandlebarsFormatter : IHandlebarsFormatter, IFormatterProvider {
    private readonly ITemplateFormatter _templateFormatter;

    public HandlebarsFormatter(ITemplateFormatter templateFormatter) {
        _templateFormatter = templateFormatter;
    }

    public void Format<T>(T value, in EncodedTextWriter writer) {
        var formattedValue = _templateFormatter.Format(value);

        if (formattedValue != null) {
            writer.Write(formattedValue);
        }
    }

    public virtual bool TryCreateFormatter(Type type, out IHandlebarsFormatter formatter) {
        if (_templateFormatter.CanFormat(type)) {
            formatter = this;
        } else {
            formatter = null;
        }

        return formatter != null;
    }
}
