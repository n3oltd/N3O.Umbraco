export declare class CartClient {
    private http;
    private baseUrl;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined;
    constructor(baseUrl?: string, http?: {
        fetch(url: RequestInfo, init?: RequestInit): Promise<Response>;
    });
    add(req: AddToCartReq): Promise<void>;
    protected processAdd(response: Response): Promise<void>;
    getSummary(): Promise<CartSummaryRes>;
    protected processGetSummary(response: Response): Promise<CartSummaryRes>;
    remove(req: RemoveFromCartReq): Promise<void>;
    protected processRemove(response: Response): Promise<void>;
}
export interface ProblemDetails {
    type?: string | undefined;
    title?: string | undefined;
    status?: number | undefined;
    detail?: string | undefined;
    instance?: string | undefined;
}
export interface AddToCartReq {
    givingType?: GivingType | undefined;
    allocation?: AllocationReq | undefined;
    quantity?: number | undefined;
}
/** One of 'donation', 'regularGiving' */
export declare enum GivingType {
    Donation = "donation",
    RegularGiving = "regularGiving"
}
export interface AllocationReq {
    type?: AllocationType | undefined;
    value?: MoneyReq | undefined;
    fundDimensions?: FundDimensionValuesReq | undefined;
    fund?: FundAllocationReq | undefined;
    sponsorship?: SponsorshipAllocationReq | undefined;
}
/** One of 'fund', 'sponsorship' */
export declare enum AllocationType {
    Fund = "fund",
    Sponsorship = "sponsorship"
}
export interface MoneyReq {
    amount?: number | undefined;
    currency?: string | undefined;
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
    cultures?: {
        [key: string]: PublishedCultureInfo;
    } | undefined;
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
export declare enum PublishedItemType {
    Unknown = 0,
    Element = 1,
    Content = 2,
    Media = 3,
    Member = 4
}
export interface FundDimensionValuesReq {
    dimension1?: string | undefined;
    dimension2?: string | undefined;
    dimension3?: string | undefined;
    dimension4?: string | undefined;
}
export interface FundAllocationReq {
    donationItem?: string | undefined;
}
export interface PriceContent {
    content?: IPublishedContent | undefined;
    amount?: number;
    locked?: boolean;
}
export interface PricingRuleElement {
    content?: IPublishedElement | undefined;
    amount?: number;
    locked?: boolean;
    dimension1?: string | undefined;
    dimension2?: string | undefined;
    dimension3?: string | undefined;
    dimension4?: string | undefined;
}
export interface IPublishedElement {
    contentType?: IPublishedContentType | undefined;
    key?: string;
    properties?: IPublishedProperty[] | undefined;
}
export interface IPublishedContentType {
    key?: string;
    id?: number;
    alias?: string | undefined;
    itemType?: PublishedItemType;
    compositionAliases?: string[] | undefined;
    variations?: ContentVariation;
    isElement?: boolean;
    propertyTypes?: IPublishedPropertyType[] | undefined;
}
export declare enum ContentVariation {
    Nothing = 0,
    Culture = 1,
    Segment = 2,
    CultureAndSegment = 3
}
export interface IPublishedPropertyType {
    contentType?: IPublishedContentType | undefined;
    dataType?: PublishedDataType | undefined;
    alias?: string | undefined;
    editorAlias?: string | undefined;
    isUserProperty?: boolean;
    variations?: ContentVariation;
    cacheLevel?: PropertyCacheLevel;
    modelClrType?: string | undefined;
    clrType?: string | undefined;
}
export interface PublishedDataType {
    id?: number;
    editorAlias?: string | undefined;
    configuration?: any | undefined;
}
export declare enum PropertyCacheLevel {
    Unknown = 0,
    Element = 1,
    Elements = 2,
    Snapshot = 3,
    None = 4
}
export interface IPublishedProperty {
    propertyType?: IPublishedPropertyType | undefined;
    alias?: string | undefined;
}
export interface SponsorshipAllocationReq {
    beneficiary?: string | undefined;
    scheme?: string | undefined;
    duration?: SponsorshipDuration | undefined;
    components?: SponsorshipComponentAllocationReq[] | undefined;
}
/** One of '_6', '_12', '_18', '_24' */
export declare enum SponsorshipDuration {
    _6 = "_6",
    _12 = "_12",
    _18 = "_18",
    _24 = "_24"
}
export interface SponsorshipComponentAllocationReq {
    component?: string | undefined;
    value?: MoneyReq | undefined;
}
export interface CartSummaryRes {
    itemCount?: number;
}
export interface RemoveFromCartReq {
    givingType?: GivingType | undefined;
    index?: number | undefined;
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