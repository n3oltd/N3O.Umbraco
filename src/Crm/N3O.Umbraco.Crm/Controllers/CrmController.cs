﻿using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Accounts.Exceptions;
using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Crm.Models;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Validation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Crm.Controllers;

[ApiDocument(CrmConstants.ApiName)]
public class CrmController : LookupsController<CrmLookupsRes> {
    private readonly IAccountManager _accountManager;

    public CrmController(ILookups lookups, IUmbracoMapper mapper, IAccountManager accountManager)
        : base(lookups, mapper) {
        _accountManager = accountManager;
    }

    [HttpPut("accounts/find/{emailAddress}")]
    public async Task<IEnumerable<AccountRes>> FindAccountByEmail([FromRoute] string emailAddress) {
        var res = await _accountManager.FindAccountsByEmailAsync(emailAddress);

        return res;
    }

    [HttpPut("accounts/select")]
    public async Task<ActionResult> SelectAccountAsync(SelectAccountReq req) {
        try {
            await _accountManager.SelectAccountAsync(req.AccountId, req.AccountReference, req.AccountToken);
            
            return Ok();
        } catch (InvalidAccountException ex) {
            return await Task.FromResult<ActionResult>(BadRequest(ex.Message));
        }
    }

    [HttpPost("accounts/create")]
    public async Task<string> CreateAccountAsync(AccountReq req) {
        var res = await _accountManager.CreateAccountAsync(req);

        return res;
    }

    [HttpPut("accounts/update")]
    public async Task<ActionResult> UpdateAccountAsync(AccountReq req) {
        try {
            var res = await _accountManager.UpdateAccountAsync(req);

            return Ok(res);
        } catch (Exception ex) when (ex is ValidationException validationException) {
            return StatusCode(412, new {
                                           Message = "Validation failed",
                                           Details = validationException.Failures
                                       });
        }
    }
}