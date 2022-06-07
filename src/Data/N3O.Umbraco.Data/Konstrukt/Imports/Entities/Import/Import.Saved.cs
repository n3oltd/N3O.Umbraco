using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Data.Konstrukt {
    public partial class Import {
        public void Saved(Guid id, string contentSummary, IEnumerable<string> saveErrors) {
            ImportedContentId = id;
            ImportedContentSummary = contentSummary;
            Errors = JsonConvert.SerializeObject(saveErrors);
            Status = ImportStatuses.Saved;
        }
    }
}