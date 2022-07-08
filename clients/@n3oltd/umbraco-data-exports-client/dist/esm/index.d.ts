export declare class ExportsClient {
    private http;
    private baseUrl;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined;
    constructor(baseUrl?: string, http?: {
        fetch(url: RequestInfo, init?: RequestInit): Promise<Response>;
    });
    getLookupContentMetadata(): Promise<ContentMetadataRes[]>;
    protected processGetLookupContentMetadata(response: Response): Promise<ContentMetadataRes[]>;
    getExportableProperties(contentType: string): Promise<ExportableProperty[]>;
    protected processGetExportableProperties(response: Response): Promise<ExportableProperty[]>;
    createExport(containerId: string, contentType: string, req: ExportReq): Promise<void>;
    protected processCreateExport(response: Response): Promise<void>;
}
export interface ContentMetadataRes {
    name?: string | undefined;
    id?: string | undefined;
    autoSelected?: boolean;
    displayOrder?: number;
}
export interface ProblemDetails {
    type?: string | undefined;
    title?: string | undefined;
    status?: number | undefined;
    detail?: string | undefined;
    instance?: string | undefined;
}
export interface ExportableProperty {
    alias?: string | undefined;
    columnTitle?: string | undefined;
}
export interface ExportReq {
    format?: WorkbookFormat | undefined;
    includeUnpublished?: boolean | undefined;
    metadata?: ContentMetadata[] | undefined;
    properties?: string[] | undefined;
}
/** One of 'csv', 'excel' */
export declare enum WorkbookFormat {
    Csv = "csv",
    Excel = "excel"
}
/** One of 'createdAt', 'createdBy', 'editLink', 'hasUnpublishedChanges', 'isPublished', 'name', 'path', 'updatedAt', 'updatedBy' */
export declare enum ContentMetadata {
    CreatedAt = "createdAt",
    CreatedBy = "createdBy",
    EditLink = "editLink",
    HasUnpublishedChanges = "hasUnpublishedChanges",
    IsPublished = "isPublished",
    Name = "name",
    Path = "path",
    UpdatedAt = "updatedAt",
    UpdatedBy = "updatedBy"
}
/** One of 'blob', 'bool', 'content', 'date', 'dateTime', 'decimal', 'guid', 'integer', 'lookup', 'money', 'publishedContent', 'reference', 'string', 'time', 'yearMonth' */
export declare enum DataType {
    Blob = "blob",
    Bool = "bool",
    Content = "content",
    Date = "date",
    DateTime = "dateTime",
    Decimal = "decimal",
    Guid = "guid",
    Integer = "integer",
    Lookup = "lookup",
    Money = "money",
    PublishedContent = "publishedContent",
    Reference = "reference",
    String = "string",
    Time = "time",
    YearMonth = "yearMonth"
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