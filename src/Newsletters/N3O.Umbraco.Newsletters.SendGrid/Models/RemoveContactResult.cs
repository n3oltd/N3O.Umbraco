using N3O.Umbraco.Newsletters.SendGrid.Lookups;

namespace N3O.Umbraco.Newsletters.SendGrid.Models; 

public class RemoveContactResult {
    public UnsubscribeAction Action { get; set; }
    public string ContactId { get; set; }

    public static RemoveContactResult ForUnsubscribe(string contactId) {
        var res = new RemoveContactResult();
        res.Action = UnsubscribeActions.Delete;
        res.ContactId = contactId;

        return res;
    }

    public static RemoveContactResult ForNoAction() {
        var res = new RemoveContactResult();
        res.Action = UnsubscribeActions.NoDelete;

        return res;
    } 
}