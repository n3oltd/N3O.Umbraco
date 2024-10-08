//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v14.1.0.0 (NJsonSchema v11.0.2.0 (Newtonsoft.Json v13.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
/* tslint:disable */
/* eslint-disable */
// ReSharper disable InconsistentNaming
var EngageClient = /** @class */ (function () {
    function EngageClient(baseUrl, http) {
        this.jsonParseReviver = undefined;
        this.http = http ? http : window;
        this.baseUrl = baseUrl !== null && baseUrl !== void 0 ? baseUrl : "https://localhost:6001";
    }
    EngageClient.prototype.updateCurrentAccount = function (req) {
        var _this = this;
        var url_ = this.baseUrl + "/umbraco/api/Crm/accounts/current";
        url_ = url_.replace(/[?&]$/, "");
        var content_ = JSON.stringify(req);
        var options_ = {
            body: content_,
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
            }
        };
        return this.http.fetch(url_, options_).then(function (_response) {
            return _this.processUpdateCurrentAccount(_response);
        });
    };
    EngageClient.prototype.processUpdateCurrentAccount = function (response) {
        var _this = this;
        var status = response.status;
        var _headers = {};
        if (response.headers && response.headers.forEach) {
            response.headers.forEach(function (v, k) { return _headers[k] = v; });
        }
        ;
        if (status === 200) {
            return response.text().then(function (_responseText) {
                return;
            });
        }
        else if (status === 400) {
            return response.text().then(function (_responseText) {
                var result400 = null;
                result400 = _responseText === "" ? null : JSON.parse(_responseText, _this.jsonParseReviver);
                return throwException("A server side error occurred.", status, _responseText, _headers, result400);
            });
        }
        else if (status === 500) {
            return response.text().then(function (_responseText) {
                return throwException("A server side error occurred.", status, _responseText, _headers);
            });
        }
        else if (status !== 200 && status !== 204) {
            return response.text().then(function (_responseText) {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve(null);
    };
    EngageClient.prototype.getAllLookups = function (criteria) {
        var _this = this;
        var url_ = this.baseUrl + "/umbraco/api/Crm/lookups/all";
        url_ = url_.replace(/[?&]$/, "");
        var content_ = JSON.stringify(criteria);
        var options_ = {
            body: content_,
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/json"
            }
        };
        return this.http.fetch(url_, options_).then(function (_response) {
            return _this.processGetAllLookups(_response);
        });
    };
    EngageClient.prototype.processGetAllLookups = function (response) {
        var _this = this;
        var status = response.status;
        var _headers = {};
        if (response.headers && response.headers.forEach) {
            response.headers.forEach(function (v, k) { return _headers[k] = v; });
        }
        ;
        if (status === 200) {
            return response.text().then(function (_responseText) {
                var result200 = null;
                result200 = _responseText === "" ? null : JSON.parse(_responseText, _this.jsonParseReviver);
                return result200;
            });
        }
        else if (status === 400) {
            return response.text().then(function (_responseText) {
                var result400 = null;
                result400 = _responseText === "" ? null : JSON.parse(_responseText, _this.jsonParseReviver);
                return throwException("A server side error occurred.", status, _responseText, _headers, result400);
            });
        }
        else if (status === 500) {
            return response.text().then(function (_responseText) {
                return throwException("A server side error occurred.", status, _responseText, _headers);
            });
        }
        else if (status === 412) {
            return response.text().then(function (_responseText) {
                var result412 = null;
                result412 = _responseText === "" ? null : JSON.parse(_responseText, _this.jsonParseReviver);
                return throwException("A server side error occurred.", status, _responseText, _headers, result412);
            });
        }
        else if (status !== 200 && status !== 204) {
            return response.text().then(function (_responseText) {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve(null);
    };
    return EngageClient;
}());
export { EngageClient };
/** One of 'individual', 'organization' */
export var AccountType;
(function (AccountType) {
    AccountType["Individual"] = "individual";
    AccountType["Organization"] = "organization";
})(AccountType || (AccountType = {}));
/** One of 'business' */
export var OrganizationType;
(function (OrganizationType) {
    OrganizationType["Business"] = "business";
})(OrganizationType || (OrganizationType = {}));
/** One of 'email', 'sms', 'post', 'telephone' */
export var ConsentChannel;
(function (ConsentChannel) {
    ConsentChannel["Email"] = "email";
    ConsentChannel["Sms"] = "sms";
    ConsentChannel["Post"] = "post";
    ConsentChannel["Telephone"] = "telephone";
})(ConsentChannel || (ConsentChannel = {}));
/** One of 'noResponse', 'optIn', 'optOut' */
export var ConsentResponse;
(function (ConsentResponse) {
    ConsentResponse["NoResponse"] = "noResponse";
    ConsentResponse["OptIn"] = "optIn";
    ConsentResponse["OptOut"] = "optOut";
})(ConsentResponse || (ConsentResponse = {}));
/** One of 'payer', 'nonPayer', 'notSpecified' */
export var TaxStatus;
(function (TaxStatus) {
    TaxStatus["Payer"] = "payer";
    TaxStatus["NonPayer"] = "nonPayer";
    TaxStatus["NotSpecified"] = "notSpecified";
})(TaxStatus || (TaxStatus = {}));
var ApiException = /** @class */ (function (_super) {
    __extends(ApiException, _super);
    function ApiException(message, status, response, headers, result) {
        var _this = _super.call(this) || this;
        _this.isApiException = true;
        _this.message = message;
        _this.status = status;
        _this.response = response;
        _this.headers = headers;
        _this.result = result;
        return _this;
    }
    ApiException.isApiException = function (obj) {
        return obj.isApiException === true;
    };
    return ApiException;
}(Error));
export { ApiException };
function throwException(message, status, response, headers, result) {
    if (result !== null && result !== undefined)
        throw result;
    else
        throw new ApiException(message, status, response, headers, null);
}
//# sourceMappingURL=index.js.map