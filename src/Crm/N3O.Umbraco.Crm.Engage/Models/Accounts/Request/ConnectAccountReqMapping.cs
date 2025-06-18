using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Crm.Engage.Clients;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.TaxRelief.Lookups;
using System;
using Umbraco.Cms.Core.Mapping;
using ConnectAccountType = N3O.Umbraco.Crm.Engage.Clients.AccountType;

namespace N3O.Umbraco.Crm.Models;

public class ConnectAccountReqMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<IAccount, ConnectAccountReq>((_, _) => new ConnectAccountReq(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(IAccount src, ConnectAccountReq dest, MapperContext ctx) {
        dest.Id = src.Id;
        dest.Reference = src.Reference;
        dest.Type = src.Type.IfNotNull(x => x.Id, Enum.Parse<ConnectAccountType>);
        dest.Individual = src.Individual.IfNotNull(ctx.Map<IIndividual, ConnectIndividualReq>);
        dest.Organization = src.Organization.IfNotNull(ctx.Map<IOrganization, ConnectOrganizationReq>);
        dest.Address = src.Address.IfNotNull(ctx.Map<IAddress, ConnectAddressReq>);
        dest.Email = src.Email.IfNotNull(ctx.Map<IEmail, ConnectEmailReq>);
        dest.Telephone = src.Telephone.IfNotNull(ctx.Map<ITelephone, ConnectTelephoneReq>);
        dest.Preferences = src.Consent.IfNotNull(ctx.Map<IConsent, ConnectPreferencesReq>);
        dest.TaxStatus = src.TaxStatus.IfNotNull(ctx.Map<TaxStatus, ConnectTaxStatusReq>);
    }
}