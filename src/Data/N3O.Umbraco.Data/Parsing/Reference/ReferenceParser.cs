using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.References;
using System;
using System.Linq;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;

namespace N3O.Umbraco.Data.Parsing;

public class ReferenceParser : DataTypeParser<Reference>, IReferenceParser {
    private readonly ILookups _lookups;

    public ReferenceParser(ILookups lookups) {
        _lookups = lookups;
    }

    public override bool CanParse(DataType dataType) {
        return dataType == OurDataTypes.Reference;
    }

    protected override ParseResult<Reference> TryParse(string text, Type targetType) {
        var referenceTypes = _lookups.GetAll<ReferenceType>();
        var matches = referenceTypes.Where(x => x.IsMatch(text)).ToList();

        if (text.HasValue() && !matches.IsSingle()) {
            return ParseResult.Fail<Reference>();
        }

        var reference = matches.SingleOrDefault()?.ToReference(text);

        return ParseResult.Success(reference);
    }
}
