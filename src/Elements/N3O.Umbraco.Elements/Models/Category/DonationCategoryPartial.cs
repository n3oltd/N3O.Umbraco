using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Elements.Models;

public class DonationCategoryPartial {
    public Guid Id { get; set; }
    public Guid? ParentId { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Image { get; set; }
    public DimensionCategoryData Dimension { get; set; }
    public EphemeralCategoryData Ephemeral { get; set; }
    public GeneralCategoryData General { get; set; }
    public IEnumerable<EntryData> Entries { get; set; }
}