using N3O.Umbraco.Data.Criteria;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Mediator;
using System.Collections.Generic;

namespace N3O.Umbraco.Data.Queries;

public class FindContentTypesQuery : Request<ContentTypeCriteria, IEnumerable<ContentTypeRes>> { }