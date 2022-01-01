export declare class {
}
export interface ProblemDetails {
    type?: string | undefined;
    title?: string | undefined;
    status?: number | undefined;
    detail?: string | undefined;
    instance?: string | undefined;
}
export interface AddToCartReq {
    donationType?: DonationType | undefined;
    allocation?: AllocationReq | undefined;
    quantity?: number | undefined;
}
export interface Value {
}
export interface Lookup extends Value {
    id?: string | undefined;
}
export interface NamedLookup extends Lookup {
    name?: string | undefined;
}
export interface DonationType extends NamedLookup {
}
export interface AllocationReq {
    type?: AllocationType | undefined;
    value?: MoneyReq | undefined;
    dimension1?: FundDimension1Option | undefined;
    dimension2?: FundDimension2Option | undefined;
    dimension3?: FundDimension3Option | undefined;
    dimension4?: FundDimension4Option | undefined;
    fund?: FundAllocationReq | undefined;
    sponsorship?: SponsorshipAllocationReq | undefined;
}
export interface AllocationType extends NamedLookup {
}
export interface MoneyReq {
    amount?: number | undefined;
    currency?: Currency | undefined;
}
export interface UmbracoContent extends Value {
    content?: PublishedContentModel | undefined;
}
export interface LookupContent extends UmbracoContent {
    id?: string | undefined;
    name?: string | undefined;
}
export interface Currency extends LookupContent {
    symbol?: string | undefined;
    decimalDigits?: number;
    isBaseCurrency?: boolean;
}
export interface PublishedContentWrapped {
    contentType?: IPublishedContentType | undefined;
    key?: string;
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
    properties?: IPublishedProperty[] | undefined;
}
export interface PublishedContentModel extends PublishedContentWrapped {
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
export interface PublishedCultureInfo {
    culture?: string | undefined;
    name?: string | undefined;
    urlSegment?: string | undefined;
    date?: Date;
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
export interface IPublishedProperty {
    propertyType?: IPublishedPropertyType | undefined;
    alias?: string | undefined;
}
export interface FundDimensionOption extends LookupContent {
    isUnrestricted?: boolean;
}
export interface FundDimension1Option extends FundDimensionOption {
}
export interface FundDimension2Option extends FundDimensionOption {
}
export interface FundDimension3Option extends FundDimensionOption {
}
export interface FundDimension4Option extends FundDimensionOption {
}
export interface FundAllocationReq {
    donationItem?: DonationItem | undefined;
}
export interface DonationItem extends LookupContent {
    allowSingleDonations?: boolean;
    allowRegularDonations?: boolean;
    free?: boolean;
    price?: number;
    dimension1Options?: FundDimension1Option[] | undefined;
    dimension2Options?: FundDimension2Option[] | undefined;
    dimension3Options?: FundDimension3Option[] | undefined;
    dimension4Options?: FundDimension4Option[] | undefined;
}
export interface SponsorshipAllocationReq {
    scheme?: SponsorshipScheme | undefined;
}
export interface SponsorshipScheme extends LookupContent {
    allowSingleDonations?: boolean;
    allowRegularDonations?: boolean;
    price?: number;
    dimension1Options?: FundDimension1Option[] | undefined;
    dimension2Options?: FundDimension2Option[] | undefined;
    dimension3Options?: FundDimension3Option[] | undefined;
    dimension4Options?: FundDimension4Option[] | undefined;
}
export interface CartSummaryRes {
    itemCount?: number;
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