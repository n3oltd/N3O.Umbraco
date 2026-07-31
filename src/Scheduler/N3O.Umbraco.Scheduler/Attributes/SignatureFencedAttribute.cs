using System;

namespace N3O.Umbraco.Scheduler.Attributes;

/// <summary>
/// Marks a command whose handler mutates state local to the process that runs it. The job is stamped with the
/// signature of the runtime that queued it, and a concurrent runtime of the same version defers it rather than
/// running it; a runtime of a different version runs it immediately, since the stamping runtime is going away.
/// </summary>
/// <remarks>
/// Only apply to commands that are idempotent and re-scheduled on every startup. Deferral is bounded at 20
/// attempts 30 seconds apart, after which the job runs wherever it is held, so the fence delays work but never
/// discards it. Deferring completes the original job and schedules a new one, so the returned job id does not
/// survive it. The fence assumes one replica per deployment; at two or more it cannot converge.
/// </remarks>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class SignatureFencedAttribute : Attribute { }
