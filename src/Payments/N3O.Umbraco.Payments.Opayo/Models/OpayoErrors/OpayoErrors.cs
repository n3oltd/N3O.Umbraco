using System.Collections.Generic;

namespace N3O.Umbraco.Payments.Opayo.Models {
    public class OpayoErrors {
        public IEnumerable<OpayoError> Errors { get; set; }
    }
}