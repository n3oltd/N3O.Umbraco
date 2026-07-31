using System;

namespace N3O.Umbraco.Scheduler.Attributes;

// Apply only to idempotent commands that are re-scheduled on every startup.
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class SignatureFencedAttribute : Attribute { }
