using N3O.Umbraco.Accounts.Models;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Accounts.Extensions {
    public static class AddressDataEntrySettingsExtension {
        public static IEnumerable<FieldSettings> GetOrderedFields(this AddressDataEntrySettings settings) {
            var fields = new FieldSettings[] {settings.Line1, settings.Line2, settings.Line3, settings.Locality, settings.AdministrativeArea, settings.PostalCode};
            
            return fields.Where(x => x.Visible).OrderBy(x => x.Order);
        }
    }
}