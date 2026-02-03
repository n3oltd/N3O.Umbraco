using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using N3O.Umbraco.Newsletters.SendGrid.Extensions;
using N3O.Umbraco.Newsletters.SendGrid.Lookups;
using N3O.Umbraco.Newsletters.SendGrid.Models;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Extensions;

namespace N3O.Umbraco.Newsletters.SendGrid;

public class MarketingApiClient : ApiClient, IMarketingApiClient {
    public MarketingApiClient(ISendGridClient client, IJsonProvider jsonProvider) : base(client, jsonProvider) { }
    
    public async Task AddOrUpdateContactAsync(string email,
                                              string listId,
                                              IReadOnlyDictionary<string, object> reservedFields,
                                              IReadOnlyDictionary<string, object> customFields) {
        try {
            var apiReq = new ApiAddOrUpdateContactsReq();
            var existing = await GetContactByEmailAsync(email);
            var apiContactReq = CreateContact(existing, email, reservedFields, customFields);

            apiReq.ListIds = listId.Yield().ToList();
            apiReq.Contacts = apiContactReq.Yield().ToList();

            await PutAsync("marketing/contacts", apiReq);
        } catch (Exception ex) {
            if (ex.IsInvalidApiKey()) {
                RaiseApiError(ApiErrors.InvalidApiKey);
            } else if (ex.IsNotFound()) {
                RaiseApiError(ApiErrors.ListNotFound);
            } else {
                throw;
            }
        }
    }

    public async Task<IReadOnlyList<FieldDefinition>> GetAllFieldDefinitionsAsync(CancellationToken cancellationToken = default) {
        var sgFieldDefinitions = await GetAsync<ApiFieldDefinitions>("marketing/field_definitions", cancellationToken);

        var res = new List<FieldDefinition>();
        
        res.AddRange(sgFieldDefinitions.ReservedFields.OrEmpty().Select(x => x.ToFieldDefinition(true)));
        res.AddRange(sgFieldDefinitions.CustomFields.OrEmpty().Select(x => x.ToFieldDefinition(false)));

        return res;
    }

    public async Task<IReadOnlyList<SendGridList>> GetAllListsAsync(CancellationToken cancellationToken = default) {
        var sgLists = await GetAsync<IEnumerable<ApiListRes>>("marketing/lists", cancellationToken);

        var res = sgLists.Select(x => new SendGridList(x)).ToList();

        return res;
    }

    public async Task<SendGridList> GetListAsync(string listId, CancellationToken cancellationToken = default) {
        try {
            var sgList = await GetAsync<ApiListRes>($"marketing/lists/{listId}", cancellationToken);

            var list = new SendGridList(sgList);

            return list;
        } catch (Exception ex) {
            if (ex.IsInvalidApiKey()) {
                RaiseApiError(ApiErrors.InvalidApiKey);

                return null;
            } else if (ex.IsNotFound()) {
                RaiseApiError(ApiErrors.ListNotFound);

                return null;
            } else {
                throw;
            }
        }
    }

    public async Task<RemoveContactResult> RemoveContactAsync(string email, string listId) {
        try {
            var existing = await GetContactByEmailAsync(email);

            if (existing != null) {
                await DeleteAsync($"marketing/lists/{listId}/contacts?contact_ids={existing.Id}");
                
                return RemoveContactResult.ForUnsubscribe(existing.Id);
            } else {
                return RemoveContactResult.ForNoAction();
            }
        } catch (Exception ex) {
            if (ex.IsInvalidApiKey()) {
                RaiseApiError(ApiErrors.InvalidApiKey);

                return null;
            } else if (ex.IsNotFound()) {
                RaiseApiError(ApiErrors.ListNotFound);

                return null;
            } else {
                throw;
            }
        }
    }

    private async Task<ApiContactRes> GetContactByEmailAsync(string email) {
        try {
            var apiReq = new ApiGetContactByEmailReq();
            apiReq.Emails = email.Yield();

            var res = await PostAsync<ApiGetContactByEmailReq, ApiGetContactByEmailRes>("marketing/contacts/search/emails", apiReq);

            return res.Result.First().Value;
        } catch (Exception ex) {
            if (ex.IsNotFound()) {
                return null;
            } else {
                throw;
            }
        }
    }

    private ApiContactReq CreateContact(ApiContactRes existing,
                                        string email,
                                        IReadOnlyDictionary<string, object> reservedFields,
                                        IReadOnlyDictionary<string, object> customFields) {
        var contact = new ApiContactReq();
        contact.Email = email;

        if (reservedFields.HasAny() || existing.HasAny(x => x.ReservedFields)) {
            contact.ReservedFields = new Dictionary<string, object>(reservedFields.OrEmpty()
                                                                                  .Concat(existing.OrEmpty(x => x.ReservedFields)));
        }

        if (customFields.HasAny() || existing.HasAny(x => x.CustomFields)) {
            contact.CustomFields = new Dictionary<string, object>(customFields.OrEmpty()
                                                                              .Concat(existing.OrEmpty(x => x.CustomFields)));
        }

        return contact;
    }
}
