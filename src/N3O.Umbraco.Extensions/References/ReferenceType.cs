using N3O.Umbraco.Lookups;
using System.Text.RegularExpressions;

namespace N3O.Umbraco.References;

public class ReferenceType : Lookup {
    public ReferenceType(string prefix, long startFrom) : base(prefix) {
        Prefix = prefix;
        StartFrom = startFrom;
    }

    public string Prefix { get; }
    public long StartFrom { get; }

    public int MinDigits => StartFrom.ToString().Length;

    public bool IsMatch(string s) {
        var isMatch = Regex.IsMatch(s, $"^{Prefix}[0-9]{{{MinDigits},}}$");

        return isMatch;
    }
    
    public Reference ToReference(string s) {
        if (!IsMatch(s)) {
            return null;
        }
        
        var match = Regex.Match(s, $"^{Prefix}([0-9]{{{MinDigits},}})$");

        return new Reference(this, long.Parse(match.Groups[1].Value));
    }
}

public class ReferenceTypes : TypesLookupsCollection<ReferenceType> { }
