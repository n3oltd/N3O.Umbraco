using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Data.Konstrukt {
    public partial class Import {
        public void Saved(Guid id, string contentSummary, IEnumerable<string> errors) {
            ImportedContentId = id;
            ImportedContentSummary = contentSummary;
            Errors = JsonConvert.SerializeObject(errors);;
            Status = ImportStatuses.Saved;
        }
    }
}