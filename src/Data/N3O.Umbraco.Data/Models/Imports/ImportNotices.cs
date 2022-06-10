using System.Collections.Generic;

namespace N3O.Umbraco.Data.Models {
    public class ImportNotices {
        public IEnumerable<string> Errors { get; set; }
        public IEnumerable<string> Warnings { get; set; }
    }
}