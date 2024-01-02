using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Crowdfunding;

/*
 * We need to determine when rendering a fundraising page if the current user is "allowed to edit" that page.
 * In principle, this involves checking the current user against some ACL of who is allowed to edit the page.
 * This should of course include the page creator, but we may want to open the door for future enhancement of
 * having shared pages (e.g. Noor + Talha's Syria Page where either of us can edit) + we have team pages where
 * multiple people can potentially edit + we **may** (TBC) want to allow backoffice users to be able to edit
 * any page.
 *
 * This need to determine "allowed to edit" arises in at least 2 separate contexts:
 * (a) When rendering the page whether it is in edit mode or view mode
 * (b) When we have API calls that have been member authenticated we still need to determine if the user
 *     is allowed to edit the specific page they have specified.
 *
 * We should aim to implement this in as generic a way as possible, as that allows us to isolate the functionality
 * and also potentially to re-use it later in other situations. We can put the below code, apart from crowdfunding
 * implementations into N3O.Umbraco. So, let's assume we have an:
 *
 * IContentAccessControl {
 *    bool CanApply(IContent content); // or maybe Task<bool>, is it for this type of content
 *    bool CanEdit(Member member, IContent content); // Can this member edit this content
 * }
 *
 * builder.RegisterAll(IContentAccessControl);
 *
 * FundraisingPageAccessControl {
 *   CanApply => docType matches
 *   CanEdit => check the member picker on the content to see if we are listed, etc.
 * }
 *
 * IMembersAccessControl {
 *   ctor(IEnumerable<IContentAccessControl> contentAccessControls) {
 * 
 *   }
 * 
 *   bool CanEdit(Member member, IContent content); => SingleOrDefault() contentAccessControls and return, otherwise true
 * }
 *
 * The next challenge is how to use this in the 2 contexts we identified above:
 *
 * (a) When rendering a page. This is easy, we can add a service IFundraisingPageMode which just fetches current page
 *     and current member and exposes a property with an enum so we have:
 *
 *  @if (FundraisingPageMode.Mode == Modes.View) {
 *  } else if (FundraisingPageMode.Mode == Modes.Edit) {
 *  } else throw;
 *
 * (b) We wanted to deal with API calls that involve modifying the page content. We can either try and authenticate
 *     the API call at the web layer (using some kind of authorization filter) or we can try and build this logic into
 *     the domain layer.
 *
 *     PUT /{pageId}/{property}/text
 *     { value: "..." }
 *
 *     [MembersAccessControl]
 *     PUT /{pageId}/{property}/imageCropper
 *     { value: { dimensions, tokens etc. } }
 *
 *     Alternatively, we want to be able to validate the changes, e.g. we might have a limit on the title length
 *     or on certain properties being mandatory.

public class FundraisingPages : IFundraisingPages {
 public FundraisingPages(IContentEditor contentEditor, FundraisingPageAccessControl accessControl) { }
 
 IContentPublisher GetEditor(Guid id) => if (accessControl.CanEdit(pageId)) => contentEditor.ForExisting(pageId);
 IContentPublisher New(string name); => ContentEditor.ForNew();
}

We already have IContentValidator used in ContentValidationHandler to perform validation so should use this as then
it works both for backoffice edits as well as API edits. We need to update COntentPublisher to use this.

static class FundraisingPages {
   Task UpdatePropertyAsync(this IFundraisingPages fundraisingPages, Guid pageId, Action<IContentPublisher> apply) {
     var editor = fundraisingPages.GetEditor(pageId);
   
      apply(editor);
      
      editor.SavePublish();
   }
}

e.g. controllers become:
     fundraisingPages.UpdatePropertyAsync(pageId, x => x.Cropper(propertyAlias).SetImage();
     
     
     We need an API so the user can from the frontend amend the page allocation, we should create this API and controller
     and request models and validators so Waheed has this also
     
     We will also need some additional APIs, e.g. for Waheed to show a UI where the user can edit their allocation he
     will need an API where he can fetch the list of allowed allocations for a campaign etc.
*/