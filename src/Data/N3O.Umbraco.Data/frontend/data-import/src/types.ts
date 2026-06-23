// Surfaces a backoffice toast via the native UMB_NOTIFICATION_CONTEXT (provided by the host element).
export type Notify = (color: 'positive' | 'warning' | 'danger', headline: string, message: string) => void;

export interface ContentType {
    alias: string;
    name: string;
}

export interface DatePattern {
    id: string;
    name: string;
}

export interface ImportableProperty {
    alias: string;
    columnTitle: string;
    selected: boolean;
}
