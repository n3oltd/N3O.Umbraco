export declare class PayPalClient {
    private http;
    private baseUrl;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined;
    constructor(baseUrl?: string, http?: {
        fetch(url: RequestInfo, init?: RequestInit): Promise<Response>;
    });
    capture(flowId: string, req: PayPalTransactionReq): Promise<PaymentFlowResOfPayPalPayment>;
    protected processCapture(response: Response): Promise<PaymentFlowResOfPayPalPayment>;
}
export interface PaymentFlowResOfPayPalPayment {
    flowRevision?: number;
    result?: PayPalPayment | undefined;
}
export interface PayPalPayment {
    card?: CardPayment | undefined;
    paidAt?: Date | undefined;
    declinedAt?: Date | undefined;
    declinedReason?: string | undefined;
    isDeclined?: boolean;
    isPaid?: boolean;
    type?: PaymentObjectType | undefined;
    completeAt?: Date | undefined;
    errorAt?: Date | undefined;
    errorMessage?: string | undefined;
    exceptionDetails?: string | undefined;
    status?: PaymentObjectStatus | undefined;
    payPalEmail?: string | undefined;
    payPalTransactionId?: string | undefined;
    method?: string | undefined;
}
export interface CardPayment {
    threeDSecureRequired?: boolean;
    threeDSecureCompleted?: boolean;
    threeDSecureUrl?: string | undefined;
    challenge?: ChallengeThreeDSecure | undefined;
    fallback?: FallbackThreeDSecure | undefined;
}
export interface ChallengeThreeDSecure {
    acsTransId?: string | undefined;
    cReq?: string | undefined;
    cRes?: string | undefined;
}
export interface FallbackThreeDSecure {
    termUrl?: string | undefined;
    paReq?: string | undefined;
    paRes?: string | undefined;
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
export interface ProblemDetails {
    type?: string | undefined;
    title?: string | undefined;
    status?: number | undefined;
    detail?: string | undefined;
    instance?: string | undefined;
}
export interface PayPalTransactionReq {
    email?: string | undefined;
    authorizationId?: string | undefined;
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