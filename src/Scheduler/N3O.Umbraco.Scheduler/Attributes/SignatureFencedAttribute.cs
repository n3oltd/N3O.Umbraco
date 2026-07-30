using System;

namespace N3O.Umbraco.Scheduler.Attributes;

/// <summary>
/// Marks a command whose handler mutates state local to the process that runs it. Such a job is stamped with
/// the signature of the runtime that queued it, and any other runtime defers it rather than running it.
/// </summary>
/// <remarks>
/// Apply only to commands re-scheduled on every startup: deferral is bounded at 20 attempts 30 seconds apart,
/// after which the job runs wherever it is held, so the fence delays work but never discards it. For a
/// recurring command the stamp is taken at registration, which happens on every startup.
/// </remarks>
public class SignatureFencedAttribute : Attribute { }
