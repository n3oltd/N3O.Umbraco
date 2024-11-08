export declare class PayPalClient {
    private http;
    private baseUrl;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined;
    constructor(baseUrl?: string, http?: {
        fetch(url: RequestInfo, init?: RequestInit): Promise<Response>;
    });
    capture(flowId: string, req: PayPalTransactionReq): Promise<PaymentFlowResOfPayPalPayment>;
    protected processCapture(response: Response): Promise<PaymentFlowResOfPayPalPayment>;
    captureSubscription(flowId: string, req: PayPalSubscriptionReq): Promise<PaymentFlowResOfPayPalCredential>;
    protected processCaptureSubscription(response: Response): Promise<PaymentFlowResOfPayPalCredential>;
    createSubscription(flowId: string, req: PayPalCreateSubscriptionReq): Promise<Subscription>;
    protected processCreateSubscription(response: Response): Promise<Subscription>;
    createPlan(flowId: string): Promise<PayPalCreatePlanRes>;
    protected processCreatePlan(response: Response): Promise<PayPalCreatePlanRes>;
}
export interface PaymentFlowResOfPayPalPayment {
    flowRevision?: number;
    result?: PayPalPayment | undefined;
}
export interface PayPalPayment {
    card?: CardPayment | undefined;
    paidAt?: string | undefined;
    declinedAt?: string | undefined;
    declinedReason?: string | undefined;
    isDeclined?: boolean;
    isPaid?: boolean;
    type?: PaymentObjectType | undefined;
    completeAt?: string | undefined;
    errorAt?: string | undefined;
    errorMessage?: string | undefined;
    exceptionDetails?: string | undefined;
    status?: PaymentObjectStatus | undefined;
    method?: string | undefined;
    clock?: IClock | undefined;
    payPalEmail?: string | undefined;
    payPalTransactionId?: string | undefined;
}
export interface CardPayment {
    threeDSecureRequired?: boolean;
    threeDSecureCompleted?: boolean;
    threeDSecureV1?: ThreeDSecureV1 | undefined;
    threeDSecureV2?: ThreeDSecureV2 | undefined;
}
export interface ThreeDSecureV1 {
    acsUrl?: string | undefined;
    md?: string | undefined;
    paReq?: string | undefined;
    paRes?: string | undefined;
}
export interface ThreeDSecureV2 {
    acsUrl?: string | undefined;
    acsTransId?: string | undefined;
    sessionData?: string | undefined;
    cReq?: string | undefined;
    cRes?: string | undefined;
    html?: string | undefined;
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
/** Represents a clock which can return the current time as an Instant. */
export interface IClock {
}
export interface ProblemDetails {
    type?: string | undefined;
    title?: string | undefined;
    status?: number | undefined;
    detail?: string | undefined;
    instance?: string | undefined;
    [key: string]: any;
}
export interface PayPalTransactionReq {
    email?: string | undefined;
    authorizationId?: string | undefined;
}
export interface PaymentFlowResOfPayPalCredential {
    flowRevision?: number;
    result?: PayPalCredential | undefined;
}
export interface PayPalCredential {
    advancePayment?: Payment | undefined;
    setupAt?: string | undefined;
    isSetUp?: boolean;
    type?: PaymentObjectType | undefined;
    completeAt?: string | undefined;
    errorAt?: string | undefined;
    errorMessage?: string | undefined;
    exceptionDetails?: string | undefined;
    status?: PaymentObjectStatus | undefined;
    method?: string | undefined;
    clock?: IClock | undefined;
    payPalSubscriptionId?: string | undefined;
    payPalSubscriptionReason?: string | undefined;
}
export interface Payment {
    completeAt?: string | undefined;
    errorAt?: string | undefined;
    errorMessage?: string | undefined;
    exceptionDetails?: string | undefined;
    status?: PaymentObjectStatus | undefined;
    type?: PaymentObjectType | undefined;
    method?: string | undefined;
    clock?: IClock | undefined;
    card?: CardPayment | undefined;
    paidAt?: string | undefined;
    declinedAt?: string | undefined;
    declinedReason?: string | undefined;
    isDeclined?: boolean;
    isPaid?: boolean;
}
export interface PayPalSubscriptionReq {
    subscriptionId?: string | undefined;
    reason?: string | undefined;
}
export interface Subscription {
    id?: string | undefined;
    planId?: string | undefined;
    startTime?: Date;
    applicationContext?: ApplicationContext | undefined;
    quantity?: string | undefined;
    shippingAmount?: Amount | undefined;
    subscriber?: Subscriber | undefined;
    billingInfo?: BillingInfo | undefined;
    createdAt?: Date;
    updatedAt?: Date;
    links?: Link[] | undefined;
    status?: string | undefined;
    statusChangeNote?: string | undefined;
    statusUpdatedAt?: Date;
}
export interface ApplicationContext {
    returnUrl?: string | undefined;
    cancelUrl?: string | undefined;
    paymentMethod?: PaymentMethod2 | undefined;
}
export interface PaymentMethod2 {
    payerSelected?: string | undefined;
    payeePreferred?: string | undefined;
}
export interface Amount {
    currencyCode?: string | undefined;
    value?: number;
}
export interface Subscriber {
    shippingAddress?: ShippingAddress | undefined;
    name?: Name | undefined;
    emailAddress?: string | undefined;
    payerId?: string | undefined;
}
export interface ShippingAddress {
    name?: Name | undefined;
    address?: Address | undefined;
}
export interface Name {
    title?: string | undefined;
    firstName?: string | undefined;
    lastName?: string | undefined;
}
export interface Address {
    line1?: string | undefined;
    line2?: string | undefined;
    line3?: string | undefined;
    locality?: string | undefined;
    administrativeArea?: string | undefined;
    postalCode?: string | undefined;
    country?: string | undefined;
}
export interface BillingInfo {
    address?: Address | undefined;
    email?: Email | undefined;
    name?: Name | undefined;
    telephone?: Telephone | undefined;
}
export interface Email {
    address?: string | undefined;
}
export interface Telephone {
    country?: string | undefined;
    number?: string | undefined;
}
export interface Link {
    href?: string | undefined;
    rel?: string | undefined;
    method?: string | undefined;
    encType?: string | undefined;
}
export interface PayPalCreateSubscriptionReq {
    returnUrl?: string | undefined;
    cancelUrl?: string | undefined;
}
export interface PayPalCreatePlanRes {
    planId?: string | undefined;
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