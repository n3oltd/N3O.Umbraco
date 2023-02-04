namespace N3O.Umbraco.Email.Models;

public class EmailAttachment : Value {
    public EmailAttachment(string name, string contentType, byte[] bytes) {
        Name = name;
        ContentType = contentType;
        Bytes = bytes;
    }

    public string Name { get; }
    public string ContentType { get; }
    public byte[] Bytes { get; }
}