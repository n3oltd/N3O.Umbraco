using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Extensions {
    public static class ContentExtensions {
        public static bool SetNameIfChanged(this IContent content, string newName) {
            if (content.Name == newName) {
                return false;
            }

            content.Name = newName;

            return true;
        }

        public static bool SetValueIfChanged<T>(this IContent content, string propertyAlias, object value) {
            var changed = false;

            var currentValue = content.GetValue<T>(propertyAlias);

            if ((currentValue == null && value != null) ||
                (currentValue != null && value == null) ||
                (currentValue != null && !currentValue.Equals(value))) {
                content.SetValue(propertyAlias, value);

                changed = true;
            }

            return changed;
        }
    }
}
