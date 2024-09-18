using N3O.Umbraco.Accounts.Extensions;
using N3O.Umbraco.Accounts.Lookups;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Utilities;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Accounts.Models;

public class AccountMapping : IMapDefinition {
    private readonly IFormatter _formatter;
    private readonly IJsonProvider _jsonProvider;

    public AccountMapping(IFormatter formatter, IJsonProvider jsonProvider) {
        _formatter = formatter;
        _jsonProvider = jsonProvider;
    }
    
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<Account, AccountRes>((_, _) => new AccountRes(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(Account src, AccountRes dest, MapperContext ctx) {
        dest.Id = src.Id;
        dest.Reference = src.Reference;
        dest.Type = src.Type;
        dest.Individual = ctx.Map<Individual, IndividualRes>(src.Individual);
        dest.Organization = ctx.Map<Organization, OrganizationRes>(src.Organization);
        dest.Address = ctx.Map<Address, AddressRes>(src.Address);
        dest.Email = ctx.Map<Email, EmailRes>(src.Email);
        dest.Telephone = ctx.Map<Telephone, TelephoneRes>(src.Telephone);
        dest.Consent = ctx.Map<Consent, ConsentRes>(src.Consent);
        dest.TaxStatus = src.TaxStatus;
        dest.Token = GetToken(dest);
    }

    private string GetToken(AccountRes dest) {
        var data = new {
            Id = dest.Id.Value,
            Reference = dest.Reference,
            Name = GetName(dest),
            Initials = GetInitials(dest)
        };

        var json = _jsonProvider.SerializeObject(data);

        return Base64.Encode(json);
    }

    private string GetName(AccountRes dest) {
        if (dest.Type == AccountTypes.Individual) {
            if (dest.Individual.HasValue(x => x.Name)) {
                return _formatter.Text.ToDisplayName(dest.Individual.Name);
            }
        } else if (dest.Type == AccountTypes.Organization) {
            if (dest.Organization.HasValue(x => x.Name)) {
                return dest.Organization.Name;
            } else if (dest.Organization.HasValue(x => x.Contact)) {
                return _formatter.Text.ToDisplayName(dest.Organization.Contact);
            }
        } else {
            throw UnrecognisedValueException.For(dest.Type);
        }

        return dest.Reference;
    }

    private string GetInitials(AccountRes dest) {
        if (dest.Type == AccountTypes.Individual) {
            if (dest.Individual.HasValue(x => x.Name?.FirstName) && dest.Individual.HasValue(x => x.Name?.LastName)) {
                return $"{GetFirstLetter(dest.Individual.Name.FirstName)}{GetFirstLetter(dest.Individual.Name.LastName)}";
            } else if (dest.Individual.HasValue(x => x.Name?.FirstName)) {
                return GetFirstLetter(dest.Individual.Name.FirstName);
            } else if (dest.Individual.HasValue(x => x.Name?.LastName)) {
                return GetFirstLetter(dest.Individual.Name.LastName);
            }
        } else if (dest.Type == AccountTypes.Organization) {
            if (dest.Organization.HasValue(x => x.Name)) {
                return GetFirstLetters(dest.Organization.Name, 2);
            }
        } else {
            throw UnrecognisedValueException.For(dest.Type);
        }

        return null;
    }

    private string GetFirstLetter(string str) {
        return GetFirstLetters(str, 1);
    }

    private string GetFirstLetters(string str, int length) {
        return str.Left(length);
    }
}