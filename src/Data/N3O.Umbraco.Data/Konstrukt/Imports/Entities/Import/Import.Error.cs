using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Umbraco.Extensions;

namespace N3O.Umbraco.Data.Konstrukt {
    public partial class Import {
        public void Error(Exception ex, IJsonProvider jsonProvider) {
            Error(ex.ToString().Yield(), jsonProvider);
        }

        public void Error(IEnumerable<string> errors, IJsonProvider jsonProvider) {
            Notices = jsonProvider.SerializeObject(new ImportNotices {
                Errors = errors
            });
            Status = ImportStatuses.Error;
        }
    }
}