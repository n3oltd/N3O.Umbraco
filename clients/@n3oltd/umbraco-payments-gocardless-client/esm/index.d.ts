export declare class GoCardlessClient {
    private http;
    private baseUrl;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined;
    constructor(baseUrl?: string, http?: {
        fetch(url: RequestInfo, init?: RequestInit): Promise<Response>;
    });
    beginRedirectFlow(flowId: string, req: RedirectFlowReq): Promise<PaymentFlowResOfGoCardlessCredential>;
    protected processBeginRedirectFlow(response: Response): Promise<PaymentFlowResOfGoCardlessCredential>;
    completeRedirectFlow(flowId: string): Promise<void>;
    protected processCompleteRedirectFlow(response: Response): Promise<void>;
}
export interface PaymentFlowResOfGoCardlessCredential {
    flowRevision?: number;
    result?: GoCardlessCredential | undefined;
}
export interface GoCardlessCredential {
    advancePayment?: Payment | undefined;
    setupAt?: Date | undefined;
    isSetUp?: boolean;
    completeAt?: Date | undefined;
    errorAt?: Date | undefined;
    errorMessage?: string | undefined;
    exceptionDetails?: string | undefined;
    status?: PaymentObjectStatus | undefined;
    method?: string | undefined;
    goCardlessSessionToken?: string | undefined;
    goCardlessRedirectFlowId?: string | undefined;
    goCardlessCustomerId?: string | undefined;
    goCardlessMandateId?: string | undefined;
    returnUrl?: string | undefined;
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
/** One of 'complete', 'error', 'inProgress' */
export declare enum PaymentObjectStatus {
    Complete = "complete",
    Error = "error",
    InProgress = "inProgress"
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
export interface ProblemDetails {
    type?: string | undefined;
    title?: string | undefined;
    status?: number | undefined;
    detail?: string | undefined;
    instance?: string | undefined;
}
export interface RedirectFlowReq {
    returnUrl?: string | undefined;
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