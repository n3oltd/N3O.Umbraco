using N3O.Umbraco.Mediator;
using N3O.Umbraco.Scheduler.Attributes;

namespace N3O.Umbraco.Search.Commands;

[RunsWhereQueued]
public class GenerateSitemapCommand : Request<None, None> { }