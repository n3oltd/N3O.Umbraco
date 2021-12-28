using N3O.Umbraco.Localization;
using System;

namespace N3O.Umbraco.Templates {
    public abstract class MergeFormatter<TValue> : IMergeFormatter {
        protected MergeFormatter(IFormatter formatter) {
            Formatter = formatter;
        }
    
        public string Format(object value) {
            if (value == null) {
                return null;
            }
        
            if (!(value is TValue typedValue)) {
                throw new ArgumentException($"is not of type {typeof(TValue)}", nameof(value));
            }

            var formattedValue = Format(typedValue);

            return formattedValue;
        }

        public virtual bool CanFormat(Type type) {
            var canFormat = type.IsAssignableTo(typeof(TValue));

            return canFormat;
        }

        protected abstract string Format(TValue value);
    
        protected IFormatter Formatter { get; }
    }
}
