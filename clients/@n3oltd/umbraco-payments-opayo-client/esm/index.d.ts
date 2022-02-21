export declare class OpayoClient {
    private http;
    private baseUrl;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined;
    constructor(baseUrl?: string, http?: {
        fetch(url: RequestInfo, init?: RequestInit): Promise<Response>;
    });
    completeThreeDSecureChallenge(flowId: string, cRes: string | null | undefined, threeDsSessionData: string | null | undefined): Promise<void>;
    protected processCompleteThreeDSecureChallenge(response: Response): Promise<void>;
    getMerchantSessionKey(): Promise<MerchantSessionKeyRes>;
    protected processGetMerchantSessionKey(response: Response): Promise<MerchantSessionKeyRes>;
    chargeCard(flowId: string, req: ChargeCardReq): Promise<PaymentFlowResOfOpayoPayment>;
    protected processChargeCard(response: Response): Promise<PaymentFlowResOfOpayoPayment>;
    storeCard(flowId: string, req: StoreCardReq): Promise<PaymentFlowResOfOpayoCredential>;
    protected processStoreCard(response: Response): Promise<PaymentFlowResOfOpayoCredential>;
}
export interface ProblemDetails {
    type?: string | undefined;
    title?: string | undefined;
    status?: number | undefined;
    detail?: string | undefined;
    instance?: string | undefined;
}
export interface MerchantSessionKeyRes {
    key?: string | undefined;
    expiresAt?: Date;
}
export interface PaymentFlowResOfOpayoPayment {
    flowRevision?: number;
    result?: OpayoPayment | undefined;
}
export interface OpayoPayment {
    card?: CardPayment | undefined;
    paidAt?: Date | undefined;
    declinedAt?: Date | undefined;
    declinedReason?: string | undefined;
    isDeclined?: boolean;
    isPaid?: boolean;
    completeAt?: Date | undefined;
    errorAt?: Date | undefined;
    errorMessage?: string | undefined;
    exceptionDetails?: string | undefined;
    status?: PaymentObjectStatus | undefined;
    opayoTransactionId?: string | undefined;
    opayoStatusCode?: number | undefined;
    opayoStatusDetail?: string | undefined;
    opayoErrorCode?: number | undefined;
    opayoErrorMessage?: string | undefined;
    opayoBankAuthorisationCode?: string | undefined;
    opayoRetrievalReference?: number | undefined;
    returnUrl?: string | undefined;
    method?: string | undefined;
}
export interface CardPayment {
    threeDSecureRequired?: boolean;
    threeDSecureCompleted?: boolean;
    threeDSecureChallengeUrl?: string | undefined;
    threeDSecureAcsTransId?: string | undefined;
    threeDSecureCReq?: string | undefined;
    threeDSecureCRes?: string | undefined;
}
/** One of 'credential', 'payment' */
export declare enum PaymentObjectType {
    Credential = "credential",
    Payment = "payment"
}
/** One of 'complete', 'error', 'inProgress' */
export declare enum PaymentObjectStatus {
    Complete = "complete",
    Error = "error",
    InProgress = "inProgress"
}
export interface ChargeCardReq {
    merchantSessionKey?: string | undefined;
    cardIdentifier?: string | undefined;
    value?: MoneyReq | undefined;
    browserParameters?: BrowserParametersReq | undefined;
    challengeWindowSize?: ChallengeWindowSize | undefined;
    returnUrl?: string | undefined;
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
export interface PaymentFlowResOfOpayoCredential {
    flowRevision?: number;
    result?: OpayoCredential | undefined;
}
export interface OpayoCredential {
    setupAt?: Date | undefined;
    isSetUp?: boolean;
    completeAt?: Date | undefined;
    errorAt?: Date | undefined;
    errorMessage?: string | undefined;
    exceptionDetails?: string | undefined;
    status?: PaymentObjectStatus | undefined;
    method?: string | undefined;
}
export interface Payment {
    completeAt?: Date | undefined;
    errorAt?: Date | undefined;
    errorMessage?: string | undefined;
    exceptionDetails?: string | undefined;
    status?: PaymentObjectStatus | undefined;
    card?: CardPayment | undefined;
    paidAt?: Date | undefined;
    declinedAt?: Date | undefined;
    declinedReason?: string | undefined;
    isDeclined?: boolean;
    isPaid?: boolean;
}
export interface StoreCardReq {
    advancePayment?: ChargeCardReq | undefined;
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