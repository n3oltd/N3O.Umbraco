export declare class ContentTypesClient {
    private http;
    private baseUrl;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined;
    constructor(baseUrl?: string, http?: {
        fetch(url: RequestInfo, init?: RequestInit): Promise<Response>;
    });
    getContentTypeByAlias(contentType: string): Promise<ContentTypeRes>;
    protected processGetContentTypeByAlias(response: Response): Promise<ContentTypeRes>;
    getRelationContentTypes(type: string | null | undefined, contentId: string): Promise<ContentTypeSummary[]>;
    protected processGetRelationContentTypes(response: Response): Promise<ContentTypeSummary[]>;
}
export interface ContentTypeRes {
    alias?: string | undefined;
    name?: string | undefined;
    properties?: UmbracoPropertyInfoRes[] | undefined;
}
export interface UmbracoPropertyInfoRes {
    alias?: string | undefined;
    group?: string | undefined;
    dataType?: string | undefined;
    name?: string | undefined;
}
export interface ProblemDetails {
    type?: string | undefined;
    title?: string | undefined;
    status?: number | undefined;
    detail?: string | undefined;
    instance?: string | undefined;
}
export interface ContentTypeSummary {
    alias?: string | undefined;
    name?: string | undefined;
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