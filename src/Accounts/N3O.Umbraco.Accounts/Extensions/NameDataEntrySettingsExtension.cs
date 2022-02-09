using N3O.Umbraco.Accounts.Models;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Accounts.Extensions {
    public static class NameDataEntrySettingsExtension {
        public static IEnumerable<FieldSettings> GetOrderedFields(this NameDataEntrySettings settings) {
            var fields = new FieldSettings[] {settings.Title, settings.FirstName, settings.LastName};
            
            return fields.Where(x => x.Visible).OrderBy(x => x.Order);
        }
    }
}