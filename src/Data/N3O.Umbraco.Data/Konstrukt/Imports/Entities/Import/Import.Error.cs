using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Json;
using System;
using System.Collections.Generic;
using Umbraco.Extensions;

namespace N3O.Umbraco.Data.Konstrukt {
    public partial class Import {
        public void Error(IJsonProvider jsonProvider, Exception ex) {
            Error(jsonProvider, ex.ToString().Yield());
        }

        public void Error(IJsonProvider jsonProvider, IEnumerable<string> errors) {
            Notices = jsonProvider.SerializeObject(new ImportNotices {
                Errors = errors
            });
            Status = ImportStatuses.Error;
        }
    }
}