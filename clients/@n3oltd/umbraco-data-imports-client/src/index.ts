//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v14.0.3.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------

/* tslint:disable */
/* eslint-disable */
// ReSharper disable InconsistentNaming

export class ImportsClient {
    private http: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> };
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(baseUrl?: string, http?: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> }) {
        this.http = http ? http : window as any;
        this.baseUrl = baseUrl ?? "https://localhost:6001";
    }

    addFileToImport(referenceId: string, req: AddFileToImportReq): Promise<void> {
        let url_ = this.baseUrl + "/umbraco/backoffice/api/Imports/queued/{referenceId}/files";
        if (referenceId === undefined || referenceId === null)
            throw new Error("The parameter 'referenceId' must be defined.");
        url_ = url_.replace("{referenceId}", encodeURIComponent("" + referenceId));
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(req);

        let options_: RequestInit = {
            body: content_,
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processAddFileToImport(_response);
        });
    }

    protected processAddFileToImport(response: Response): Promise<void> {
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
        return Promise.resolve<void>(null as any);
    }

    getLookupDatePatterns(): Promise<NamedLookupRes[]> {
        let url_ = this.baseUrl + "/umbraco/backoffice/api/Imports/lookups/datePatterns";
        url_ = url_.replace(/[?&]$/, "");

        let options_: RequestInit = {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processGetLookupDatePatterns(_response);
        });
    }

    protected processGetLookupDatePatterns(response: Response): Promise<NamedLookupRes[]> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            result200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as NamedLookupRes[];
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
        return Promise.resolve<NamedLookupRes[]>(null as any);
    }

    getTemplate(contentType: string): Promise<void> {
        let url_ = this.baseUrl + "/umbraco/backoffice/api/Imports/template/{contentType}";
        if (contentType === undefined || contentType === null)
            throw new Error("The parameter 'contentType' must be defined.");
        url_ = url_.replace("{contentType}", encodeURIComponent("" + contentType));
        url_ = url_.replace(/[?&]$/, "");

        let options_: RequestInit = {
            method: "GET",
            headers: {
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processGetTemplate(_response);
        });
    }

    protected processGetTemplate(response: Response): Promise<void> {
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

    queue(containerId: string, contentType: string, req: QueueImportsReq): Promise<QueueImportsRes> {
        let url_ = this.baseUrl + "/umbraco/backoffice/api/Imports/queue/{containerId}/{contentType}";
        if (containerId === undefined || containerId === null)
            throw new Error("The parameter 'containerId' must be defined.");
        url_ = url_.replace("{containerId}", encodeURIComponent("" + containerId));
        if (contentType === undefined || contentType === null)
            throw new Error("The parameter 'contentType' must be defined.");
        url_ = url_.replace("{contentType}", encodeURIComponent("" + contentType));
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
            return this.processQueue(_response);
        });
    }

    protected processQueue(response: Response): Promise<QueueImportsRes> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            result200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as QueueImportsRes;
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
        return Promise.resolve<QueueImportsRes>(null as any);
    }

    requeueFailed(): Promise<void> {
        let url_ = this.baseUrl + "/umbraco/backoffice/api/Imports/requeueFailed";
        url_ = url_.replace(/[?&]$/, "");

        let options_: RequestInit = {
            method: "PUT",
            headers: {
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processRequeueFailed(_response);
        });
    }

    protected processRequeueFailed(response: Response): Promise<void> {
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
}

export interface ProblemDetails {
    type?: string | undefined;
    title?: string | undefined;
    status?: number | undefined;
    detail?: string | undefined;
    instance?: string | undefined;

    [key: string]: any;
}

export interface AddFileToImportReq {
    file?: string | undefined;
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

export interface NamedLookupRes {
    id?: string | undefined;
    name?: string | undefined;
}

export interface QueueImportsRes {
    count?: number;
}

export interface QueueImportsReq {
    datePattern?: DatePattern | undefined;
    moveUpdatedContentToCurrentLocation?: boolean | undefined;
    csvFile?: string | undefined;
    zipFile?: string | undefined;
}

/** One of 'dmy', 'mdy', 'ymd' */
export enum DatePattern {
    Dmy = "dmy",
    Mdy = "mdy",
    Ymd = "ymd",
}

/** One of 'dmy_slashes', 'dmy_dashes', 'mdy_slashes', 'mdy_dashes', 'ymd_slashes', 'ymd_dashes' */
export enum DateFormat {
    Dmy_slashes = "dmy_slashes",
    Dmy_dashes = "dmy_dashes",
    Mdy_slashes = "mdy_slashes",
    Mdy_dashes = "mdy_dashes",
    Ymd_slashes = "ymd_slashes",
    Ymd_dashes = "ymd_dashes",
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