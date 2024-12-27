namespace N3O.Umbraco.Blocks.Extensions;

public static class BlockViewModelExtensions {
    public static string HtmlId(this IBlockViewModel blockViewModel) {
        return $"html_{blockViewModel.Id.ToString().Substring(0, 6)}";
    }
}