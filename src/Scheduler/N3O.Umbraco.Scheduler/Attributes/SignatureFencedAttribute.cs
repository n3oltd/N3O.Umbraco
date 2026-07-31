using System;

namespace N3O.Umbraco.Scheduler.Attributes;

// Apply only to commands whose handlers mutate process-local state, are idempotent, and are re-scheduled on
// every startup; a fenced job can run late or more than once but is never discarded.
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class SignatureFencedAttribute : Attribute { }
