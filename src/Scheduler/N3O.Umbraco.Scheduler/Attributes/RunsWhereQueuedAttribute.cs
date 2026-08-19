using System;

namespace N3O.Umbraco.Scheduler.Attributes;

// Use when a command must only execute on the instance of the site where it was enqueued.
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class RunsWhereQueuedAttribute : Attribute { }
