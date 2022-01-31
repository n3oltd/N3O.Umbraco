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
    getRegularGivingFrequencies(): Promise<NamedLookupRes[]>;
    protected processGetRegularGivingFrequencies(response: Response): Promise<NamedLookupRes[]>;
    updateAccount(checkoutRevisionId: string, req: AccountReq): Promise<CheckoutRes>;
    protected processUpdateAccount(response: Response): Promise<CheckoutRes>;
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
    consent?: ConsentRes | undefined;
    taxStatus?: TaxStatus | undefined;
}
export interface NameRes {
    title?: string | undefined;
    firstName?: string | undefined;
    lastName?: string | undefined;
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
    country?: string | undefined;
}
export interface EmailRes {
    address?: string | undefined;
}
export interface TelephoneRes {
    country?: string | undefined;
    number?: string | undefined;
}
export interface ConsentRes {
    choices?: ConsentChoiceRes[] | undefined;
}
export interface ConsentChoiceRes {
    channel?: ConsentChannel | undefined;
    category?: string | undefined;
    response?: ConsentResponse | undefined;
}
/** One of 'email', 'sms', 'post', 'telephone' */
export declare enum ConsentChannel {
    Email = "email",
    Sms = "sms",
    Post = "post",
    Telephone = "telephone"
}
/** One of 'noResponse', 'optIn', 'optOut' */
export declare enum ConsentResponse {
    NoResponse = "noResponse",
    OptIn = "optIn",
    OptOut = "optOut"
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
export interface NamedLookupRes {
    id?: string | undefined;
    name?: string | undefined;
}
export interface AccountReq {
    name?: NameReq | undefined;
    address?: AddressReq | undefined;
    email?: EmailReq | undefined;
    telephone?: TelephoneReq | undefined;
    consent?: ConsentReq | undefined;
    taxStatus?: TaxStatus | undefined;
}
export interface NameReq {
    title?: string | undefined;
    firstName?: string | undefined;
    lastName?: string | undefined;
}
export interface AddressReq {
    line1?: string | undefined;
    line2?: string | undefined;
    line3?: string | undefined;
    locality?: string | undefined;
    administrativeArea?: string | undefined;
    postalCode?: string | undefined;
    country?: string | undefined;
}
export interface EmailReq {
    address?: string | undefined;
}
export interface TelephoneReq {
    country?: string | undefined;
    number?: string | undefined;
}
export interface ConsentReq {
    choices?: ConsentChoiceReq[] | undefined;
}
export interface ConsentChoiceReq {
    channel?: ConsentChannel | undefined;
    category?: string | undefined;
    response?: ConsentResponse | undefined;
}
export interface CheckoutLookupsRes {
    checkoutStages?: NamedLookupRes[] | undefined;
    regularGivingFrequencies?: NamedLookupRes[] | undefined;
}
export interface LookupsCriteria {
    types?: string[] | undefined;
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