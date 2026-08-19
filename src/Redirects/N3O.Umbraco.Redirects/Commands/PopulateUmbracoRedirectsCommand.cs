using N3O.Umbraco.Mediator;
using N3O.Umbraco.Scheduler.Attributes;

namespace N3O.Umbraco.Redirects.Commands;

[RunsWhereQueued]
public class PopulateUmbracoRedirectsCommand : Request<None, None> { }