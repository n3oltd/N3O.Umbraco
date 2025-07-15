using Humanizer;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Cloud.Models;

public class PublishedFundDimension {
    public int Index { get; set; }
    public int Number { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public bool Browsable { get; set; }
    public bool Searchable { get; set; }
    public PublishedFundDimensionView View { get; set; }
    public IEnumerable<string> RestrictedOptions { get; set; }
    public string UnrestrictedOption { get; set; }

    public IEnumerable<T> GetOptions<T>(ILookups lookups) where T : FundDimensionValue<T> {
        var options = RestrictedOptions.Select(x => lookups.FindById<T>(x.Camelize())).ToList();

        if (UnrestrictedOption.HasValue()) {
            options.Add(lookups.FindById<T>(UnrestrictedOption.Camelize()));
        }
        
        return options;
    }
}