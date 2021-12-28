using System;

namespace N3O.Umbraco.Scheduler.Attributes {
    public class RecurringJobAttribute : Attribute {
        public RecurringJobAttribute(string jobName, string cronExpression) {
            JobName = jobName;
            CronExpression = cronExpression;
        }

        public string JobName { get; }
        public string CronExpression { get; }
    }
}