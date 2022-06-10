using N3O.Umbraco.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Data.Konstrukt {
    public partial class Import {
        public void Saved(Guid id, string contentSummary, IEnumerable<string> warnings) {
            ImportedContentId = id;
            ImportedContentSummary = contentSummary;
            Notices = JsonConvert.SerializeObject(new ImportNotices {
                Warnings = warnings
            });
            Status = ImportStatuses.Saved;
        }
    }
}