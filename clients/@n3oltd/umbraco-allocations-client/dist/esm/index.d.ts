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
    getLookupDonationTypes(): Promise<NamedLookupRes[]>;
    protected processGetLookupDonationTypes(response: Response): Promise<NamedLookupRes[]>;
    getLookupFundDimension1Options(): Promise<FundDimensionOptionRes[]>;
    protected processGetLookupFundDimension1Options(response: Response): Promise<FundDimensionOptionRes[]>;
    getLookupFundDimension2Options(): Promise<FundDimensionOptionRes[]>;
    protected processGetLookupFundDimension2Options(response: Response): Promise<FundDimensionOptionRes[]>;
    getLookupFundDimension3Options(): Promise<FundDimensionOptionRes[]>;
    protected processGetLookupFundDimension3Options(response: Response): Promise<FundDimensionOptionRes[]>;
    getLookupFundDimension4Options(): Promise<FundDimensionOptionRes[]>;
    protected processGetLookupFundDimension4Options(response: Response): Promise<FundDimensionOptionRes[]>;
    getLookupSponsorshipSchemes(): Promise<NamedLookupRes[]>;
    protected processGetLookupSponsorshipSchemes(response: Response): Promise<NamedLookupRes[]>;
    getAllLookups(criteria: LookupsCriteria): Promise<AllocationsLookupsRes>;
    protected processGetAllLookups(response: Response): Promise<AllocationsLookupsRes>;
}
export interface Value {
}
export interface FundStructure extends Value {
    dimension1?: Dimension1 | undefined;
    dimension2?: Dimension2 | undefined;
    dimension3?: Dimension3 | undefined;
    dimension4?: Dimension4 | undefined;
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
export interface UmbracoContentOfFundDimension1 extends Value {
    content?: IPublishedContent | undefined;
}
export interface UmbracoContentOfFundDimension2Option extends Value {
    content?: IPublishedContent | undefined;
}
export interface UmbracoContentOfFundDimension2 extends Value {
    content?: IPublishedContent | undefined;
}
export interface UmbracoContentOfFundDimension3Option extends Value {
    content?: IPublishedContent | undefined;
}
export interface UmbracoContentOfFundDimension3 extends Value {
    content?: IPublishedContent | undefined;
}
export interface UmbracoContentOfFundDimension4Option extends Value {
    content?: IPublishedContent | undefined;
}
export interface UmbracoContentOfFundDimension4 extends Value {
    content?: IPublishedContent | undefined;
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
export interface DonationItemRes extends NamedLookupRes {
    allowSingleDonations?: boolean;
    allowRegularDonations?: boolean;
    free?: boolean;
    price?: MoneyRes | undefined;
    dimension1Options?: FundDimensionOptionRes[] | undefined;
    dimension2Options?: FundDimensionOptionRes[] | undefined;
    dimension3Options?: FundDimensionOptionRes[] | undefined;
    dimension4Options?: FundDimensionOptionRes[] | undefined;
}
export interface MoneyRes {
    amount?: number;
    currency?: Currency | undefined;
    text?: string | undefined;
}
export interface UmbracoContentOfCurrency extends Value {
    content?: IPublishedContent | undefined;
}
export interface FundDimensionOptionRes extends NamedLookupRes {
    isUnrestricted?: boolean;
}
export interface LookupsRes {
}
export interface AllocationsLookupsRes extends LookupsRes {
    allocationTypes?: NamedLookupRes[] | undefined;
    donationItems?: DonationItemRes[] | undefined;
    donationTypes?: NamedLookupRes[] | undefined;
    fundDimension1Options?: FundDimensionOptionRes[] | undefined;
    fundDimension2Options?: FundDimensionOptionRes[] | undefined;
    fundDimension3Options?: FundDimensionOptionRes[] | undefined;
    fundDimension4Options?: FundDimensionOptionRes[] | undefined;
    sponsorshipSchemes?: NamedLookupRes[] | undefined;
}
export interface LookupsCriteria {
    types?: Types[] | undefined;
}
export interface Anonymous7 extends UmbracoContentOfFundDimension1 {
    id?: string | undefined;
    name?: string | undefined;
}
export interface Anonymous extends Anonymous7 {
    isActive?: boolean;
    options?: Options[] | undefined;
}
export interface Dimension1 extends Anonymous {
}
export interface Anonymous8 extends UmbracoContentOfFundDimension2 {
    id?: string | undefined;
    name?: string | undefined;
}
export interface Anonymous2 extends Anonymous8 {
    isActive?: boolean;
    options?: Options2[] | undefined;
}
export interface Dimension2 extends Anonymous2 {
}
export interface Anonymous9 extends UmbracoContentOfFundDimension3 {
    id?: string | undefined;
    name?: string | undefined;
}
export interface Anonymous3 extends Anonymous9 {
    isActive?: boolean;
    options?: Options3[] | undefined;
}
export interface Dimension3 extends Anonymous3 {
}
export interface Anonymous10 extends UmbracoContentOfFundDimension4 {
    id?: string | undefined;
    name?: string | undefined;
}
export interface Anonymous4 extends Anonymous10 {
    isActive?: boolean;
    options?: Options4[] | undefined;
}
export interface Dimension4 extends Anonymous4 {
}
export interface Anonymous5 extends UmbracoContentOfCurrency {
    id?: string | undefined;
    name?: string | undefined;
}
export interface Currency extends Anonymous5 {
    symbol?: string | undefined;
    isBaseCurrency?: boolean;
    decimalDigits?: number;
}
export interface Anonymous6 extends Value {
    id?: string | undefined;
}
export interface Types extends Anonymous6 {
    lookupType?: string | undefined;
}
export interface Anonymous15 extends UmbracoContentOfFundDimension1Option {
    id?: string | undefined;
    name?: string | undefined;
}
export interface Anonymous11 extends Anonymous15 {
    isUnrestricted?: boolean;
}
export interface Options extends Anonymous11 {
}
export interface Anonymous16 extends UmbracoContentOfFundDimension2Option {
    id?: string | undefined;
    name?: string | undefined;
}
export interface Anonymous12 extends Anonymous16 {
    isUnrestricted?: boolean;
}
export interface Options2 extends Anonymous12 {
}
export interface Anonymous17 extends UmbracoContentOfFundDimension3Option {
    id?: string | undefined;
    name?: string | undefined;
}
export interface Anonymous13 extends Anonymous17 {
    isUnrestricted?: boolean;
}
export interface Options3 extends Anonymous13 {
}
export interface Anonymous18 extends UmbracoContentOfFundDimension4Option {
    id?: string | undefined;
    name?: string | undefined;
}
export interface Anonymous14 extends Anonymous18 {
    isUnrestricted?: boolean;
}
export interface Options4 extends Anonymous14 {
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