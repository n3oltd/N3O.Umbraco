using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Blocks.Perplex;

public interface IPerplexBlockBuilder {
    IEnumerable<PerplexBlockDefinition> Build(IUmbracoBuilder builder, IWebHostEnvironment webHostEnvironment);
}