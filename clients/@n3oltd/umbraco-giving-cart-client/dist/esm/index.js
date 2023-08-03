//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v13.18.0.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v10.0.0.0)) (http://NSwag.org)
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
var CartClient = /** @class */ (function () {
    function CartClient(baseUrl, http) {
        this.jsonParseReviver = undefined;
        this.http = http ? http : window;
        this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "https://localhost:6001";
    }
    CartClient.prototype.add = function (req) {
        var _this = this;
        var url_ = this.baseUrl + "/umbraco/api/Cart/add";
        url_ = url_.replace(/[?&]$/, "");
        var content_ = JSON.stringify(req);
        var options_ = {
            body: content_,
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            }
        };
        return this.http.fetch(url_, options_).then(function (_response) {
            return _this.processAdd(_response);
        });
    };
    CartClient.prototype.processAdd = function (response) {
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
        else if (status === 422) {
            return response.text().then(function (_responseText) {
                var result422 = null;
                result422 = _responseText === "" ? null : JSON.parse(_responseText, _this.jsonParseReviver);
                return throwException("A server side error occurred.", status, _responseText, _headers, result422);
            });
        }
        else if (status !== 200 && status !== 204) {
            return response.text().then(function (_responseText) {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve(null);
    };
    CartClient.prototype.addUpsellToCart = function (upsellOfferId, req) {
        var _this = this;
        var url_ = this.baseUrl + "/umbraco/api/Cart/upsells/{upsellOfferId}/addToCart";
        if (upsellOfferId === undefined || upsellOfferId === null)
            throw new Error("The parameter 'upsellOfferId' must be defined.");
        url_ = url_.replace("{upsellOfferId}", encodeURIComponent("" + upsellOfferId));
        url_ = url_.replace(/[?&]$/, "");
        var content_ = JSON.stringify(req);
        var options_ = {
            body: content_,
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            }
        };
        return this.http.fetch(url_, options_).then(function (_response) {
            return _this.processAddUpsellToCart(_response);
        });
    };
    CartClient.prototype.processAddUpsellToCart = function (response) {
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
        else if (status === 422) {
            return response.text().then(function (_responseText) {
                var result422 = null;
                result422 = _responseText === "" ? null : JSON.parse(_responseText, _this.jsonParseReviver);
                return throwException("A server side error occurred.", status, _responseText, _headers, result422);
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
    CartClient.prototype.getSummary = function () {
        var _this = this;
        var url_ = this.baseUrl + "/umbraco/api/Cart/summary";
        url_ = url_.replace(/[?&]$/, "");
        var options_ = {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        };
        return this.http.fetch(url_, options_).then(function (_response) {
            return _this.processGetSummary(_response);
        });
    };
    CartClient.prototype.processGetSummary = function (response) {
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
        else if (status !== 200 && status !== 204) {
            return response.text().then(function (_responseText) {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve(null);
    };
    CartClient.prototype.reset = function () {
        var _this = this;
        var url_ = this.baseUrl + "/umbraco/api/Cart/reset";
        url_ = url_.replace(/[?&]$/, "");
        var options_ = {
            method: "PUT",
            headers: {}
        };
        return this.http.fetch(url_, options_).then(function (_response) {
            return _this.processReset(_response);
        });
    };
    CartClient.prototype.processReset = function (response) {
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
        else if (status === 422) {
            return response.text().then(function (_responseText) {
                var result422 = null;
                result422 = _responseText === "" ? null : JSON.parse(_responseText, _this.jsonParseReviver);
                return throwException("A server side error occurred.", status, _responseText, _headers, result422);
            });
        }
        else if (status !== 200 && status !== 204) {
            return response.text().then(function (_responseText) {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve(null);
    };
    CartClient.prototype.remove = function (req) {
        var _this = this;
        var url_ = this.baseUrl + "/umbraco/api/Cart/remove";
        url_ = url_.replace(/[?&]$/, "");
        var content_ = JSON.stringify(req);
        var options_ = {
            body: content_,
            method: "DELETE",
            headers: {
                "Content-Type": "application/json",
            }
        };
        return this.http.fetch(url_, options_).then(function (_response) {
            return _this.processRemove(_response);
        });
    };
    CartClient.prototype.processRemove = function (response) {
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
        else if (status === 422) {
            return response.text().then(function (_responseText) {
                var result422 = null;
                result422 = _responseText === "" ? null : JSON.parse(_responseText, _this.jsonParseReviver);
                return throwException("A server side error occurred.", status, _responseText, _headers, result422);
            });
        }
        else if (status !== 200 && status !== 204) {
            return response.text().then(function (_responseText) {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve(null);
    };
    CartClient.prototype.removeUpsellFromCart = function (upsellOfferId) {
        var _this = this;
        var url_ = this.baseUrl + "/umbraco/api/Cart/upsells/{upsellOfferId}/removeFromCart";
        if (upsellOfferId === undefined || upsellOfferId === null)
            throw new Error("The parameter 'upsellOfferId' must be defined.");
        url_ = url_.replace("{upsellOfferId}", encodeURIComponent("" + upsellOfferId));
        url_ = url_.replace(/[?&]$/, "");
        var options_ = {
            method: "DELETE",
            headers: {}
        };
        return this.http.fetch(url_, options_).then(function (_response) {
            return _this.processRemoveUpsellFromCart(_response);
        });
    };
    CartClient.prototype.processRemoveUpsellFromCart = function (response) {
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
        else if (status === 422) {
            return response.text().then(function (_responseText) {
                var result422 = null;
                result422 = _responseText === "" ? null : JSON.parse(_responseText, _this.jsonParseReviver);
                return throwException("A server side error occurred.", status, _responseText, _headers, result422);
            });
        }
        else if (status !== 200 && status !== 204) {
            return response.text().then(function (_responseText) {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve(null);
    };
    return CartClient;
}());
export { CartClient };
/** One of 'donation', 'regularGiving' */
export var GivingType;
(function (GivingType) {
    GivingType["Donation"] = "donation";
    GivingType["RegularGiving"] = "regularGiving";
})(GivingType || (GivingType = {}));
/** One of 'feedback', 'fund', 'sponsorship' */
export var AllocationType;
(function (AllocationType) {
    AllocationType["Feedback"] = "feedback";
    AllocationType["Fund"] = "fund";
    AllocationType["Sponsorship"] = "sponsorship";
})(AllocationType || (AllocationType = {}));
export var PublishedItemType;
(function (PublishedItemType) {
    PublishedItemType[PublishedItemType["Unknown"] = 0] = "Unknown";
    PublishedItemType[PublishedItemType["Element"] = 1] = "Element";
    PublishedItemType[PublishedItemType["Content"] = 2] = "Content";
    PublishedItemType[PublishedItemType["Media"] = 3] = "Media";
    PublishedItemType[PublishedItemType["Member"] = 4] = "Member";
})(PublishedItemType || (PublishedItemType = {}));
export var ContentVariation;
(function (ContentVariation) {
    ContentVariation[ContentVariation["Nothing"] = 0] = "Nothing";
    ContentVariation[ContentVariation["Culture"] = 1] = "Culture";
    ContentVariation[ContentVariation["Segment"] = 2] = "Segment";
    ContentVariation[ContentVariation["CultureAndSegment"] = 3] = "CultureAndSegment";
})(ContentVariation || (ContentVariation = {}));
export var PropertyCacheLevel;
(function (PropertyCacheLevel) {
    PropertyCacheLevel[PropertyCacheLevel["Unknown"] = 0] = "Unknown";
    PropertyCacheLevel[PropertyCacheLevel["Element"] = 1] = "Element";
    PropertyCacheLevel[PropertyCacheLevel["Elements"] = 2] = "Elements";
    PropertyCacheLevel[PropertyCacheLevel["Snapshot"] = 3] = "Snapshot";
    PropertyCacheLevel[PropertyCacheLevel["None"] = 4] = "None";
})(PropertyCacheLevel || (PropertyCacheLevel = {}));
/** One of 'bool', 'date', 'text' */
export var FeedbackCustomFieldType;
(function (FeedbackCustomFieldType) {
    FeedbackCustomFieldType["Bool"] = "bool";
    FeedbackCustomFieldType["Date"] = "date";
    FeedbackCustomFieldType["Text"] = "text";
})(FeedbackCustomFieldType || (FeedbackCustomFieldType = {}));
/** One of '_6', '_12', '_18', '_24', '_36', '_48', '_60' */
export var SponsorshipDuration;
(function (SponsorshipDuration) {
    SponsorshipDuration["_6"] = "_6";
    SponsorshipDuration["_12"] = "_12";
    SponsorshipDuration["_18"] = "_18";
    SponsorshipDuration["_24"] = "_24";
    SponsorshipDuration["_36"] = "_36";
    SponsorshipDuration["_48"] = "_48";
    SponsorshipDuration["_60"] = "_60";
})(SponsorshipDuration || (SponsorshipDuration = {}));
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