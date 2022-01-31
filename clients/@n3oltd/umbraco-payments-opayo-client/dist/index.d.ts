export declare class OpayoClient {
    private http;
    private baseUrl;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined;
    constructor(baseUrl?: string, http?: {
        fetch(url: RequestInfo, init?: RequestInit): Promise<Response>;
    });
    getMerchantSessionKey(): Promise<void>;
    protected processGetMerchantSessionKey(response: Response): Promise<void>;
    process(flowId: string, req: OpayoPaymentReq): Promise<PaymentFlowResOfOpayoPayment>;
    protected processProcess(response: Response): Promise<PaymentFlowResOfOpayoPayment>;
    authorize(flowId: string, cRes: string | null | undefined, threeDsSessionData: string | null | undefined): Promise<ThreeDSecureStatus>;
    protected processAuthorize(response: Response): Promise<ThreeDSecureStatus>;
    findPaymentMethods(req: PaymentMethodCriteria): Promise<PaymentMethodRes[]>;
    protected processFindPaymentMethods(response: Response): Promise<PaymentMethodRes[]>;
    getLookupPaymentMethods(): Promise<PaymentMethodRes[]>;
    protected processGetLookupPaymentMethods(response: Response): Promise<PaymentMethodRes[]>;
    getAllLookups(criteria: LookupsCriteria): Promise<PaymentsLookupsRes>;
    protected processGetAllLookups(response: Response): Promise<PaymentsLookupsRes>;
}
export interface ProblemDetails {
    type?: string | undefined;
    title?: string | undefined;
    status?: number | undefined;
    detail?: string | undefined;
    instance?: string | undefined;
}
export interface PaymentFlowResOfOpayoPayment {
    flowRevision?: number;
    result?: OpayoPayment | undefined;
}
export interface OpayoPayment {
    type?: PaymentObjectType | undefined;
    declineReason?: string | undefined;
    isDeclined?: boolean;
    isPaid?: boolean;
    isFailed?: boolean;
    requireThreeDSecure?: boolean;
    /** A well formed guid */
    id?: string | undefined;
    status?: PaymentObjectStatus | undefined;
    transactionId?: string | undefined;
    opayoErrorCode?: number | undefined;
    opayoErrorMessage?: string | undefined;
    opayoStatusDetail?: string | undefined;
    opayoRetrievalReference?: number | undefined;
    bankAuthorisationCode?: string | undefined;
    threeDSecureUrl?: string | undefined;
    acsTransId?: string | undefined;
    cReq?: string | undefined;
    callbackUrl?: string | undefined;
    threeDSecureCompleted?: boolean;
    method?: string | undefined;
}
/** One of 'credential', 'payment' */
export declare enum PaymentObjectType {
    Credential = "credential",
    Payment = "payment"
}
/** One of 'complete', 'failed', 'inProgress' */
export declare enum PaymentObjectStatus {
    Complete = "complete",
    Failed = "failed",
    InProgress = "inProgress"
}
export interface OpayoPaymentReq {
    cardIdentifier?: string | undefined;
    merchantSessionKey?: string | undefined;
    value?: MoneyReq | undefined;
    callbackUrl?: string | undefined;
    browserParameters?: BrowserParametersReq | undefined;
    challengeWindowSize?: ChallengeWindowSize | undefined;
}
export interface MoneyReq {
    amount?: number | undefined;
    currency?: string | undefined;
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
export interface BrowserParametersReq {
    colourDepth?: number | undefined;
    javaEnabled?: boolean | undefined;
    javaScriptEnabled?: boolean | undefined;
    screenHeight?: number | undefined;
    screenWidth?: number | undefined;
    utcOffsetMinutes?: number | undefined;
}
/** One of 'small', 'medium', 'large', 'extraLarge', 'fullScreen' */
export declare enum ChallengeWindowSize {
    Small = "small",
    Medium = "medium",
    Large = "large",
    ExtraLarge = "extraLarge",
    FullScreen = "fullScreen"
}
export interface ThreeDSecureStatus {
    completed?: boolean;
    success?: boolean;
}
export interface PaymentMethodRes {
    name?: string | undefined;
    id?: string | undefined;
}
export interface PaymentMethodCriteria {
    country?: string | undefined;
    currency?: string | undefined;
}
export interface PaymentsLookupsRes {
    paymentMethods?: PaymentMethodRes[] | undefined;
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