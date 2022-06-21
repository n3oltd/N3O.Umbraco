namespace N3O.Umbraco.Accounts.Models;

public class DataEntrySettings : Value {
    public DataEntrySettings(NameDataEntrySettings name,
                             AddressDataEntrySettings address,
                             EmailDataEntrySettings email,
                             PhoneDataEntrySettings phone,
                             ConsentDataEntrySettings consent) {
        Name = name;
        Address = address;
        Email = email;
        Phone = phone;
        Consent = consent;
    }

    public NameDataEntrySettings Name { get; }
    public AddressDataEntrySettings Address { get; }
    public EmailDataEntrySettings Email { get; }
    public PhoneDataEntrySettings Phone { get; }
    public ConsentDataEntrySettings Consent { get; }
}
