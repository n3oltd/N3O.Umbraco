using N3O.Umbraco.Accounts.Models;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Accounts.Extensions;

public static class FieldSettingsCollectionExtensions {
    public static IEnumerable<FieldSettings> GetOrderedFields(this IFieldSettingsCollection collection) {
        return collection.GetFieldSettings().Where(x => x.Visible).OrderBy(x => x.Order);
    }
}
