export declare class PaymentsClient {
    private http;
    private baseUrl;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined;
    constructor(baseUrl?: string, http?: {
        fetch(url: RequestInfo, init?: RequestInit): Promise<Response>;
    });
    findPaymentMethods(req: PaymentMethodCriteria): Promise<PaymentMethodRes[]>;
    protected processFindPaymentMethods(response: Response): Promise<PaymentMethodRes[]>;
    getLookupPaymentMethods(): Promise<PaymentMethodRes[]>;
    protected processGetLookupPaymentMethods(response: Response): Promise<PaymentMethodRes[]>;
    getAllLookups(criteria: LookupsCriteria): Promise<PaymentsLookupsRes>;
    protected processGetAllLookups(response: Response): Promise<PaymentsLookupsRes>;
}
export interface LookupRes {
    id?: string | undefined;
}
export interface NamedLookupRes extends LookupRes {
    name?: string | undefined;
}
export interface PaymentMethodRes extends NamedLookupRes {
}
export interface ProblemDetails {
    type?: string | undefined;
    title?: string | undefined;
    status?: number | undefined;
    detail?: string | undefined;
    instance?: string | undefined;
}
export interface PaymentMethodCriteria {
    country?: Country | undefined;
    currency?: Currency | undefined;
}
export interface Value {
}
export interface UmbracoContentOfCountry extends Value {
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
export interface UmbracoContentOfCurrency extends Value {
    content?: IPublishedContent | undefined;
}
export interface LookupsRes {
}
export interface PaymentsLookupsRes extends LookupsRes {
    paymentMethods?: PaymentMethodRes[] | undefined;
}
export interface LookupsCriteria {
    types?: Types[] | undefined;
}
export interface Anonymous extends UmbracoContentOfCountry {
    id?: string | undefined;
    name?: string | undefined;
}
export interface Country extends Anonymous {
    iso2Code?: string | undefined;
    iso3Code?: string | undefined;
    localityOptional?: boolean;
    postalCodeOptional?: boolean;
}
export interface Anonymous2 extends UmbracoContentOfCurrency {
    id?: string | undefined;
    name?: string | undefined;
}
export interface Currency extends Anonymous2 {
    symbol?: string | undefined;
    isBaseCurrency?: boolean;
    decimalDigits?: number;
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