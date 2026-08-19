using N3O.Umbraco.Mediator;
using N3O.Umbraco.Scheduler.Attributes;

namespace N3O.Umbraco.Cloud.Platforms.Commands;

[RunsWhereQueued]
public class GeneratePublishedCompositionsCommand : Request<None, None> { }