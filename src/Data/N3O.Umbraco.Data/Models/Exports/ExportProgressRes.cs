using System;

namespace N3O.Umbraco.Data.Models;

public class ExportProgressRes {
    public Guid Id { get; set; }
    public bool IsComplete { get; set; }
    public string Text { get; set; }
}