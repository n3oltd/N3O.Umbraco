export declare class AllocationsClient {
    private http;
    private baseUrl;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined;
    constructor(baseUrl?: string, http?: {
        fetch(url: RequestInfo, init?: RequestInit): Promise<Response>;
    });
    getFundStructure(): Promise<FundStructure>;
    protected processGetFundStructure(response: Response): Promise<FundStructure>;
    getLookupAllocationTypes(): Promise<NamedLookupRes[]>;
    protected processGetLookupAllocationTypes(response: Response): Promise<NamedLookupRes[]>;
    getLookupDonationItems(): Promise<DonationItemRes[]>;
    protected processGetLookupDonationItems(response: Response): Promise<DonationItemRes[]>;
    getLookupFundDimension1Options(): Promise<FundDimensionOptionRes[]>;
    protected processGetLookupFundDimension1Options(response: Response): Promise<FundDimensionOptionRes[]>;
    getLookupFundDimension2Options(): Promise<FundDimensionOptionRes[]>;
    protected processGetLookupFundDimension2Options(response: Response): Promise<FundDimensionOptionRes[]>;
    getLookupFundDimension3Options(): Promise<FundDimensionOptionRes[]>;
    protected processGetLookupFundDimension3Options(response: Response): Promise<FundDimensionOptionRes[]>;
    getLookupFundDimension4Options(): Promise<FundDimensionOptionRes[]>;
    protected processGetLookupFundDimension4Options(response: Response): Promise<FundDimensionOptionRes[]>;
    getLookupGivingTypes(): Promise<NamedLookupRes[]>;
    protected processGetLookupGivingTypes(response: Response): Promise<NamedLookupRes[]>;
    getAllLookups(criteria: LookupsCriteria): Promise<AllocationsLookupRes>;
    protected processGetAllLookups(response: Response): Promise<AllocationsLookupRes>;
}
export interface FundStructure {
    dimension1?: string | undefined;
    dimension2?: string | undefined;
    dimension3?: string | undefined;
    dimension4?: string | undefined;
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
export interface DonationItemRes {
    name?: string | undefined;
    id?: string | undefined;
    allowedGivingTypes?: GivingType[] | undefined;
    dimension1Options?: FundDimensionOptionRes[] | undefined;
    dimension2Options?: FundDimensionOptionRes[] | undefined;
    dimension3Options?: FundDimensionOptionRes[] | undefined;
    dimension4Options?: FundDimensionOptionRes[] | undefined;
    pricing?: PricingRes | undefined;
}
/** One of 'donation', 'regularGiving' */
export declare enum GivingType {
    Donation = "donation",
    RegularGiving = "regularGiving"
}
export interface FundDimensionOptionRes {
    name?: string | undefined;
    id?: string | undefined;
    isUnrestricted?: boolean;
}
export interface PricingRes {
    value?: MoneyRes | undefined;
    locked?: boolean;
    priceRules?: PricingRuleRes[] | undefined;
}
export interface MoneyRes {
    amount?: number;
    currency?: string | undefined;
    text?: string | undefined;
}
export interface PricingRuleRes {
    value?: MoneyRes | undefined;
    locked?: boolean;
    dimension1Options?: string[] | undefined;
    dimension2Options?: string[] | undefined;
    dimension3Options?: string[] | undefined;
    dimension4Options?: string[] | undefined;
}
export interface AllocationsLookupRes {
    allocationTypes?: NamedLookupRes[] | undefined;
    donationItems?: DonationItemRes[] | undefined;
    givingTypes?: NamedLookupRes[] | undefined;
    fundDimension1Options?: FundDimensionOptionRes[] | undefined;
    fundDimension2Options?: FundDimensionOptionRes[] | undefined;
    fundDimension3Options?: FundDimensionOptionRes[] | undefined;
    fundDimension4Options?: FundDimensionOptionRes[] | undefined;
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