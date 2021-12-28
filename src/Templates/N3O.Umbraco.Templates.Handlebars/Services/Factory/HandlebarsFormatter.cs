using HandlebarsDotNet;
using HandlebarsDotNet.IO;
using System;
using IHandlebarsFormatter = HandlebarsDotNet.IO.IFormatter;

namespace N3O.Umbraco.Templates.Handlebars {
    public class HandlebarsFormatter : IHandlebarsFormatter, IFormatterProvider {
        private readonly IMergeFormatter _mergeFormatter;

        public HandlebarsFormatter(IMergeFormatter mergeFormatter) {
            _mergeFormatter = mergeFormatter;
        }
    
        public void Format<T>(T value, in EncodedTextWriter writer) {
            var formattedValue = _mergeFormatter.Format(value);

            if (formattedValue != null) {
                writer.Write(formattedValue);
            }
        }

        public virtual bool TryCreateFormatter(Type type, out IHandlebarsFormatter formatter) {
            if (_mergeFormatter.CanFormat(type)) {
                formatter = this;
            } else {
                formatter = null;
            }

            return formatter != null;
        }
    }
}
