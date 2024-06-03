using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Extensions; 

public static class FundraisingPageExtensions {
    public static void CreatePage(this IFundraisingPages fundraisingPages, string pageName) {
        var editor = fundraisingPages.New(pageName);
      
        editor.SaveAndPublish();
    }
    
    public static async Task UpdatePropertyAsync(this IFundraisingPages fundraisingPages,
                                                 Guid pageId,
                                                 string pageName,
                                                 Action<IContentPublisher> apply) {
        if (pageId.HasValue() && pageId != Guid.Empty) {
            var editor = await fundraisingPages.GetEditorAsync(pageId);
            
            apply(editor);
      
            editor.SaveAndPublish();
        }
    }
}