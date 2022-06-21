using N3O.Umbraco.Lookups;
using System.Text;

namespace N3O.Umbraco.Data.Lookups;

public class TextEncoding : NamedLookup {
    public TextEncoding(string id, string name, int codePage) : base(id, name) {
        CodePage = codePage;
    }

    public int CodePage { get; }
}

[StaticLookups]
public class TextEncodings : StaticLookupsCollection<TextEncoding> {
    public static readonly TextEncoding Iso88591 = new("iso88591", "ISO-8859-1", 28591);
    public static readonly TextEncoding Utf8 = new("utf8", "UTF8", Encoding.UTF8.CodePage);
    public static readonly TextEncoding Windows1252 = new("windows1252", "Windows-1252", 1252);
}
