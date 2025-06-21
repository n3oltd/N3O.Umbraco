using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Models;

public class PublishedNisab {
    public Dictionary<string, PublishedNisabAmount> Amounts { get; set; }
}