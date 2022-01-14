export declare class CheckoutClient {
    private http;
    private baseUrl;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined;
    constructor(baseUrl?: string, http?: {
        fetch(url: RequestInfo, init?: RequestInit): Promise<Response>;
    });
    getLookupCountries(): Promise<CountryRes[]>;
    protected processGetLookupCountries(response: Response): Promise<CountryRes[]>;
    getAllLookups(criteria: LookupsCriteria): Promise<CheckoutLookupsRes>;
    protected processGetAllLookups(response: Response): Promise<CheckoutLookupsRes>;
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
export interface ProblemDetails {
    type?: string | undefined;
    title?: string | undefined;
    status?: number | undefined;
    detail?: string | undefined;
    instance?: string | undefined;
}
export interface LookupsRes {
}
export interface CheckoutLookupsRes extends LookupsRes {
    countries?: CountryRes[] | undefined;
}
export interface LookupsCriteria {
    types?: Types[] | undefined;
}
export interface Value {
}
export interface Anonymous extends Value {
    id?: string | undefined;
}
export interface Types extends Anonymous {
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