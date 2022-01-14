export declare class AllocationsClient {
    private http;
    private baseUrl;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined;
    constructor(baseUrl?: string, http?: {
        fetch(url: RequestInfo, init?: RequestInit): Promise<Response>;
    });
    getLookupAllocationTypes(): Promise<NamedLookupRes[]>;
    protected processGetLookupAllocationTypes(response: Response): Promise<NamedLookupRes[]>;
    getLookupDonationItems(): Promise<NamedLookupRes[]>;
    protected processGetLookupDonationItems(response: Response): Promise<NamedLookupRes[]>;
    getLookupDonationTypes(): Promise<NamedLookupRes[]>;
    protected processGetLookupDonationTypes(response: Response): Promise<NamedLookupRes[]>;
    getLookupFundDimension1Options(): Promise<NamedLookupRes[]>;
    protected processGetLookupFundDimension1Options(response: Response): Promise<NamedLookupRes[]>;
    getLookupFundDimension2Options(): Promise<NamedLookupRes[]>;
    protected processGetLookupFundDimension2Options(response: Response): Promise<NamedLookupRes[]>;
    getLookupFundDimension3Options(): Promise<NamedLookupRes[]>;
    protected processGetLookupFundDimension3Options(response: Response): Promise<NamedLookupRes[]>;
    getLookupFundDimension4Options(): Promise<NamedLookupRes[]>;
    protected processGetLookupFundDimension4Options(response: Response): Promise<NamedLookupRes[]>;
    getLookupSponsorshipSchemes(): Promise<NamedLookupRes[]>;
    protected processGetLookupSponsorshipSchemes(response: Response): Promise<NamedLookupRes[]>;
    getAllLookups(criteria: LookupsCriteria): Promise<AllocationsLookupsRes>;
    protected processGetAllLookups(response: Response): Promise<AllocationsLookupsRes>;
}
export interface LookupRes {
    id?: string | undefined;
}
export interface NamedLookupRes extends LookupRes {
    name?: string | undefined;
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
export interface AllocationsLookupsRes extends LookupsRes {
    allocationTypes?: NamedLookupRes[] | undefined;
    donationItems?: NamedLookupRes[] | undefined;
    donationTypes?: NamedLookupRes[] | undefined;
    fundDimension1Options?: NamedLookupRes[] | undefined;
    fundDimension2Options?: NamedLookupRes[] | undefined;
    fundDimension3Options?: NamedLookupRes[] | undefined;
    fundDimension4Options?: NamedLookupRes[] | undefined;
    sponsorshipSchemes?: NamedLookupRes[] | undefined;
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