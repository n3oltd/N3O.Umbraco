export declare class AccountsClient {
    private http;
    private baseUrl;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined;
    constructor(baseUrl?: string, http?: {
        fetch(url: RequestInfo, init?: RequestInit): Promise<Response>;
    });
    getDataEntrySettings(): Promise<DataEntrySettings>;
    protected processGetDataEntrySettings(response: Response): Promise<DataEntrySettings>;
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
export interface DataEntrySettings {
    name?: NameDataEntrySettings | undefined;
    address?: AddressDataEntrySettings | undefined;
    email?: EmailDataEntrySettings | undefined;
    phone?: PhoneDataEntrySettings | undefined;
    consent?: ConsentDataEntrySettings | undefined;
}
export interface NameDataEntrySettings {
    title?: TitleDataEntrySettings | undefined;
    firstName?: FirstNameDataEntrySettings | undefined;
    lastName?: LastNameDataEntrySettings | undefined;
}
export interface TitleDataEntrySettings {
    required?: boolean;
    label?: string | undefined;
    helpText?: string | undefined;
    order?: number;
    options?: string[] | undefined;
}
export interface FirstNameDataEntrySettings {
    required?: boolean;
    label?: string | undefined;
    helpText?: string | undefined;
    order?: number;
    capitalisation?: Capitalisation | undefined;
}
/** One of 'lower', 'title', 'upper' */
export declare enum Capitalisation {
    Lower = "lower",
    Title = "title",
    Upper = "upper"
}
export interface LastNameDataEntrySettings {
    required?: boolean;
    label?: string | undefined;
    helpText?: string | undefined;
    order?: number;
    capitalisation?: Capitalisation | undefined;
}
export interface AddressDataEntrySettings {
    defaultCountry?: string | undefined;
    line1?: AddressFieldDataEntrySettings | undefined;
    line2?: AddressFieldDataEntrySettings | undefined;
    line3?: AddressFieldDataEntrySettings | undefined;
    locality?: AddressFieldDataEntrySettings | undefined;
    administrativeArea?: AddressFieldDataEntrySettings | undefined;
    postalCode?: AddressFieldDataEntrySettings | undefined;
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
export interface AddressFieldDataEntrySettings {
    visible?: boolean;
    required?: boolean;
    label?: string | undefined;
    helpText?: string | undefined;
    order?: number;
}
export interface EmailDataEntrySettings {
    required?: boolean;
    label?: string | undefined;
    helpText?: string | undefined;
    validate?: boolean;
}
export interface PhoneDataEntrySettings {
    required?: boolean;
    label?: string | undefined;
    helpText?: string | undefined;
    defaultCountry?: string | undefined;
    validate?: boolean;
}
export interface ConsentDataEntrySettings {
    consentOptions?: ConsentOption[] | undefined;
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
    diallingCode?: string | undefined;
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