using Newtonsoft.Json;

namespace N3O.Umbraco.Newsletters.SendGrid.Models;

public class SendGridList : Value, ISendGridList {
    [JsonConstructor]
    public SendGridList(string id, string name) {
        Id = id;
        Name = name;
    }

    public SendGridList(ISendGridList sendGridList) : this(sendGridList.Id, sendGridList.Name) { }

    public string Id { get; }
    public string Name { get; }
}
