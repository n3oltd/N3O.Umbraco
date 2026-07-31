using System;

namespace N3O.Umbraco.Scheduler.Attributes;

// Apply only to commands that are re-scheduled on every startup and can safely run late or more than once.
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class RunsWhereQueuedAttribute : Attribute { }
