export declare class CartClient {
    private http;
    private baseUrl;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined;
    constructor(baseUrl?: string, http?: {
        fetch(url: RequestInfo, init?: RequestInit): Promise<Response>;
    });
    add(req: AddToCartReq): Promise<void>;
    protected processAdd(response: Response): Promise<void>;
    addUpsellToCart(upsellOfferId: string, req: AddUpsellToCartReq): Promise<void>;
    protected processAddUpsellToCart(response: Response): Promise<void>;
    bulkRemove(req: BulkRemoveFromCartReq): Promise<void>;
    protected processBulkRemove(response: Response): Promise<void>;
    getSummary(): Promise<CartSummaryRes>;
    protected processGetSummary(response: Response): Promise<CartSummaryRes>;
    reset(): Promise<void>;
    protected processReset(response: Response): Promise<void>;
    remove(req: RemoveFromCartReq): Promise<void>;
    protected processRemove(response: Response): Promise<void>;
    removeUpsellFromCart(upsellOfferId: string): Promise<void>;
    protected processRemoveUpsellFromCart(response: Response): Promise<void>;
}
export interface ProblemDetails {
    type?: string | undefined;
    title?: string | undefined;
    status?: number | undefined;
    detail?: string | undefined;
    instance?: string | undefined;
    [key: string]: any;
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
    feedback?: FeedbackAllocationReq | undefined;
    fund?: FundAllocationReq | undefined;
    sponsorship?: SponsorshipAllocationReq | undefined;
    upsellOfferId?: string | undefined;
    "n3O.Umbraco.Giving.Models.IAllocation.FundDimensions"?: IFundDimensionValues | undefined;
    "n3O.Umbraco.Giving.Models.IAllocation.Fund"?: IFundAllocation | undefined;
    "n3O.Umbraco.Giving.Models.IAllocation.Sponsorship"?: ISponsorshipAllocation | undefined;
    "n3O.Umbraco.Giving.Models.IAllocation.Feedback"?: IFeedbackAllocation | undefined;
    "n3O.Umbraco.Giving.Models.IAllocation.Value"?: Money | undefined;
}
/** One of 'feedback', 'fund', 'sponsorship' */
export declare enum AllocationType {
    Feedback = "feedback",
    Fund = "fund",
    Sponsorship = "sponsorship"
}
export interface MoneyReq {
    amount?: number | undefined;
    currency?: string | undefined;
}
export interface FundDimensionValuesReq {
    dimension1?: string | undefined;
    dimension2?: string | undefined;
    dimension3?: string | undefined;
    dimension4?: string | undefined;
}
export interface FeedbackAllocationReq {
    scheme?: string | undefined;
    customFields?: FeedbackNewCustomFieldsReq | undefined;
    "n3O.Umbraco.Giving.Models.IFeedbackAllocation.CustomFields"?: IFeedbackCustomField[] | undefined;
}
export interface FeedbackCustomFieldDefinitionElement {
    content?: IPublishedElement | undefined;
    type?: FeedbackCustomFieldType | undefined;
    name?: string | undefined;
    required?: boolean;
    textMaxLength?: number | undefined;
    alias?: string | undefined;
}
export interface IPublishedElement {
    contentType?: IPublishedContentType;
    key?: string;
    properties?: IPublishedProperty[];
}
export interface IPublishedContentType {
    key?: string;
    id?: number;
    alias?: string;
    itemType?: PublishedItemType;
    compositionAliases?: string[];
    variations?: ContentVariation;
    isElement?: boolean;
    propertyTypes?: IPublishedPropertyType[];
}
export declare enum PublishedItemType {
    Unknown = 0,
    Element = 1,
    Content = 2,
    Media = 3,
    Member = 4
}
export declare enum ContentVariation {
    Nothing = 0,
    Culture = 1,
    Segment = 2,
    CultureAndSegment = 3
}
export interface IPublishedPropertyType {
    contentType?: IPublishedContentType | undefined;
    dataType?: PublishedDataType;
    alias?: string;
    editorAlias?: string;
    isUserProperty?: boolean;
    variations?: ContentVariation;
    cacheLevel?: PropertyCacheLevel;
    deliveryApiCacheLevel?: PropertyCacheLevel;
    modelClrType?: string;
    clrType?: string | undefined;
}
export interface PublishedDataType {
    id?: number;
    editorAlias?: string;
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
    propertyType?: IPublishedPropertyType;
    alias?: string;
}
/** One of 'bool', 'date', 'text' */
export declare enum FeedbackCustomFieldType {
    Bool = "bool",
    Date = "date",
    Text = "text"
}
export interface PriceContent {
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
    "n3O.Umbraco.Giving.Models.IPricingRule.FundDimensions"?: IFundDimensionValues | undefined;
}
export interface IFundDimensionValues {
    dimension1?: string | undefined;
    dimension2?: string | undefined;
    dimension3?: string | undefined;
    dimension4?: string | undefined;
}
export interface IPricingRule {
    fundDimensions?: IFundDimensionValues | undefined;
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
export interface IFeedbackCustomField {
    type?: FeedbackCustomFieldType | undefined;
    alias?: string | undefined;
    name?: string | undefined;
    bool?: boolean | undefined;
    date?: Date | undefined;
    text?: string | undefined;
}
export interface FundAllocationReq {
    donationItem?: string | undefined;
}
export interface SponsorshipAllocationReq {
    beneficiaryReference?: string | undefined;
    scheme?: string | undefined;
    duration?: SponsorshipDuration | undefined;
    components?: SponsorshipComponentAllocationReq[] | undefined;
    "n3O.Umbraco.Giving.Models.ISponsorshipAllocation.Components"?: ISponsorshipComponentAllocation[] | undefined;
}
/** One of '_6', '_12', '_18', '_24', '_36', '_48', '_60' */
export declare enum SponsorshipDuration {
    _6 = "_6",
    _12 = "_12",
    _18 = "_18",
    _24 = "_24",
    _36 = "_36",
    _48 = "_48",
    _60 = "_60"
}
export interface SponsorshipComponentAllocationReq {
    component?: string | undefined;
    value?: MoneyReq | undefined;
    "n3O.Umbraco.Giving.Models.ISponsorshipComponentAllocation.Value"?: Money | undefined;
}
export interface Money {
    amount?: number;
    currency?: string | undefined;
}
export interface ISponsorshipComponentAllocation {
    component?: string | undefined;
    value?: Money | undefined;
}
export interface IFundAllocation {
    donationItem?: string | undefined;
}
export interface ISponsorshipAllocation {
    beneficiaryReference?: string | undefined;
    scheme?: string | undefined;
    duration?: SponsorshipDuration | undefined;
    components?: ISponsorshipComponentAllocation[] | undefined;
}
export interface IFeedbackAllocation {
    scheme?: string | undefined;
    customFields?: IFeedbackCustomField[] | undefined;
}
export interface AddUpsellToCartReq {
    amount?: number | undefined;
}
export interface BulkRemoveFromCartReq {
    givingType?: GivingType | undefined;
    indexes?: number[] | undefined;
}
export interface CartSummaryRes {
    revisionId?: string | undefined;
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