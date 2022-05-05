using System.Collections.Generic;

namespace N3O.Umbraco.Data.Models {
    public class QueueImportsRes {
        public IEnumerable<string> Errors { get; set; }
        public bool Success { get; set; }
        public int? Count { get; set; }
    }
}