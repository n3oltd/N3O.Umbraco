using N3O.Umbraco.Accounts.Lookups;
using N3O.Umbraco.Entities;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.TaxRelief.Lookups;
using N3O.Umbraco.Utilities;
using Newtonsoft.Json;

namespace N3O.Umbraco.Accounts.Models;

public class AccountRes : IAccount {
    public EntityId Id { get; set; }
    public string Reference { get; set; }
    public AccountType Type { get; set; }
    public IndividualRes Individual { get; set; }
    public OrganizationRes Organization { get; set; }
    public AddressRes Address { get; set; }
    public EmailRes Email { get; set; }
    public TelephoneRes Telephone { get; set; }
    public ConsentRes Consent { get; set; }
    public TaxStatus TaxStatus { get; set; }

    [JsonIgnore]
    IIndividual IAccount.Individual => Individual;

    [JsonIgnore]
    IOrganization IAccount.Organization => Organization;

    [JsonIgnore]
    IAddress IAccount.Address => Address;

    [JsonIgnore]
    IEmail IAccount.Email => Email;

    [JsonIgnore]
    ITelephone IAccount.Telephone => Telephone;

    [JsonIgnore]
    IConsent IAccount.Consent => Consent;

    public string Token {
        get {
            var json = JsonConvert.SerializeObject(new {
                                                           id = Id.Value,
                                                           reference = Reference,
                                                           name = GetName(),
                                                           initials = GetInitials()
                                                       });

            return Base64.Encode(json);
        }
    }

    private string GetName() {
        if (Type == AccountTypes.Individual) {
            if (Individual.HasValue(x => x.Name)) {
                return FormatName(Individual.Name.Title, Individual.Name.FirstName, Individual.Name.LastName);
            }
        } else if (Type == AccountTypes.Organization) {
            if (Organization.HasValue(x => x.Name)) { } else {
                return FormatName(Organization.Contact.Title, Organization.Contact.FirstName, Organization.Contact.LastName);
            }
        } else {
            throw UnrecognisedValueException.For(Type);
        }

        return null;
    }

    private string GetInitials() {
        if (Type == AccountTypes.Individual) {
            if (Individual.HasValue(x => x.Name?.FirstName) && Individual.HasValue(x => x.Name?.LastName)) {
                return $"{GetFirstLetter(Individual.Name.FirstName)}{GetFirstLetter(Individual.Name.LastName)}";
            } else if (Individual.HasValue(x => x.Name?.FirstName)) {
                return GetFirstLetter(Individual.Name.FirstName);
            } else if (Individual.HasValue(x => x.Name?.LastName)) {
                return GetFirstLetter(Individual.Name.LastName);
            } else {
                return null;
            }
        } else if (Type == AccountTypes.Organization) {
            return GetFirstLetters(Organization.Name, 2);
        } else {
            throw UnrecognisedValueException.For(Type);
        }
    }

    private string FormatName(string title, string firstName, string lastName) {
        var names = new[] {
                              title, firstName, lastName
                          }.ExceptNull();

        var formattedName = string.Join(" ", names);

        return formattedName;
    }

    private string GetFirstLetter(string str) {
        return GetFirstLetters(str, 1);
    }

    private string GetFirstLetters(string str, int length) {
        return str.Left(length);
    }
}