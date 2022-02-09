/* tslint:disable */
/* eslint-disable */
//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v13.15.5.0 (NJsonSchema v10.6.6.0 (Newtonsoft.Json v9.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------
// ReSharper disable InconsistentNaming

export class AccountsClient {
    private http: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> };
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(baseUrl?: string, http?: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> }) {
        this.http = http ? http : <any>window;
        this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "https://localhost:6001";
    }

    getDataEntrySettings(): Promise<DataEntrySettings> {
        let url_ = this.baseUrl + "/umbraco/api/Accounts/dataEntrySettings";
        url_ = url_.replace(/[?&]$/, "");

        let options_ = <RequestInit>{
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processGetDataEntrySettings(_response);
        });
    }

    protected processGetDataEntrySettings(response: Response): Promise<DataEntrySettings> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            result200 = _responseText === "" ? null : <DataEntrySettings>JSON.parse(_responseText, this.jsonParseReviver);
            return result200;
            });
        } else if (status === 400) {
            return response.text().then((_responseText) => {
            let result400: any = null;
            result400 = _responseText === "" ? null : <ProblemDetails>JSON.parse(_responseText, this.jsonParseReviver);
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
        return Promise.resolve<DataEntrySettings>(<any>null);
    }

    getLookupConsentCategories(): Promise<NamedLookupRes[]> {
        let url_ = this.baseUrl + "/umbraco/api/Accounts/lookups/consentCategories";
        url_ = url_.replace(/[?&]$/, "");

        let options_ = <RequestInit>{
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processGetLookupConsentCategories(_response);
        });
    }

    protected processGetLookupConsentCategories(response: Response): Promise<NamedLookupRes[]> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            result200 = _responseText === "" ? null : <NamedLookupRes[]>JSON.parse(_responseText, this.jsonParseReviver);
            return result200;
            });
        } else if (status === 400) {
            return response.text().then((_responseText) => {
            let result400: any = null;
            result400 = _responseText === "" ? null : <ProblemDetails>JSON.parse(_responseText, this.jsonParseReviver);
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
        return Promise.resolve<NamedLookupRes[]>(<any>null);
    }

    getLookupConsentChannels(): Promise<NamedLookupRes[]> {
        let url_ = this.baseUrl + "/umbraco/api/Accounts/lookups/consentChannels";
        url_ = url_.replace(/[?&]$/, "");

        let options_ = <RequestInit>{
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processGetLookupConsentChannels(_response);
        });
    }

    protected processGetLookupConsentChannels(response: Response): Promise<NamedLookupRes[]> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            result200 = _responseText === "" ? null : <NamedLookupRes[]>JSON.parse(_responseText, this.jsonParseReviver);
            return result200;
            });
        } else if (status === 400) {
            return response.text().then((_responseText) => {
            let result400: any = null;
            result400 = _responseText === "" ? null : <ProblemDetails>JSON.parse(_responseText, this.jsonParseReviver);
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
        return Promise.resolve<NamedLookupRes[]>(<any>null);
    }

    getLookupConsentResponses(): Promise<NamedLookupRes[]> {
        let url_ = this.baseUrl + "/umbraco/api/Accounts/lookups/consentResponses";
        url_ = url_.replace(/[?&]$/, "");

        let options_ = <RequestInit>{
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processGetLookupConsentResponses(_response);
        });
    }

    protected processGetLookupConsentResponses(response: Response): Promise<NamedLookupRes[]> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            result200 = _responseText === "" ? null : <NamedLookupRes[]>JSON.parse(_responseText, this.jsonParseReviver);
            return result200;
            });
        } else if (status === 400) {
            return response.text().then((_responseText) => {
            let result400: any = null;
            result400 = _responseText === "" ? null : <ProblemDetails>JSON.parse(_responseText, this.jsonParseReviver);
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
        return Promise.resolve<NamedLookupRes[]>(<any>null);
    }

    getLookupCountries(): Promise<CountryRes[]> {
        let url_ = this.baseUrl + "/umbraco/api/Accounts/lookups/countries";
        url_ = url_.replace(/[?&]$/, "");

        let options_ = <RequestInit>{
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processGetLookupCountries(_response);
        });
    }

    protected processGetLookupCountries(response: Response): Promise<CountryRes[]> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            result200 = _responseText === "" ? null : <CountryRes[]>JSON.parse(_responseText, this.jsonParseReviver);
            return result200;
            });
        } else if (status === 400) {
            return response.text().then((_responseText) => {
            let result400: any = null;
            result400 = _responseText === "" ? null : <ProblemDetails>JSON.parse(_responseText, this.jsonParseReviver);
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
        return Promise.resolve<CountryRes[]>(<any>null);
    }

    getLookupTaxStatuses(): Promise<NamedLookupRes[]> {
        let url_ = this.baseUrl + "/umbraco/api/Accounts/lookups/taxStatuses";
        url_ = url_.replace(/[?&]$/, "");

        let options_ = <RequestInit>{
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processGetLookupTaxStatuses(_response);
        });
    }

    protected processGetLookupTaxStatuses(response: Response): Promise<NamedLookupRes[]> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            result200 = _responseText === "" ? null : <NamedLookupRes[]>JSON.parse(_responseText, this.jsonParseReviver);
            return result200;
            });
        } else if (status === 400) {
            return response.text().then((_responseText) => {
            let result400: any = null;
            result400 = _responseText === "" ? null : <ProblemDetails>JSON.parse(_responseText, this.jsonParseReviver);
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
        return Promise.resolve<NamedLookupRes[]>(<any>null);
    }

    getAllLookups(criteria: LookupsCriteria): Promise<AccountsLookupsRes> {
        let url_ = this.baseUrl + "/umbraco/api/Accounts/lookups/all";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(criteria);

        let options_ = <RequestInit>{
            body: content_,
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/json"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processGetAllLookups(_response);
        });
    }

    protected processGetAllLookups(response: Response): Promise<AccountsLookupsRes> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            result200 = _responseText === "" ? null : <AccountsLookupsRes>JSON.parse(_responseText, this.jsonParseReviver);
            return result200;
            });
        } else if (status === 400) {
            return response.text().then((_responseText) => {
            let result400: any = null;
            result400 = _responseText === "" ? null : <ProblemDetails>JSON.parse(_responseText, this.jsonParseReviver);
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
        return Promise.resolve<AccountsLookupsRes>(<any>null);
    }
}

export interface DataEntrySettings {
    name?: NameDataEntrySettings | undefined;
    address?: AddressDataEntrySettings | undefined;
    email?: EmailDataEntrySettings | undefined;
    phone?: PhoneDataEntrySettings | undefined;
    consent?: ConsentDataEntrySettings | undefined;
}

export interface NameDataEntrySettings {
    title?: SelectFieldSettings | undefined;
    firstName?: TextFieldSettings | undefined;
    lastName?: TextFieldSettings | undefined;
}

export interface SelectFieldSettings {
    required?: boolean;
    label?: string | undefined;
    helpText?: string | undefined;
    order?: number;
    visible?: boolean;
    validate?: boolean;
    options?: string[] | undefined;
    type?: string | undefined;
}

export interface TextFieldSettings {
    required?: boolean;
    label?: string | undefined;
    helpText?: string | undefined;
    order?: number;
    visible?: boolean;
    validate?: boolean;
    capitalisation?: Capitalisation | undefined;
    type?: string | undefined;
}

/** One of 'lower', 'title', 'upper' */
export enum Capitalisation {
    Lower = "lower",
    Title = "title",
    Upper = "upper",
}

export interface AddressDataEntrySettings {
    defaultCountry?: string | undefined;
    line1?: TextFieldSettings | undefined;
    line2?: TextFieldSettings | undefined;
    line3?: TextFieldSettings | undefined;
    locality?: TextFieldSettings | undefined;
    administrativeArea?: TextFieldSettings | undefined;
    postalCode?: TextFieldSettings | undefined;
}

export interface IPublishedContent {
    id?: number;
    name?: string | undefined;
    urlSegment?: string | undefined;
    sortOrder?: number;
    level?: number;
    path?: string | undefined;
    templateId?: number | undefined;
    creatorId?: number;
    createDate?: Date;
    writerId?: number;
    updateDate?: Date;
    cultures?: { [key: string]: PublishedCultureInfo; } | undefined;
    itemType?: PublishedItemType;
    parent?: IPublishedContent | undefined;
    children?: IPublishedContent[] | undefined;
    childrenForAllCultures?: IPublishedContent[] | undefined;
}

export interface PublishedCultureInfo {
    culture?: string | undefined;
    name?: string | undefined;
    urlSegment?: string | undefined;
    date?: Date;
}

export enum PublishedItemType {
    Unknown = 0,
    Element = 1,
    Content = 2,
    Media = 3,
    Member = 4,
}

export interface EmailDataEntrySettings {
    required?: boolean;
    label?: string | undefined;
    helpText?: string | undefined;
    order?: number;
    visible?: boolean;
    validate?: boolean;
    type?: string | undefined;
}

export interface PhoneDataEntrySettings {
    required?: boolean;
    label?: string | undefined;
    helpText?: string | undefined;
    order?: number;
    visible?: boolean;
    validate?: boolean;
    defaultCountry?: string | undefined;
    type?: string | undefined;
}

export interface ConsentDataEntrySettings {
    consentOptions?: ConsentOption[] | undefined;
}

export interface ConsentOption {
    channel?: ConsentChannel | undefined;
    categories?: string[] | undefined;
    statement?: string | undefined;
}

/** One of 'email', 'sms', 'post', 'telephone' */
export enum ConsentChannel {
    Email = "email",
    Sms = "sms",
    Post = "post",
    Telephone = "telephone",
}

export interface ProblemDetails {
    type?: string | undefined;
    title?: string | undefined;
    status?: number | undefined;
    detail?: string | undefined;
    instance?: string | undefined;
}

export interface NamedLookupRes {
    id?: string | undefined;
    name?: string | undefined;
}

export interface CountryRes {
    name?: string | undefined;
    id?: string | undefined;
    iso2Code?: string | undefined;
    iso3Code?: string | undefined;
    diallingCode?: string | undefined;
    localityOptional?: boolean;
    postalCodeOptional?: boolean;
}

export interface AccountsLookupsRes {
    consentCategories?: NamedLookupRes[] | undefined;
    consentChannels?: NamedLookupRes[] | undefined;
    consentResponses?: NamedLookupRes[] | undefined;
    countries?: CountryRes[] | undefined;
    taxStatuses?: NamedLookupRes[] | undefined;
}

export interface LookupsCriteria {
    types?: string[] | undefined;
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