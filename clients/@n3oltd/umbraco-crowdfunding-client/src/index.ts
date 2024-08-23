//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v14.0.3.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------

/* tslint:disable */
/* eslint-disable */
// ReSharper disable InconsistentNaming

export class CrowdfundingClient {
    private http: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> };
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(baseUrl?: string, http?: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> }) {
        this.http = http ? http : window as any;
        this.baseUrl = baseUrl ?? "https://localhost:6001";
    }

    getContentPropertyValue(contentId: string, propertyAlias: string): Promise<ContentPropertyValueRes> {
        let url_ = this.baseUrl + "/umbraco/api/Crowdfunding/content/{contentId}/properties/{propertyAlias}";
        if (contentId === undefined || contentId === null)
            throw new Error("The parameter 'contentId' must be defined.");
        url_ = url_.replace("{contentId}", encodeURIComponent("" + contentId));
        if (propertyAlias === undefined || propertyAlias === null)
            throw new Error("The parameter 'propertyAlias' must be defined.");
        url_ = url_.replace("{propertyAlias}", encodeURIComponent("" + propertyAlias));
        url_ = url_.replace(/[?&]$/, "");

        let options_: RequestInit = {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processGetContentPropertyValue(_response);
        });
    }

    protected processGetContentPropertyValue(response: Response): Promise<ContentPropertyValueRes> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            result200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as ContentPropertyValueRes;
            return result200;
            });
        } else if (status === 400) {
            return response.text().then((_responseText) => {
            let result400: any = null;
            result400 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as ProblemDetails;
            return throwException("A server side error occurred.", status, _responseText, _headers, result400);
            });
        } else if (status === 500) {
            return response.text().then((_responseText) => {
            return throwException("A server side error occurred.", status, _responseText, _headers);
            });
        } else if (status === 404) {
            return response.text().then((_responseText) => {
            let result404: any = null;
            result404 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as ProblemDetails;
            return throwException("A server side error occurred.", status, _responseText, _headers, result404);
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<ContentPropertyValueRes>(null as any);
    }

    getNestedPropertySchema(contentId: string, propertyAlias: string): Promise<NestedSchemaRes> {
        let url_ = this.baseUrl + "/umbraco/api/Crowdfunding/content/{contentId}/nested/{propertyAlias}/schema";
        if (contentId === undefined || contentId === null)
            throw new Error("The parameter 'contentId' must be defined.");
        url_ = url_.replace("{contentId}", encodeURIComponent("" + contentId));
        if (propertyAlias === undefined || propertyAlias === null)
            throw new Error("The parameter 'propertyAlias' must be defined.");
        url_ = url_.replace("{propertyAlias}", encodeURIComponent("" + propertyAlias));
        url_ = url_.replace(/[?&]$/, "");

        let options_: RequestInit = {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processGetNestedPropertySchema(_response);
        });
    }

    protected processGetNestedPropertySchema(response: Response): Promise<NestedSchemaRes> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            result200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as NestedSchemaRes;
            return result200;
            });
        } else if (status === 400) {
            return response.text().then((_responseText) => {
            let result400: any = null;
            result400 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as ProblemDetails;
            return throwException("A server side error occurred.", status, _responseText, _headers, result400);
            });
        } else if (status === 500) {
            return response.text().then((_responseText) => {
            return throwException("A server side error occurred.", status, _responseText, _headers);
            });
        } else if (status === 404) {
            return response.text().then((_responseText) => {
            let result404: any = null;
            result404 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as ProblemDetails;
            return throwException("A server side error occurred.", status, _responseText, _headers, result404);
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<NestedSchemaRes>(null as any);
    }

    updateProperty(contentId: string, req: ContentPropertyReq): Promise<void> {
        let url_ = this.baseUrl + "/umbraco/api/Crowdfunding/content/{contentId}/property";
        if (contentId === undefined || contentId === null)
            throw new Error("The parameter 'contentId' must be defined.");
        url_ = url_.replace("{contentId}", encodeURIComponent("" + contentId));
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(req);

        let options_: RequestInit = {
            body: content_,
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processUpdateProperty(_response);
        });
    }

    protected processUpdateProperty(response: Response): Promise<void> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            return;
            });
        } else if (status === 400) {
            return response.text().then((_responseText) => {
            let result400: any = null;
            result400 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as ProblemDetails;
            return throwException("A server side error occurred.", status, _responseText, _headers, result400);
            });
        } else if (status === 500) {
            return response.text().then((_responseText) => {
            return throwException("A server side error occurred.", status, _responseText, _headers);
            });
        } else if (status === 404) {
            return response.text().then((_responseText) => {
            let result404: any = null;
            result404 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as ProblemDetails;
            return throwException("A server side error occurred.", status, _responseText, _headers, result404);
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<void>(null as any);
    }

    checkTitle(req: CreateFundraiserReq): Promise<boolean> {
        let url_ = this.baseUrl + "/umbraco/api/Crowdfunding/fundraisers/checkTitle";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(req);

        let options_: RequestInit = {
            body: content_,
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/json"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processCheckTitle(_response);
        });
    }

    protected processCheckTitle(response: Response): Promise<boolean> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            result200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as boolean;
            return result200;
            });
        } else if (status === 400) {
            return response.text().then((_responseText) => {
            let result400: any = null;
            result400 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as ProblemDetails;
            return throwException("A server side error occurred.", status, _responseText, _headers, result400);
            });
        } else if (status === 500) {
            return response.text().then((_responseText) => {
            return throwException("A server side error occurred.", status, _responseText, _headers);
            });
        } else if (status === 412) {
            return response.text().then((_responseText) => {
            let result412: any = null;
            result412 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as ProblemDetails;
            return throwException("A server side error occurred.", status, _responseText, _headers, result412);
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<boolean>(null as any);
    }

    createFundraiser(req: CreateFundraiserReq): Promise<string> {
        let url_ = this.baseUrl + "/umbraco/api/Crowdfunding/fundraisers";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(req);

        let options_: RequestInit = {
            body: content_,
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/json"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processCreateFundraiser(_response);
        });
    }

    protected processCreateFundraiser(response: Response): Promise<string> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            result200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as string;
            return result200;
            });
        } else if (status === 400) {
            return response.text().then((_responseText) => {
            let result400: any = null;
            result400 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as ProblemDetails;
            return throwException("A server side error occurred.", status, _responseText, _headers, result400);
            });
        } else if (status === 500) {
            return response.text().then((_responseText) => {
            return throwException("A server side error occurred.", status, _responseText, _headers);
            });
        } else if (status === 412) {
            return response.text().then((_responseText) => {
            let result412: any = null;
            result412 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as ProblemDetails;
            return throwException("A server side error occurred.", status, _responseText, _headers, result412);
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<string>(null as any);
    }

    updateFundraiserAllocation(contentId: string, req: UpdateFundraiserAllocationsReq): Promise<void> {
        let url_ = this.baseUrl + "/umbraco/api/Crowdfunding/fundraisers/{contentId}/allocations";
        if (contentId === undefined || contentId === null)
            throw new Error("The parameter 'contentId' must be defined.");
        url_ = url_.replace("{contentId}", encodeURIComponent("" + contentId));
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(req);

        let options_: RequestInit = {
            body: content_,
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processUpdateFundraiserAllocation(_response);
        });
    }

    protected processUpdateFundraiserAllocation(response: Response): Promise<void> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            return;
            });
        } else if (status === 400) {
            return response.text().then((_responseText) => {
            let result400: any = null;
            result400 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as ProblemDetails;
            return throwException("A server side error occurred.", status, _responseText, _headers, result400);
            });
        } else if (status === 500) {
            return response.text().then((_responseText) => {
            return throwException("A server side error occurred.", status, _responseText, _headers);
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<void>(null as any);
    }

    getPropertyTypes(): Promise<LookupRes[]> {
        let url_ = this.baseUrl + "/umbraco/api/Crowdfunding/lookups/propertyTypes";
        url_ = url_.replace(/[?&]$/, "");

        let options_: RequestInit = {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processGetPropertyTypes(_response);
        });
    }

    protected processGetPropertyTypes(response: Response): Promise<LookupRes[]> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            result200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as LookupRes[];
            return result200;
            });
        } else if (status === 400) {
            return response.text().then((_responseText) => {
            let result400: any = null;
            result400 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as ProblemDetails;
            return throwException("A server side error occurred.", status, _responseText, _headers, result400);
            });
        } else if (status === 500) {
            return response.text().then((_responseText) => {
            return throwException("A server side error occurred.", status, _responseText, _headers);
            });
        } else if (status === 404) {
            return response.text().then((_responseText) => {
            let result404: any = null;
            result404 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as ProblemDetails;
            return throwException("A server side error occurred.", status, _responseText, _headers, result404);
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<LookupRes[]>(null as any);
    }
}

export interface ContentPropertyValueRes {
    alias?: string | undefined;
    type?: PropertyType | undefined;
    boolean?: BooleanValueRes | undefined;
    cropper?: CropperValueRes | undefined;
    dateTime?: DateTimeValueRes | undefined;
    nested?: NestedValueRes | undefined;
    numeric?: NumericValueRes | undefined;
    raw?: RawValueRes | undefined;
    textarea?: TextareaValueRes | undefined;
    textBox?: TextBoxValueRes | undefined;
}

/** One of 'boolean', 'cropper', 'dateTime', 'nested', 'numeric', 'raw', 'textarea', 'textBox' */
export enum PropertyType {
    Boolean = "boolean",
    Cropper = "cropper",
    DateTime = "dateTime",
    Nested = "nested",
    Numeric = "numeric",
    Raw = "raw",
    Textarea = "textarea",
    TextBox = "textBox",
}

export interface BooleanValueRes {
    value?: boolean | undefined;
    configuration?: BooleanConfigurationRes | undefined;
}

export interface BooleanConfigurationRes {
    description?: string | undefined;
}

export interface CropperValueRes {
    image?: CropperSource | undefined;
    configuration?: CropperConfigurationRes | undefined;
}

export interface CropperSource {
    src?: string | undefined;
    mediaId?: string | undefined;
    filename?: string | undefined;
    width?: number;
    height?: number;
    altText?: string | undefined;
    crops?: Crop[] | undefined;
}

export interface Crop {
    x?: number;
    y?: number;
    width?: number;
    height?: number;
}

export interface CropperConfigurationRes {
    description?: string | undefined;
}

export interface DateTimeValueRes {
    value?: Date | undefined;
    configuration?: DateTimeConfigurationRes | undefined;
}

export interface DateTimeConfigurationRes {
    description?: string | undefined;
}

export interface NestedValueRes {
    items?: NestedItemRes[] | undefined;
    schema?: NestedSchemaRes | undefined;
    configuration?: NestedConfigurationRes | undefined;
}

export interface NestedItemRes {
    contentTypeAlias?: string | undefined;
    properties?: ContentPropertyValueRes[] | undefined;
}

export interface NestedSchemaRes {
    items?: NestedSchemaItemRes[] | undefined;
}

export interface NestedSchemaItemRes {
    contentTypeAlias?: string | undefined;
    properties?: NestedSchemaPropertyRes[] | undefined;
}

export interface NestedSchemaPropertyRes {
    alias?: string | undefined;
    type?: PropertyType | undefined;
}

export interface NestedConfigurationRes {
    description?: string | undefined;
    maximumItems?: number;
    minimumItems?: number;
}

export interface NumericValueRes {
    value?: number | undefined;
    configuration?: NumericConfigurationRes | undefined;
}

export interface NumericConfigurationRes {
    description?: string | undefined;
}

export interface RawValueRes {
    value?: HtmlEncodedString | undefined;
    configuration?: RawConfigurationRes | undefined;
}

export interface HtmlEncodedString {
}

export interface RawConfigurationRes {
    description?: string | undefined;
    maximumLength?: number;
}

export interface TextareaValueRes {
    value?: string | undefined;
    configuration?: TextareaConfigurationRes | undefined;
}

export interface TextareaConfigurationRes {
    description?: string | undefined;
    maximumLength?: number;
}

export interface TextBoxValueRes {
    value?: string | undefined;
    configuration?: TextBoxConfigurationRes | undefined;
}

export interface TextBoxConfigurationRes {
    description?: string | undefined;
    maximumLength?: number;
}

export interface ProblemDetails {
    type?: string | undefined;
    title?: string | undefined;
    status?: number | undefined;
    detail?: string | undefined;
    instance?: string | undefined;

    [key: string]: any;
}

export interface ContentPropertyReq {
    alias?: string | undefined;
    type?: PropertyType | undefined;
    boolean?: BooleanValueReq | undefined;
    cropper?: CropperValueReq | undefined;
    dateTime?: DateTimeValueReq | undefined;
    nested?: NestedValueReq | undefined;
    numeric?: NumericValueReq | undefined;
    raw?: RawValueReq | undefined;
    textarea?: TextareaValueReq | undefined;
    textBox?: TextBoxValueReq | undefined;
}

export interface BooleanValueReq {
    value?: boolean | undefined;
}

export interface CropperValueReq {
    type?: PropertyType | undefined;
    storageToken?: string | undefined;
    shape?: CropShape | undefined;
    circle?: CircleCropReq | undefined;
    rectangle?: RectangleCropReq | undefined;
}

export interface ByteSize {
    bits?: number;
    bytes?: number;
    kilobytes?: number;
    megabytes?: number;
    gigabytes?: number;
    terabytes?: number;
    largestWholeNumberSymbol?: string | undefined;
    largestWholeNumberFullWord?: string | undefined;
    largestWholeNumberValue?: number;
}

/** One of 'circle', 'rectangle' */
export enum CropShape {
    Circle = "circle",
    Rectangle = "rectangle",
}

export interface CircleCropReq {
    center?: PointReq | undefined;
    radius?: number | undefined;
}

export interface PointReq {
    x?: number | undefined;
    y?: number | undefined;
}

export interface RectangleCropReq {
    bottomLeft?: PointReq | undefined;
    topRight?: PointReq | undefined;
}

export interface DateTimeValueReq {
    value?: Date | undefined;
}

export interface NestedValueReq {
    items?: NestedItemReq[] | undefined;
}

export interface NestedItemReq {
    contentTypeAlias?: string | undefined;
    properties?: ContentPropertyReq[] | undefined;
}

export interface NumericValueReq {
    value?: number | undefined;
}

export interface RawValueReq {
    value?: string | undefined;
}

export interface TextareaValueReq {
    value?: string | undefined;
}

export interface TextBoxValueReq {
    value?: string | undefined;
}

export interface AutoPropertyOfValueReq {
    value?: ValueReq | undefined;
}

export interface ValueReq {
    type?: PropertyType | undefined;
}

export interface CreateFundraiserReq {
    title?: string | undefined;
    slug?: string | undefined;
    campaignId?: string | undefined;
    endDate?: Date | undefined;
    allocations?: FundraiserAllocationReq[] | undefined;
}

export interface FundraiserAllocationReq {
    amount?: number | undefined;
    goalId?: string | undefined;
    feedbackNewCustomFields?: FeedbackNewCustomFieldsReq | undefined;
}

export interface FeedbackNewCustomFieldsReq {
    entries?: FeedbackNewCustomFieldReq[] | undefined;
}

export interface FeedbackNewCustomFieldReq {
    alias?: string | undefined;
    bool?: boolean | undefined;
    date?: Date | undefined;
    text?: string | undefined;
}

export interface UpdateFundraiserAllocationsReq {
    allocations?: UpdateFundraiserAllocationReq[] | undefined;
}

export interface UpdateFundraiserAllocationReq {
    allocationId?: string | undefined;
    allocation?: FundraiserAllocationReq | undefined;
}

export interface LookupRes {
    id?: string | undefined;
}

export class ApiException extends Error {
    message: string;
    status: number;
    response: string;
    headers: { [key: string]: any; };
    result: any;

    constructor(message: string, status: number, response: string, headers: { [key: string]: any; }, result: any) {
        super();

        this.message = message;
        this.status = status;
        this.response = response;
        this.headers = headers;
        this.result = result;
    }

    protected isApiException = true;

    static isApiException(obj: any): obj is ApiException {
        return obj.isApiException === true;
    }
}

function throwException(message: string, status: number, response: string, headers: { [key: string]: any; }, result?: any): any {
    if (result !== null && result !== undefined)
        throw result;
    else
        throw new ApiException(message, status, response, headers, null);
}