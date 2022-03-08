export declare class StripeClient {
    private http;
    private baseUrl;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined;
    constructor(baseUrl?: string, http?: {
        fetch(url: RequestInfo, init?: RequestInit): Promise<Response>;
    });
    createPaymentIntent(flowId: string, req: PaymentIntentReq): Promise<PaymentFlowResOfStripePayment>;
    protected processCreatePaymentIntent(response: Response): Promise<PaymentFlowResOfStripePayment>;
    createSetupIntent(flowId: string, req: SetupIntentReq): Promise<PaymentFlowResOfStripeCredential>;
    protected processCreateSetupIntent(response: Response): Promise<PaymentFlowResOfStripeCredential>;
    confirmPaymentIntent(flowId: string): Promise<PaymentFlowResOfStripePayment>;
    protected processConfirmPaymentIntent(response: Response): Promise<PaymentFlowResOfStripePayment>;
    confirmSetupIntent(flowId: string): Promise<PaymentFlowResOfStripeCredential>;
    protected processConfirmSetupIntent(response: Response): Promise<PaymentFlowResOfStripeCredential>;
}
export interface PaymentFlowResOfStripePayment {
    flowRevision?: number;
    result?: StripePayment | undefined;
}
export interface StripePayment {
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
    stripeChargeId?: string | undefined;
    stripeCustomerId?: string | undefined;
    stripeDeclineCode?: string | undefined;
    stripeErrorCode?: string | undefined;
    stripeErrorMessage?: string | undefined;
    stripePaymentIntentId?: string | undefined;
    stripePaymentIntentClientSecret?: string | undefined;
    stripePaymentMethodId?: string | undefined;
    actionRequired?: boolean;
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
export interface ProblemDetails {
    type?: string | undefined;
    title?: string | undefined;
    status?: number | undefined;
    detail?: string | undefined;
    instance?: string | undefined;
}
export interface PaymentIntentReq {
    paymentMethodId?: string | undefined;
    customerId?: string | undefined;
    value?: MoneyReq | undefined;
}
export interface MoneyReq {
    amount?: number | undefined;
    currency?: string | undefined;
}
export interface PaymentFlowResOfStripeCredential {
    flowRevision?: number;
    result?: StripeCredential | undefined;
}
export interface StripeCredential {
    advancePayment?: Payment | undefined;
    setupAt?: Date | undefined;
    isSetUp?: boolean;
    type?: PaymentObjectType | undefined;
    completeAt?: Date | undefined;
    errorAt?: Date | undefined;
    errorMessage?: string | undefined;
    exceptionDetails?: string | undefined;
    status?: PaymentObjectStatus | undefined;
    stripeMandateId?: string | undefined;
    stripeCustomerId?: string | undefined;
    stripeDeclineCode?: string | undefined;
    stripeErrorCode?: string | undefined;
    stripeErrorMessage?: string | undefined;
    stripeSetupIntentId?: string | undefined;
    stripeSetupIntentClientSecret?: string | undefined;
    stripePaymentMethodId?: string | undefined;
    actionRequired?: boolean;
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
    type?: PaymentObjectType | undefined;
}
export interface SetupIntentReq {
    paymentMethodId?: string | undefined;
    customerId?: string | undefined;
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