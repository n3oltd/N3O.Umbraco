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
var CrowdfundingClient = /** @class */ (function () {
    function CrowdfundingClient(baseUrl, http) {
        this.jsonParseReviver = undefined;
        this.http = http ? http : window;
        this.baseUrl = baseUrl !== null && baseUrl !== void 0 ? baseUrl : "https://localhost:6001";
    }
    CrowdfundingClient.prototype.getCampaignGoalOptions = function (campaignId, goalOptionId) {
        var _this = this;
        var url_ = this.baseUrl + "/umbraco/api/Crowdfunding/campaigns/{campaignId}/goalOptions/{goalOptionId}";
        if (campaignId === undefined || campaignId === null)
            throw new Error("The parameter 'campaignId' must be defined.");
        url_ = url_.replace("{campaignId}", encodeURIComponent("" + campaignId));
        if (goalOptionId === undefined || goalOptionId === null)
            throw new Error("The parameter 'goalOptionId' must be defined.");
        url_ = url_.replace("{goalOptionId}", encodeURIComponent("" + goalOptionId));
        url_ = url_.replace(/[?&]$/, "");
        var options_ = {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        };
        return this.http.fetch(url_, options_).then(function (_response) {
            return _this.processGetCampaignGoalOptions(_response);
        });
    };
    CrowdfundingClient.prototype.processGetCampaignGoalOptions = function (response) {
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
        else if (status === 404) {
            return response.text().then(function (_responseText) {
                var result404 = null;
                result404 = _responseText === "" ? null : JSON.parse(_responseText, _this.jsonParseReviver);
                return throwException("A server side error occurred.", status, _responseText, _headers, result404);
            });
        }
        else if (status !== 200 && status !== 204) {
            return response.text().then(function (_responseText) {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve(null);
    };
    CrowdfundingClient.prototype.addToCart = function (crowdfundingReq) {
        var _this = this;
        var url_ = this.baseUrl + "/umbraco/api/Crowdfunding/addToCart";
        url_ = url_.replace(/[?&]$/, "");
        var content_ = JSON.stringify(crowdfundingReq);
        var options_ = {
            body: content_,
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            }
        };
        return this.http.fetch(url_, options_).then(function (_response) {
            return _this.processAddToCart(_response);
        });
    };
    CrowdfundingClient.prototype.processAddToCart = function (response) {
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
    CrowdfundingClient.prototype.getContentPropertyValue = function (contentId, propertyAlias) {
        var _this = this;
        var url_ = this.baseUrl + "/umbraco/api/Crowdfunding/content/{contentId}/properties/{propertyAlias}";
        if (contentId === undefined || contentId === null)
            throw new Error("The parameter 'contentId' must be defined.");
        url_ = url_.replace("{contentId}", encodeURIComponent("" + contentId));
        if (propertyAlias === undefined || propertyAlias === null)
            throw new Error("The parameter 'propertyAlias' must be defined.");
        url_ = url_.replace("{propertyAlias}", encodeURIComponent("" + propertyAlias));
        url_ = url_.replace(/[?&]$/, "");
        var options_ = {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        };
        return this.http.fetch(url_, options_).then(function (_response) {
            return _this.processGetContentPropertyValue(_response);
        });
    };
    CrowdfundingClient.prototype.processGetContentPropertyValue = function (response) {
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
        else if (status === 404) {
            return response.text().then(function (_responseText) {
                var result404 = null;
                result404 = _responseText === "" ? null : JSON.parse(_responseText, _this.jsonParseReviver);
                return throwException("A server side error occurred.", status, _responseText, _headers, result404);
            });
        }
        else if (status !== 200 && status !== 204) {
            return response.text().then(function (_responseText) {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve(null);
    };
    CrowdfundingClient.prototype.getNestedPropertySchema = function (contentId, propertyAlias) {
        var _this = this;
        var url_ = this.baseUrl + "/umbraco/api/Crowdfunding/content/{contentId}/nested/{propertyAlias}/schema";
        if (contentId === undefined || contentId === null)
            throw new Error("The parameter 'contentId' must be defined.");
        url_ = url_.replace("{contentId}", encodeURIComponent("" + contentId));
        if (propertyAlias === undefined || propertyAlias === null)
            throw new Error("The parameter 'propertyAlias' must be defined.");
        url_ = url_.replace("{propertyAlias}", encodeURIComponent("" + propertyAlias));
        url_ = url_.replace(/[?&]$/, "");
        var options_ = {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        };
        return this.http.fetch(url_, options_).then(function (_response) {
            return _this.processGetNestedPropertySchema(_response);
        });
    };
    CrowdfundingClient.prototype.processGetNestedPropertySchema = function (response) {
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
        else if (status === 404) {
            return response.text().then(function (_responseText) {
                var result404 = null;
                result404 = _responseText === "" ? null : JSON.parse(_responseText, _this.jsonParseReviver);
                return throwException("A server side error occurred.", status, _responseText, _headers, result404);
            });
        }
        else if (status !== 200 && status !== 204) {
            return response.text().then(function (_responseText) {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve(null);
    };
    CrowdfundingClient.prototype.updateProperty = function (contentId, req) {
        var _this = this;
        var url_ = this.baseUrl + "/umbraco/api/Crowdfunding/content/{contentId}/property";
        if (contentId === undefined || contentId === null)
            throw new Error("The parameter 'contentId' must be defined.");
        url_ = url_.replace("{contentId}", encodeURIComponent("" + contentId));
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
            return _this.processUpdateProperty(_response);
        });
    };
    CrowdfundingClient.prototype.processUpdateProperty = function (response) {
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
        else if (status === 404) {
            return response.text().then(function (_responseText) {
                var result404 = null;
                result404 = _responseText === "" ? null : JSON.parse(_responseText, _this.jsonParseReviver);
                return throwException("A server side error occurred.", status, _responseText, _headers, result404);
            });
        }
        else if (status !== 200 && status !== 204) {
            return response.text().then(function (_responseText) {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve(null);
    };
    CrowdfundingClient.prototype.suggestSlug = function (name) {
        var _this = this;
        var url_ = this.baseUrl + "/umbraco/api/Crowdfunding/suggestSlug?";
        if (name !== undefined && name !== null)
            url_ += "name=" + encodeURIComponent("" + name) + "&";
        url_ = url_.replace(/[?&]$/, "");
        var options_ = {
            method: "POST",
            headers: {
                "Accept": "application/json"
            }
        };
        return this.http.fetch(url_, options_).then(function (_response) {
            return _this.processSuggestSlug(_response);
        });
    };
    CrowdfundingClient.prototype.processSuggestSlug = function (response) {
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
    CrowdfundingClient.prototype.createFundraiser = function (req) {
        var _this = this;
        var url_ = this.baseUrl + "/umbraco/api/Crowdfunding/fundraisers";
        url_ = url_.replace(/[?&]$/, "");
        var content_ = JSON.stringify(req);
        var options_ = {
            body: content_,
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/json"
            }
        };
        return this.http.fetch(url_, options_).then(function (_response) {
            return _this.processCreateFundraiser(_response);
        });
    };
    CrowdfundingClient.prototype.processCreateFundraiser = function (response) {
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
    CrowdfundingClient.prototype.getFundraiserGoals = function (contentId) {
        var _this = this;
        var url_ = this.baseUrl + "/umbraco/api/Crowdfunding/fundraisers/{contentId}/goals";
        if (contentId === undefined || contentId === null)
            throw new Error("The parameter 'contentId' must be defined.");
        url_ = url_.replace("{contentId}", encodeURIComponent("" + contentId));
        url_ = url_.replace(/[?&]$/, "");
        var options_ = {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        };
        return this.http.fetch(url_, options_).then(function (_response) {
            return _this.processGetFundraiserGoals(_response);
        });
    };
    CrowdfundingClient.prototype.processGetFundraiserGoals = function (response) {
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
        else if (status === 404) {
            return response.text().then(function (_responseText) {
                var result404 = null;
                result404 = _responseText === "" ? null : JSON.parse(_responseText, _this.jsonParseReviver);
                return throwException("A server side error occurred.", status, _responseText, _headers, result404);
            });
        }
        else if (status !== 200 && status !== 204) {
            return response.text().then(function (_responseText) {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve(null);
    };
    CrowdfundingClient.prototype.updateFundraiserGoals = function (contentId, req) {
        var _this = this;
        var url_ = this.baseUrl + "/umbraco/api/Crowdfunding/fundraisers/{contentId}/goals";
        if (contentId === undefined || contentId === null)
            throw new Error("The parameter 'contentId' must be defined.");
        url_ = url_.replace("{contentId}", encodeURIComponent("" + contentId));
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
            return _this.processUpdateFundraiserGoals(_response);
        });
    };
    CrowdfundingClient.prototype.processUpdateFundraiserGoals = function (response) {
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
    CrowdfundingClient.prototype.publishFundraiser = function (fundraiserId) {
        var _this = this;
        var url_ = this.baseUrl + "/umbraco/api/Crowdfunding/fundraisers/{fundraiserId}/publish";
        if (fundraiserId === undefined || fundraiserId === null)
            throw new Error("The parameter 'fundraiserId' must be defined.");
        url_ = url_.replace("{fundraiserId}", encodeURIComponent("" + fundraiserId));
        url_ = url_.replace(/[?&]$/, "");
        var options_ = {
            method: "POST",
            headers: {}
        };
        return this.http.fetch(url_, options_).then(function (_response) {
            return _this.processPublishFundraiser(_response);
        });
    };
    CrowdfundingClient.prototype.processPublishFundraiser = function (response) {
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
    CrowdfundingClient.prototype.getPropertyTypes = function () {
        var _this = this;
        var url_ = this.baseUrl + "/umbraco/api/Crowdfunding/lookups/propertyTypes";
        url_ = url_.replace(/[?&]$/, "");
        var options_ = {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        };
        return this.http.fetch(url_, options_).then(function (_response) {
            return _this.processGetPropertyTypes(_response);
        });
    };
    CrowdfundingClient.prototype.processGetPropertyTypes = function (response) {
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
        else if (status === 404) {
            return response.text().then(function (_responseText) {
                var result404 = null;
                result404 = _responseText === "" ? null : JSON.parse(_responseText, _this.jsonParseReviver);
                return throwException("A server side error occurred.", status, _responseText, _headers, result404);
            });
        }
        else if (status !== 200 && status !== 204) {
            return response.text().then(function (_responseText) {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve(null);
    };
    CrowdfundingClient.prototype.getDashboardStatistics = function (req) {
        var _this = this;
        var url_ = this.baseUrl + "/umbraco/api/Crowdfunding/statistics/dashboard";
        url_ = url_.replace(/[?&]$/, "");
        var content_ = JSON.stringify(req);
        var options_ = {
            body: content_,
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/json"
            }
        };
        return this.http.fetch(url_, options_).then(function (_response) {
            return _this.processGetDashboardStatistics(_response);
        });
    };
    CrowdfundingClient.prototype.processGetDashboardStatistics = function (response) {
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
    return CrowdfundingClient;
}());
export { CrowdfundingClient };
/** One of 'feedback', 'fund', 'sponsorship' */
export var AllocationType;
(function (AllocationType) {
    AllocationType["Feedback"] = "feedback";
    AllocationType["Fund"] = "fund";
    AllocationType["Sponsorship"] = "sponsorship";
})(AllocationType || (AllocationType = {}));
/** One of 'donation', 'regularGiving' */
export var GivingType;
(function (GivingType) {
    GivingType["Donation"] = "donation";
    GivingType["RegularGiving"] = "regularGiving";
})(GivingType || (GivingType = {}));
/** One of 'bool', 'date', 'text' */
export var FeedbackCustomFieldType;
(function (FeedbackCustomFieldType) {
    FeedbackCustomFieldType["Bool"] = "bool";
    FeedbackCustomFieldType["Date"] = "date";
    FeedbackCustomFieldType["Text"] = "text";
})(FeedbackCustomFieldType || (FeedbackCustomFieldType = {}));
/** One of 'campaign', 'fundraiser' */
export var CrowdfunderType;
(function (CrowdfunderType) {
    CrowdfunderType["Campaign"] = "campaign";
    CrowdfunderType["Fundraiser"] = "fundraiser";
})(CrowdfunderType || (CrowdfunderType = {}));
/** One of 'boolean', 'cropper', 'dateTime', 'nested', 'numeric', 'raw', 'textarea', 'textBox' */
export var PropertyType;
(function (PropertyType) {
    PropertyType["Boolean"] = "boolean";
    PropertyType["Cropper"] = "cropper";
    PropertyType["DateTime"] = "dateTime";
    PropertyType["Nested"] = "nested";
    PropertyType["Numeric"] = "numeric";
    PropertyType["Raw"] = "raw";
    PropertyType["Textarea"] = "textarea";
    PropertyType["TextBox"] = "textBox";
})(PropertyType || (PropertyType = {}));
/** One of 'circle', 'rectangle' */
export var CropShape;
(function (CropShape) {
    CropShape["Circle"] = "circle";
    CropShape["Rectangle"] = "rectangle";
})(CropShape || (CropShape = {}));
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