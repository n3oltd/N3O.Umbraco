using System.Collections.Generic;

namespace N3O.Umbraco.Accounts.Models;

public interface IFieldSettingsCollection {
    IEnumerable<FieldSettings> GetFieldSettings();
}
