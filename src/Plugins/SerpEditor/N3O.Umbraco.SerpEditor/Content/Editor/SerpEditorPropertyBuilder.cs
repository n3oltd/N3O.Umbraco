using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.SerpEditor.Models;
using Newtonsoft.Json;
using System;

namespace N3O.Umbraco.SerpEditor.Content;

public class SerpEditorPropertyBuilder : PropertyBuilder {
    private string _description;
    private string _title;

    public SerpEditorPropertyBuilder SetDescription(string description) {
        _description = description;

        return this;
    }

    public SerpEditorPropertyBuilder SetTitle(string title) {
        _title = title;

        return this;
    }

    public override object Build() {
        Validate();

        var serpEntry = new SerpEntry();
        serpEntry.Title = _title;
        serpEntry.Description = _description;

        Value = JsonConvert.SerializeObject(serpEntry);

        return Value;
    }

    private void Validate() {
        if (!_title.HasValue()) {
            throw new Exception("Title must be specified");
        }
    }
}