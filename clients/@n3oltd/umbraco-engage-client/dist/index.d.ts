export declare class EngageClient {
    private http;
    private baseUrl;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined;
    constructor(baseUrl?: string, http?: {
        fetch(url: RequestInfo, init?: RequestInit): Promise<Response>;
    });
    updateCurrentAccount(req: AccountReq): Promise<void>;
    protected processUpdateCurrentAccount(response: Response): Promise<void>;
    getAllLookups(criteria: LookupsCriteria): Promise<CrmLookupsRes>;
    protected processGetAllLookups(response: Response): Promise<CrmLookupsRes>;
}
export interface ProblemDetails {
    type?: string | undefined;
    title?: string | undefined;
    status?: number | undefined;
    detail?: string | undefined;
    instance?: string | undefined;
    [key: string]: any;
}
export interface AccountReq {
    type?: AccountType | undefined;
    individual?: IndividualReq | undefined;
    organization?: OrganizationReq | undefined;
    address?: AddressReq | undefined;
    email?: EmailReq | undefined;
    telephone?: TelephoneReq | undefined;
    consent?: ConsentReq | undefined;
    taxStatus?: TaxStatus | undefined;
    captcha?: CaptchaReq | undefined;
}
/** One of 'individual', 'organization' */
export declare enum AccountType {
    Individual = "individual",
    Organization = "organization"
}
export interface IndividualReq {
    name?: NameReq | undefined;
}
export interface NameReq {
    title?: string | undefined;
    firstName?: string | undefined;
    lastName?: string | undefined;
}
export interface OrganizationReq {
    type?: OrganizationType | undefined;
    name?: string | undefined;
    contact?: NameReq | undefined;
}
/** One of 'business' */
export declare enum OrganizationType {
    Business = "business"
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
export interface CaptchaReq {
    token?: string | undefined;
    action?: string | undefined;
}
export interface CrmLookupsRes {
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