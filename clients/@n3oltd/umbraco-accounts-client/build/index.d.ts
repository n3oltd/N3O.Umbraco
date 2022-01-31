export declare class AccountsClient {
    private http;
    private baseUrl;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined;
    constructor(baseUrl?: string, http?: {
        fetch(url: RequestInfo, init?: RequestInit): Promise<Response>;
    });
    getConsentOptions(): Promise<ConsentOption[]>;
    protected processGetConsentOptions(response: Response): Promise<ConsentOption[]>;
    getLookupConsentCategories(): Promise<NamedLookupRes[]>;
    protected processGetLookupConsentCategories(response: Response): Promise<NamedLookupRes[]>;
    getLookupConsentChannels(): Promise<NamedLookupRes[]>;
    protected processGetLookupConsentChannels(response: Response): Promise<NamedLookupRes[]>;
    getLookupConsentResponses(): Promise<NamedLookupRes[]>;
    protected processGetLookupConsentResponses(response: Response): Promise<NamedLookupRes[]>;
    getLookupCountries(): Promise<CountryRes[]>;
    protected processGetLookupCountries(response: Response): Promise<CountryRes[]>;
    getLookupTaxStatuses(): Promise<NamedLookupRes[]>;
    protected processGetLookupTaxStatuses(response: Response): Promise<NamedLookupRes[]>;
    getAllLookups(criteria: LookupsCriteria): Promise<AccountsLookupsRes>;
    protected processGetAllLookups(response: Response): Promise<AccountsLookupsRes>;
}
export interface ConsentOption {
    channel?: ConsentChannel | undefined;
    categories?: string[] | undefined;
    statement?: string | undefined;
}
/** One of 'email', 'sms', 'post', 'telephone' */
export declare enum ConsentChannel {
    Email = "email",
    Sms = "sms",
    Post = "post",
    Telephone = "telephone"
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
export interface CountryRes {
    name?: string | undefined;
    id?: string | undefined;
    iso2Code?: string | undefined;
    iso3Code?: string | undefined;
    localityOptional?: boolean;
    postalCodeOptional?: boolean;
}
export interface AccountsLookupsRes {
    consentCategories?: NamedLookupRes[] | undefined;
    consentChannels?: NamedLookupRes[] | undefined;
    consentResponses?: NamedLookupRes[] | undefined;
    countries?: CountryRes[] | undefined;
    taxStatuses?: NamedLookupRes[] | undefined;
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