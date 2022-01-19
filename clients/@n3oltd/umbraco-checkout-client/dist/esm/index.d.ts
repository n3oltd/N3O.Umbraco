export declare class CheckoutClient {
    private http;
    private baseUrl;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined;
    constructor(baseUrl?: string, http?: {
        fetch(url: RequestInfo, init?: RequestInit): Promise<Response>;
    });
    getCurrentCheckout(): Promise<CheckoutRes>;
    protected processGetCurrentCheckout(response: Response): Promise<CheckoutRes>;
    getLookupCheckoutStages(): Promise<NamedLookupRes[]>;
    protected processGetLookupCheckoutStages(response: Response): Promise<NamedLookupRes[]>;
    getLookupCountries(): Promise<CountryRes[]>;
    protected processGetLookupCountries(response: Response): Promise<CountryRes[]>;
    getLookupTaxStatuses(): Promise<NamedLookupRes[]>;
    protected processGetLookupTaxStatuses(response: Response): Promise<NamedLookupRes[]>;
    getAllLookups(criteria: LookupsCriteria): Promise<CheckoutLookupsRes>;
    protected processGetAllLookups(response: Response): Promise<CheckoutLookupsRes>;
}
export interface CheckoutRes {
    account?: AccountRes | undefined;
}
export interface AccountRes {
    name?: NameRes | undefined;
    address?: AddressRes | undefined;
    email?: EmailRes | undefined;
    telephone?: TelephoneRes | undefined;
    taxStatus?: TaxStatus | undefined;
}
export interface NameRes {
    title?: Title | undefined;
    firstName?: string | undefined;
    lastName?: string | undefined;
}
export interface Value {
}
export interface UmbracoContentOfTitle extends Value {
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
export interface AddressRes {
    line1?: string | undefined;
    line2?: string | undefined;
    line3?: string | undefined;
    locality?: string | undefined;
    administrativeArea?: string | undefined;
    postalCode?: string | undefined;
    country?: Country | undefined;
}
export interface UmbracoContentOfCountry extends Value {
    content?: IPublishedContent | undefined;
}
export interface EmailRes {
    address?: string | undefined;
}
export interface TelephoneRes {
    country?: Country | undefined;
    number?: string | undefined;
}
/** One of 'payer', 'nonPayer', 'notSpecified' */
export declare enum TaxStatus {
    Payer = "payer",
    NonPayer = "nonPayer",
    NotSpecified = "notSpecified"
}
export interface ProblemDetails {
    type?: string | undefined;
    title?: string | undefined;
    status?: number | undefined;
    detail?: string | undefined;
    instance?: string | undefined;
}
export interface LookupRes {
    id?: string | undefined;
}
export interface NamedLookupRes extends LookupRes {
    name?: string | undefined;
}
export interface CountryRes extends NamedLookupRes {
    iso2Code?: string | undefined;
    iso3Code?: string | undefined;
    localityOptional?: boolean;
    postalCodeOptional?: boolean;
}
export interface LookupsRes {
}
export interface CheckoutLookupsRes extends LookupsRes {
    checkoutStages?: NamedLookupRes[] | undefined;
    countries?: CountryRes[] | undefined;
    taxStatuses?: NamedLookupRes[] | undefined;
}
export interface LookupsCriteria {
    types?: Types[] | undefined;
}
export interface Anonymous extends UmbracoContentOfTitle {
    id?: string | undefined;
    name?: string | undefined;
}
export interface Title extends Anonymous {
}
export interface Anonymous2 extends UmbracoContentOfCountry {
    id?: string | undefined;
    name?: string | undefined;
}
export interface Country extends Anonymous2 {
    iso2Code?: string | undefined;
    iso3Code?: string | undefined;
    localityOptional?: boolean;
    postalCodeOptional?: boolean;
}
export interface Anonymous3 extends Value {
    id?: string | undefined;
}
export interface Types extends Anonymous3 {
    lookupType?: string | undefined;
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