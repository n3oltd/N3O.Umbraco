using N3O.Umbraco.Blocks;

namespace DemoSite.Blocks.Definitions;

public class Demo : BlockBuilder {
    public Demo() {
        WithAlias("demo");
        WithName("Demo");
        WithIcon("icon-picture");
        WithDescription("This is a demo block");
        AddToCategory(BlockCategories.General);
        SingleLayout();
    }
}
