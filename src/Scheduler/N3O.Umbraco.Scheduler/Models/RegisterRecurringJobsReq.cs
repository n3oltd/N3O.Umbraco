using N3O.Umbraco.Attributes;
using System.Collections.Generic;

namespace N3O.Umbraco.Scheduler.Models {
    public class RegisterRecurringJobsReq {
        [Name("Jobs")]
        public IEnumerable<QueueRecurringJobReq> Jobs { get; set; }
    }
}