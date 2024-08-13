export declare class CrowdfundingClient {
    private http;
    private baseUrl;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined;
    constructor(baseUrl?: string, http?: {
        fetch(url: RequestInfo, init?: RequestInit): Promise<Response>;
    });
    getCrowdfundingPagePropertyTypes(): Promise<LookupRes[]>;
    protected processGetCrowdfundingPagePropertyTypes(response: Response): Promise<LookupRes[]>;
    checkName(req: CreatePageReq): Promise<boolean>;
    protected processCheckName(response: Response): Promise<boolean>;
    createPage(req: CreatePageReq): Promise<string>;
    protected processCreatePage(response: Response): Promise<string>;
    getPagePropertyValue(pageId: string, propertyAlias: string): Promise<PagePropertyValueRes>;
    protected processGetPagePropertyValue(response: Response): Promise<PagePropertyValueRes>;
    updateProperty(pageId: string, req: PagePropertyReq): Promise<void>;
    protected processUpdateProperty(response: Response): Promise<void>;
}
export interface LookupRes {
    id?: string | undefined;
}
export interface ProblemDetails {
    type?: string | undefined;
    title?: string | undefined;
    status?: number | undefined;
    detail?: string | undefined;
    instance?: string | undefined;
    [key: string]: any;
}
export interface CreatePageReq {
    name?: string | undefined;
    slug?: string | undefined;
    campaignId?: string | undefined;
    fundraiserName?: string | undefined;
    allocation?: PageAllocationReq[] | undefined;
}
export interface PageAllocationReq {
    type?: AllocationType | undefined;
    value?: MoneyReq | undefined;
    fundDimensions?: FundDimensionValuesReq | undefined;
    feedback?: FeedbackAllocationReq | undefined;
    fund?: FundAllocationReq | undefined;
    sponsorship?: SponsorshipAllocationReq | undefined;
    upsellOfferId?: string | undefined;
    title?: string | undefined;
    priceHandles?: PriceHandleReq[] | undefined;
    [key: string]: any;
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
}
/** One of 'donation', 'regularGiving' */
export declare enum GivingType {
    Donation = "donation",
    RegularGiving = "regularGiving"
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
    deliveryApiCacheLevelForExpansion?: PropertyCacheLevel;
    modelClrType?: string;
    deliveryApiModelClrType?: string;
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
export interface FundAllocationReq {
    donationItem?: string | undefined;
}
export interface SponsorshipAllocationReq {
    beneficiaryReference?: string | undefined;
    scheme?: string | undefined;
    duration?: SponsorshipDuration | undefined;
    components?: SponsorshipComponentAllocationReq[] | undefined;
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
}
export interface PriceHandleReq {
    amount?: number | undefined;
    description?: string | undefined;
}
export interface PagePropertyValueRes {
    alias?: string | undefined;
    type?: PropertyType | undefined;
    boolean?: BooleanValueRes | undefined;
    cropper?: CropperValueRes | undefined;
    dateTime?: DateTimeValueRes | undefined;
    nested?: NestedContentValueRes | undefined;
    numeric?: NumericValueRes | undefined;
    raw?: RawValueRes | undefined;
    textarea?: TextareaValueRes | undefined;
    textBox?: TextBoxValueRes | undefined;
}
/** One of 'boolean', 'cropper', 'dateTime', 'nested', 'numeric', 'raw', 'textarea', 'textBox' */
export declare enum PropertyType {
    Boolean = "boolean",
    Cropper = "cropper",
    DateTime = "dateTime",
    Nested = "nested",
    Numeric = "numeric",
    Raw = "raw",
    Textarea = "textarea",
    TextBox = "textBox"
}
export interface BooleanValueRes {
    value?: boolean | undefined;
}
export interface CropperValueRes {
    image?: CropperSource | undefined;
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
export interface DateTimeValueRes {
    value?: Date | undefined;
}
export interface NestedContentValueRes {
    items?: NestedItemRes[] | undefined;
}
export interface NestedItemRes {
    contentTypeAlias?: string | undefined;
    properties?: PagePropertyValueRes[] | undefined;
}
export interface NumericValueRes {
    value?: number | undefined;
}
export interface RawValueRes {
    value?: HtmlEncodedString | undefined;
}
export interface HtmlEncodedString {
}
export interface TextareaValueRes {
    value?: string | undefined;
}
export interface TextBoxValueRes {
    value?: string | undefined;
}
export interface PagePropertyReq {
    alias?: string | undefined;
    type?: PropertyType | undefined;
    boolean?: BooleanValueReq | undefined;
    cropper?: CropperValueReq | undefined;
    dateTime?: DateTimeValueReq | undefined;
    numeric?: NumericValueReq | undefined;
    raw?: RawValueReq | undefined;
    textarea?: TextareaValueReq | undefined;
    nestedContent?: NestedContentValueReq | undefined;
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
export declare enum CropShape {
    Circle = "circle",
    Rectangle = "rectangle"
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
export interface NumericValueReq {
    value?: number | undefined;
}
export interface RawValueReq {
    value?: string | undefined;
}
export interface TextareaValueReq {
    value?: string | undefined;
}
export interface NestedContentValueReq {
    items?: NestedItemReq[] | undefined;
}
export interface NestedItemReq {
    contentTypeAlias?: string | undefined;
    properties?: PagePropertyReq[] | undefined;
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