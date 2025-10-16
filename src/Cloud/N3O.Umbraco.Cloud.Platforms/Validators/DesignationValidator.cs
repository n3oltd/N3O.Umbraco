using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Cloud.Platforms.Validators;

public abstract class DesignationValidator<TDesignationContent> : ContentValidator {
    private static readonly IEnumerable<string> Aliases = new[] {
        AliasHelper<TDesignationContent>.ContentTypeAlias(),
    };
    
    private readonly ILookups _lookups;

    public DesignationValidator(IContentHelper contentHelper, ILookups lookups) : base(contentHelper) {
        _lookups = lookups;
    }
    
    public override bool IsValidator(ContentProperties content) {
        return Aliases.Contains(content.ContentTypeAlias, true);
    }
    
    public override void Validate(ContentProperties content) {
        var fundDimensionOptions = GetFundDimensionOptions(content);

        if (fundDimensionOptions != null) {
            DimensionAllowed(content, fundDimensionOptions.Dimension1, AliasHelper<DesignationContent>.PropertyAlias(x => x.Dimension1));
            DimensionAllowed(content, fundDimensionOptions.Dimension2, AliasHelper<DesignationContent>.PropertyAlias(x => x.Dimension2));
            DimensionAllowed(content, fundDimensionOptions.Dimension3, AliasHelper<DesignationContent>.PropertyAlias(x => x.Dimension3));
            DimensionAllowed(content, fundDimensionOptions.Dimension4, AliasHelper<DesignationContent>.PropertyAlias(x => x.Dimension4));
        }
    }
    
    protected abstract IFundDimensionOptions GetFundDimensionOptions(ContentProperties content);

    private void DimensionAllowed<T>(ContentProperties content,
                                     IEnumerable<T> allowedValues,
                                     string propertyAlias)
        where T : FundDimensionValue<T> {
        var property = content.GetPropertyByAlias(propertyAlias);
        var value = property.IfNotNull(x => ContentHelper.GetLookupValue<T>(_lookups, x));

        if (value != null && allowedValues != null && !allowedValues.Contains(value)) {
            ErrorResult(property, $"{value.Name} is not a permitted fund dimension value");
        }
    }
}