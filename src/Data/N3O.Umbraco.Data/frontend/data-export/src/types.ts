// Surfaces a backoffice toast via the native UMB_NOTIFICATION_CONTEXT (provided by the host element).
export type Notify = (color: 'positive' | 'warning' | 'danger', headline: string, message: string) => void;

export interface ContentType {
    alias: string;
    name: string;
}

export interface ContentMetadata {
    id: string;
    name: string;
    autoSelected: boolean;
    displayOrder: number;
    selected: boolean;
}

export interface ExportableProperty {
    alias: string;
    columnTitle: string;
    selected: boolean;
}

export interface ExportProgressResponse {
    isComplete: boolean;
    text: string;
    id: string;
}

export interface CreateExportResponse {
    id: string;
}
