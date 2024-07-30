//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v14.0.3.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0)) (http://NSwag.org)
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
    CrowdfundingClient.prototype.getCrowdfundingPagePropertyTypes = function () {
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
            return _this.processGetCrowdfundingPagePropertyTypes(_response);
        });
    };
    CrowdfundingClient.prototype.processGetCrowdfundingPagePropertyTypes = function (response) {
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
    CrowdfundingClient.prototype.checkName = function (req) {
        var _this = this;
        var url_ = this.baseUrl + "/umbraco/api/Crowdfunding/pages/checkName";
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
            return _this.processCheckName(_response);
        });
    };
    CrowdfundingClient.prototype.processCheckName = function (response) {
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
    CrowdfundingClient.prototype.createPage = function (req) {
        var _this = this;
        var url_ = this.baseUrl + "/umbraco/api/Crowdfunding/pages";
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
            return _this.processCreatePage(_response);
        });
    };
    CrowdfundingClient.prototype.processCreatePage = function (response) {
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
    CrowdfundingClient.prototype.getPagePropertyValue = function (pageId, propertyAlias) {
        var _this = this;
        var url_ = this.baseUrl + "/umbraco/api/Crowdfunding/pages/{pageId}/properties/{propertyAlias}";
        if (pageId === undefined || pageId === null)
            throw new Error("The parameter 'pageId' must be defined.");
        url_ = url_.replace("{pageId}", encodeURIComponent("" + pageId));
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
            return _this.processGetPagePropertyValue(_response);
        });
    };
    CrowdfundingClient.prototype.processGetPagePropertyValue = function (response) {
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
    CrowdfundingClient.prototype.updateProperty = function (pageId, req) {
        var _this = this;
        var url_ = this.baseUrl + "/umbraco/api/Crowdfunding/pages/{pageId}/property";
        if (pageId === undefined || pageId === null)
            throw new Error("The parameter 'pageId' must be defined.");
        url_ = url_.replace("{pageId}", encodeURIComponent("" + pageId));
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
/** One of 'boolean', 'cropper', 'dateTime', 'numeric', 'raw', 'textarea', 'textBox' */
export var PropertyType;
(function (PropertyType) {
    PropertyType["Boolean"] = "boolean";
    PropertyType["Cropper"] = "cropper";
    PropertyType["DateTime"] = "dateTime";
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