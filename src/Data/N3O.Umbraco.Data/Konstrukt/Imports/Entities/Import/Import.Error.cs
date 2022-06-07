using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Umbraco.Extensions;

namespace N3O.Umbraco.Data.Konstrukt {
    public partial class Import {
        public void Error(Exception ex) {
            Error(ex.ToString().Yield());
        }

        public void Error(IEnumerable<string> errors) {
            Errors = JsonConvert.SerializeObject(errors);
            Status = ImportStatuses.Error;
        }
    }
}