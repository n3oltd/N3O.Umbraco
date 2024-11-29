namespace N3O.Umbraco.Blocks.Perplex;

public interface ILayoutBuilder {
    LayoutDefinition Build(string blockAlias);
    public void SetDescription(string description);
    public void SetName(string name);
}
