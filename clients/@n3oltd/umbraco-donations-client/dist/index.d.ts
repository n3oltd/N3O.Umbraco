export declare class DonationsClient {
    private http;
    private baseUrl;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined;
    constructor(baseUrl?: string, http?: {
        fetch(url: RequestInfo, init?: RequestInit): Promise<Response>;
    });
    getForm(id: string): Promise<DonationFormRes>;
    protected processGetForm(response: Response): Promise<DonationFormRes>;
}
export interface DonationFormRes {
    item?: Item | undefined;
    regular?: DonationOptionsRes | undefined;
    single?: DonationOptionsRes | undefined;
}
export interface Value {
}
export interface UmbracoContentOfFundDimension1Option extends Value {
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
export interface UmbracoContentOfFundDimension2Option extends Value {
    content?: IPublishedContent | undefined;
}
export interface UmbracoContentOfFundDimension3Option extends Value {
    content?: IPublishedContent | undefined;
}
export interface UmbracoContentOfFundDimension4Option extends Value {
    content?: IPublishedContent | undefined;
}
export interface UmbracoContentOfDonationItem extends Value {
    content?: IPublishedContent | undefined;
}
export interface DonationOptionsRes {
    showQuantity?: boolean;
}
export interface ProblemDetails {
    type?: string | undefined;
    title?: string | undefined;
    status?: number | undefined;
    detail?: string | undefined;
    instance?: string | undefined;
}
export interface Anonymous extends UmbracoContentOfDonationItem {
    id?: string | undefined;
    name?: string | undefined;
}
export interface Item extends Anonymous {
    allowSingleDonations?: boolean;
    allowRegularDonations?: boolean;
    free?: boolean;
    price?: number;
    dimension1Options?: Dimension1Options[] | undefined;
    dimension2Options?: Dimension2Options[] | undefined;
    dimension3Options?: Dimension3Options[] | undefined;
    dimension4Options?: Dimension4Options[] | undefined;
}
export interface Anonymous6 extends UmbracoContentOfFundDimension1Option {
    id?: string | undefined;
    name?: string | undefined;
}
export interface Anonymous2 extends Anonymous6 {
    isUnrestricted?: boolean;
}
export interface Dimension1Options extends Anonymous2 {
}
export interface Anonymous7 extends UmbracoContentOfFundDimension2Option {
    id?: string | undefined;
    name?: string | undefined;
}
export interface Anonymous3 extends Anonymous7 {
    isUnrestricted?: boolean;
}
export interface Dimension2Options extends Anonymous3 {
}
export interface Anonymous8 extends UmbracoContentOfFundDimension3Option {
    id?: string | undefined;
    name?: string | undefined;
}
export interface Anonymous4 extends Anonymous8 {
    isUnrestricted?: boolean;
}
export interface Dimension3Options extends Anonymous4 {
}
export interface Anonymous9 extends UmbracoContentOfFundDimension4Option {
    id?: string | undefined;
    name?: string | undefined;
}
export interface Anonymous5 extends Anonymous9 {
    isUnrestricted?: boolean;
}
export interface Dimension4Options extends Anonymous5 {
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