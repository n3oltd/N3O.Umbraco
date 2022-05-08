export declare class ImportsClient {
    private http;
    private baseUrl;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined;
    constructor(baseUrl?: string, http?: {
        fetch(url: RequestInfo, init?: RequestInit): Promise<Response>;
    });
    getLookupDatePatterns(): Promise<NamedLookupRes[]>;
    protected processGetLookupDatePatterns(response: Response): Promise<NamedLookupRes[]>;
    getTemplate(contentId: string, contentType: string): Promise<void>;
    protected processGetTemplate(response: Response): Promise<void>;
    queue(contentId: string, contentType: string, req: QueueImportsReq): Promise<QueueImportsRes>;
    protected processQueue(response: Response): Promise<QueueImportsRes>;
}
export interface NamedLookupRes {
    id?: string | undefined;
    name?: string | undefined;
}
export interface ProblemDetails {
    type?: string | undefined;
    title?: string | undefined;
    status?: number | undefined;
    detail?: string | undefined;
    instance?: string | undefined;
}
export interface QueueImportsRes {
    errors?: string[] | undefined;
    success?: boolean;
    count?: number | undefined;
}
export interface QueueImportsReq {
    datePattern?: DatePattern | undefined;
    csvFile?: string | undefined;
    zipFile?: string | undefined;
}
/** One of 'dmy', 'mdy', 'ymd' */
export declare enum DatePattern {
    Dmy = "dmy",
    Mdy = "mdy",
    Ymd = "ymd"
}
/** One of 'dmy_slashes', 'dmy_dashes', 'mdy_slashes', 'mdy_dashes', 'ymd_slashes', 'ymd_dashes' */
export declare enum DateFormat {
    Dmy_slashes = "dmy_slashes",
    Dmy_dashes = "dmy_dashes",
    Mdy_slashes = "mdy_slashes",
    Mdy_dashes = "mdy_dashes",
    Ymd_slashes = "ymd_slashes",
    Ymd_dashes = "ymd_dashes"
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
export declare class ApiException extends Error {
    message: string;
    status: number;
    response: string;
    headers: {
        [key: string]: any;
    };
    result: any;
    constructor(message: string, status: number, response: string, headers: {
        [key: string]: any;
    }, result: any);
    protected isApiException: boolean;
    static isApiException(obj: any): obj is ApiException;
}
//# sourceMappingURL=index.d.ts.map