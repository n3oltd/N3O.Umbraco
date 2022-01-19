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
    donationType?: DonationType | undefined;
    allocation?: AllocationReq | undefined;
    quantity?: number | undefined;
}
/** One of 'regular', 'single' */
export declare enum DonationType {
    Regular = "regular",
    Single = "single"
}
export interface Value {
}
export interface AllocationReq {
    type?: AllocationType | undefined;
    value?: MoneyReq | undefined;
    dimension1?: Dimension1 | undefined;
    dimension2?: Dimension2 | undefined;
    dimension3?: Dimension3 | undefined;
    dimension4?: Dimension4 | undefined;
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
    currency?: Currency | undefined;
}
export interface UmbracoContentOfCurrency extends Value {
    content?: IPublishedContent | undefined;
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
export interface UmbracoContentOfFundDimension1Option extends Value {
    content?: IPublishedContent | undefined;
}
export interface UmbracoContentOfFundDimension2Option extends Value {
    content?: IPublishedContent | undefined;
}
export interface UmbracoContentOfFundDimension3Option extends Value {
    content?: IPublishedContent | undefined;
}
export interface UmbracoContentOfFundDimension4Option extends Value {
    content?: IPublishedContent | undefined;
}
export interface FundAllocationReq {
    donationItem?: DonationItem | undefined;
}
export interface UmbracoContentOfDonationItem extends Value {
    content?: IPublishedContent | undefined;
}
export interface SponsorshipAllocationReq {
    scheme?: Scheme | undefined;
}
export interface UmbracoContentOfSponsorshipScheme extends Value {
    content?: IPublishedContent | undefined;
}
export interface CartSummaryRes {
    itemCount?: number;
}
export interface RemoveFromCartReq {
    donationType?: DonationType | undefined;
    index?: number | undefined;
}
export interface Anonymous8 extends UmbracoContentOfFundDimension1Option {
    id?: string | undefined;
    name?: string | undefined;
}
export interface Anonymous extends Anonymous8 {
    isUnrestricted?: boolean;
}
export interface Dimension1 extends Anonymous {
}
export interface Anonymous9 extends UmbracoContentOfFundDimension2Option {
    id?: string | undefined;
    name?: string | undefined;
}
export interface Anonymous2 extends Anonymous9 {
    isUnrestricted?: boolean;
}
export interface Dimension2 extends Anonymous2 {
}
export interface Anonymous10 extends UmbracoContentOfFundDimension3Option {
    id?: string | undefined;
    name?: string | undefined;
}
export interface Anonymous3 extends Anonymous10 {
    isUnrestricted?: boolean;
}
export interface Dimension3 extends Anonymous3 {
}
export interface Anonymous11 extends UmbracoContentOfFundDimension4Option {
    id?: string | undefined;
    name?: string | undefined;
}
export interface Anonymous4 extends Anonymous11 {
    isUnrestricted?: boolean;
}
export interface Dimension4 extends Anonymous4 {
}
export interface Anonymous5 extends UmbracoContentOfCurrency {
    id?: string | undefined;
    name?: string | undefined;
}
export interface Currency extends Anonymous5 {
    symbol?: string | undefined;
    isBaseCurrency?: boolean;
    decimalDigits?: number;
}
export interface Anonymous6 extends UmbracoContentOfDonationItem {
    id?: string | undefined;
    name?: string | undefined;
}
export interface DonationItem extends Anonymous6 {
    allowSingleDonations?: boolean;
    allowRegularDonations?: boolean;
    free?: boolean;
    price?: number;
    dimension1Options?: Dimension1[] | undefined;
    dimension2Options?: Dimension2[] | undefined;
    dimension3Options?: Dimension3[] | undefined;
    dimension4Options?: Dimension4[] | undefined;
}
export interface Anonymous7 extends UmbracoContentOfSponsorshipScheme {
    id?: string | undefined;
    name?: string | undefined;
}
export interface Scheme extends Anonymous7 {
    allowSingleDonations?: boolean;
    allowRegularDonations?: boolean;
    price?: number;
    dimension1Options?: Dimension1[] | undefined;
    dimension2Options?: Dimension2[] | undefined;
    dimension3Options?: Dimension3[] | undefined;
    dimension4Options?: Dimension4[] | undefined;
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