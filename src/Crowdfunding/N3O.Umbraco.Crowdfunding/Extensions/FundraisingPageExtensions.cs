using N3O.Umbraco.Content;
using System;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Extensions; 

public static class FundraisingPageExtensions {
    public static async Task UpdatePropertyAsync(this IFundraisingPages fundraisingPages, Guid pageId, Action<IContentPublisher> apply) {
        var editor = await fundraisingPages.GetEditorAsync(pageId);
   
        apply(editor);
      
        editor.SaveAndPublish();
    }
}