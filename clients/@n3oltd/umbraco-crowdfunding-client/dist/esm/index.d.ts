export declare class CrowdfundingClient {
    private http;
    private baseUrl;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined;
    constructor(baseUrl?: string, http?: {
        fetch(url: RequestInfo, init?: RequestInit): Promise<Response>;
    });
    getContentPropertyValue(contentId: string, propertyAlias: string): Promise<ContentPropertyValueRes>;
    protected processGetContentPropertyValue(response: Response): Promise<ContentPropertyValueRes>;
    getNestedPropertySchema(contentId: string, propertyAlias: string): Promise<NestedSchemaRes>;
    protected processGetNestedPropertySchema(response: Response): Promise<NestedSchemaRes>;
    updateProperty(contentId: string, req: ContentPropertyReq): Promise<void>;
    protected processUpdateProperty(response: Response): Promise<void>;
    checkTitle(req: CreateFundraiserReq): Promise<boolean>;
    protected processCheckTitle(response: Response): Promise<boolean>;
    createFundraiser(req: CreateFundraiserReq): Promise<string>;
    protected processCreateFundraiser(response: Response): Promise<string>;
    updateFundraiserAllocation(contentId: string, req: UpdateFundraiserAllocationsReq): Promise<void>;
    protected processUpdateFundraiserAllocation(response: Response): Promise<void>;
    getPropertyTypes(): Promise<LookupRes[]>;
    protected processGetPropertyTypes(response: Response): Promise<LookupRes[]>;
}
export interface ContentPropertyValueRes {
    alias?: string | undefined;
    type?: PropertyType | undefined;
    boolean?: BooleanValueRes | undefined;
    cropper?: CropperValueRes | undefined;
    dateTime?: DateTimeValueRes | undefined;
    nested?: NestedValueRes | undefined;
    numeric?: NumericValueRes | undefined;
    raw?: RawValueRes | undefined;
    textarea?: TextareaValueRes | undefined;
    textBox?: TextBoxValueRes | undefined;
}
/** One of 'boolean', 'cropper', 'dateTime', 'nested', 'numeric', 'raw', 'textarea', 'textBox' */
export declare enum PropertyType {
    Boolean = "boolean",
    Cropper = "cropper",
    DateTime = "dateTime",
    Nested = "nested",
    Numeric = "numeric",
    Raw = "raw",
    Textarea = "textarea",
    TextBox = "textBox"
}
export interface BooleanValueRes {
    value?: boolean | undefined;
    configuration?: BooleanConfigurationRes | undefined;
}
export interface BooleanConfigurationRes {
    description?: string | undefined;
}
export interface CropperValueRes {
    image?: CropperSource | undefined;
    configuration?: CropperConfigurationRes | undefined;
}
export interface CropperSource {
    src?: string | undefined;
    mediaId?: string | undefined;
    filename?: string | undefined;
    width?: number;
    height?: number;
    altText?: string | undefined;
    crops?: Crop[] | undefined;
}
export interface Crop {
    x?: number;
    y?: number;
    width?: number;
    height?: number;
}
export interface CropperConfigurationRes {
    description?: string | undefined;
}
export interface DateTimeValueRes {
    value?: Date | undefined;
    configuration?: DateTimeConfigurationRes | undefined;
}
export interface DateTimeConfigurationRes {
    description?: string | undefined;
}
export interface NestedValueRes {
    items?: NestedItemRes[] | undefined;
    schema?: NestedSchemaRes | undefined;
    configuration?: NestedConfigurationRes | undefined;
}
export interface NestedItemRes {
    contentTypeAlias?: string | undefined;
    properties?: ContentPropertyValueRes[] | undefined;
}
export interface NestedSchemaRes {
    items?: NestedSchemaItemRes[] | undefined;
}
export interface NestedSchemaItemRes {
    contentTypeAlias?: string | undefined;
    properties?: NestedSchemaPropertyRes[] | undefined;
}
export interface NestedSchemaPropertyRes {
    alias?: string | undefined;
    type?: PropertyType | undefined;
}
export interface NestedConfigurationRes {
    description?: string | undefined;
    maximumItems?: number;
    minimumItems?: number;
}
export interface NumericValueRes {
    value?: number | undefined;
    configuration?: NumericConfigurationRes | undefined;
}
export interface NumericConfigurationRes {
    description?: string | undefined;
}
export interface RawValueRes {
    value?: HtmlEncodedString | undefined;
    configuration?: RawConfigurationRes | undefined;
}
export interface HtmlEncodedString {
}
export interface RawConfigurationRes {
    description?: string | undefined;
    maximumLength?: number;
}
export interface TextareaValueRes {
    value?: string | undefined;
    configuration?: TextareaConfigurationRes | undefined;
}
export interface TextareaConfigurationRes {
    description?: string | undefined;
    maximumLength?: number;
}
export interface TextBoxValueRes {
    value?: string | undefined;
    configuration?: TextBoxConfigurationRes | undefined;
}
export interface TextBoxConfigurationRes {
    description?: string | undefined;
    maximumLength?: number;
}
export interface ProblemDetails {
    type?: string | undefined;
    title?: string | undefined;
    status?: number | undefined;
    detail?: string | undefined;
    instance?: string | undefined;
    [key: string]: any;
}
export interface ContentPropertyReq {
    alias?: string | undefined;
    type?: PropertyType | undefined;
    boolean?: BooleanValueReq | undefined;
    cropper?: CropperValueReq | undefined;
    dateTime?: DateTimeValueReq | undefined;
    nested?: NestedValueReq | undefined;
    numeric?: NumericValueReq | undefined;
    raw?: RawValueReq | undefined;
    textarea?: TextareaValueReq | undefined;
    textBox?: TextBoxValueReq | undefined;
}
export interface BooleanValueReq {
    value?: boolean | undefined;
}
export interface CropperValueReq {
    type?: PropertyType | undefined;
    storageToken?: string | undefined;
    shape?: CropShape | undefined;
    circle?: CircleCropReq | undefined;
    rectangle?: RectangleCropReq | undefined;
}
export interface ByteSize {
    bits?: number;
    bytes?: number;
    kilobytes?: number;
    megabytes?: number;
    gigabytes?: number;
    terabytes?: number;
    largestWholeNumberSymbol?: string | undefined;
    largestWholeNumberFullWord?: string | undefined;
    largestWholeNumberValue?: number;
}
/** One of 'circle', 'rectangle' */
export declare enum CropShape {
    Circle = "circle",
    Rectangle = "rectangle"
}
export interface CircleCropReq {
    center?: PointReq | undefined;
    radius?: number | undefined;
}
export interface PointReq {
    x?: number | undefined;
    y?: number | undefined;
}
export interface RectangleCropReq {
    bottomLeft?: PointReq | undefined;
    topRight?: PointReq | undefined;
}
export interface DateTimeValueReq {
    value?: Date | undefined;
}
export interface NestedValueReq {
    items?: NestedItemReq[] | undefined;
}
export interface NestedItemReq {
    contentTypeAlias?: string | undefined;
    properties?: ContentPropertyReq[] | undefined;
}
export interface NumericValueReq {
    value?: number | undefined;
}
export interface RawValueReq {
    value?: string | undefined;
}
export interface TextareaValueReq {
    value?: string | undefined;
}
export interface TextBoxValueReq {
    value?: string | undefined;
}
export interface AutoPropertyOfValueReq {
    value?: ValueReq | undefined;
}
export interface ValueReq {
    type?: PropertyType | undefined;
}
export interface CreateFundraiserReq {
    title?: string | undefined;
    slug?: string | undefined;
    campaignId?: string | undefined;
    endDate?: Date | undefined;
    allocations?: FundraiserAllocationReq[] | undefined;
}
export interface FundraiserAllocationReq {
    amount?: number | undefined;
    goalId?: string | undefined;
    feedbackNewCustomFields?: FeedbackNewCustomFieldsReq | undefined;
}
export interface FeedbackNewCustomFieldsReq {
    entries?: FeedbackNewCustomFieldReq[] | undefined;
}
export interface FeedbackNewCustomFieldReq {
    alias?: string | undefined;
    bool?: boolean | undefined;
    date?: Date | undefined;
    text?: string | undefined;
}
export interface UpdateFundraiserAllocationsReq {
    allocations?: UpdateFundraiserAllocationReq[] | undefined;
}
export interface UpdateFundraiserAllocationReq {
    allocationId?: string | undefined;
    allocation?: FundraiserAllocationReq | undefined;
}
export interface LookupRes {
    id?: string | undefined;
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