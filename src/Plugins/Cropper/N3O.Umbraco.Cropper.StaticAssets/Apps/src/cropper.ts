// ┌──────────────────────────────────────────────────────────────────────────────────────────┐
// │ PENDING DECISION (Umbraco 17 migration): this editor was ported 1:1 from AngularJS and      │
// │ still relies on cropperjs + Formstone (which needs a global jQuery the v17 backoffice no     │
// │ longer ships). It MAY OR MAY NOT be re-migrated to use Umbraco's native media/image picker   │
// │ + built-in Image Cropper instead of the bundled libs + custom upload widget. Not decided     │
// │ yet — do NOT rewrite this until Talha confirms the direction. See MIGRATION_PLAN.md.          │
// │                                                                                              │
// │ ⚠ FLAG: Formstone upload widget requires global jQuery ($) which Umbraco 17 does NOT ship.  │
// │ The #initUpload() method guards for !jQuery and silently no-ops if absent. Upload will not   │
// │ function until jQuery is provided or the upload widget is replaced.                           │
// └──────────────────────────────────────────────────────────────────────────────────────────┘

import Cropper from 'cropperjs';
import { LitElement, html, css, nothing, customElement, property, state } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import {
    UmbPropertyValueChangeEvent,
    type UmbPropertyEditorConfigCollection,
    type UmbPropertyEditorUiElement,
} from '@umbraco-cms/backoffice/property-editor';

const PLUGIN_PATH = '/App_Plugins/N3O.Umbraco.Cropper';
const MAX_CONTAINER_SIZE = 500;
const CONTAINER_RATIO = 5;

// --- types ---

interface CropData {
    x: number;
    y: number;
    width: number;
    height: number;
}

interface CropBoxData {
    left: number;
    top: number;
    width: number;
    height: number;
}

interface CropDefinition {
    label: string;
    alias: string;
    width: number;
    height: number;
}

interface CropperValue {
    src: string;
    mediaId: string;
    filename: string;
    width: number;
    height: number;
    altText?: string;
    crops: Array<CropData | null>;
    cropBoxes?: Array<CropBoxData | null>;
}

interface UploadResponse {
    urlPath: string;
    mediaId: string;
    filename: string;
    width: number;
    height: number;
}

// jQuery / Formstone types — jQuery is NOT shipped by Umbraco 17. We keep a minimal ambient
// interface so the TS compiler is satisfied; at runtime this may be undefined (upload no-ops).
interface JQueryUploadOptions {
    action: string;
    label: string;
    maxSize: number;
    maxQueue: number;
    postData: Record<string, number>;
}

interface JQueryUploadElement {
    upload(options: JQueryUploadOptions): JQueryUploadElement;
    on(event: string, handler: (...args: unknown[]) => void): JQueryUploadElement;
}

// Formstone registers $.fn.upload as a jQuery plugin. We declare just enough for our use.
interface JQueryStatic {
    (element: Element): JQueryUploadElement;
}

declare global {
    interface Window {
        jQuery?: JQueryStatic;
        $?: JQueryStatic;
    }
}

// --- script loader (for vendored formstone files only; cropperjs is now bundled via npm) ---

const scriptPromises: Record<string, Promise<void>> = {};

function loadScript(src: string): Promise<void> {
    if (src in scriptPromises) {
        return scriptPromises[src];
    }

    scriptPromises[src] = new Promise<void>((resolve, reject) => {
        const script = document.createElement('script');
        script.src = src;
        script.onload = () => resolve();
        script.onerror = () => reject(new Error('Failed to load ' + src));
        document.head.appendChild(script);
    });

    return scriptPromises[src];
}

// --- element ---

const elementName = 'n3o-cropper';

@customElement(elementName)
export class N3oCropperElement extends UmbElementMixin(LitElement) implements UmbPropertyEditorUiElement {
    #value: CropperValue | null = null;
    #config: UmbPropertyEditorConfigCollection | undefined;
    #cropperLoading = false;
    #minimumImageWidth = 0;
    #minimumImageHeight = 0;

    @property({ type: Object })
    get value(): CropperValue | null {
        return this.#value;
    }
    set value(v: CropperValue | null | undefined) {
        const oldValue = this.#value;
        this.#value = v ?? null;
        this.requestUpdate('value', oldValue);
    }

    // Configuration (prevalues) arrives as an UmbPropertyEditorConfigCollection.
    public set config(config: UmbPropertyEditorConfigCollection | undefined) {
        this.#config = config;

        this.#minimumImageWidth = 0;
        this.#minimumImageHeight = 0;

        for (const cropDefinition of this.#cropDefinitions) {
            if (cropDefinition.width) {
                this.#minimumImageWidth = Math.max(cropDefinition.width, this.#minimumImageWidth);
            }

            if (cropDefinition.height) {
                this.#minimumImageHeight = Math.max(cropDefinition.height, this.#minimumImageHeight);
            }
        }
    }

    @state() private _uploadInProgress = false;
    @state() private _uploadPercent = 0;
    @state() private _errorMessage: string | null = null;
    @state() private _cropIndex: number | null = null;
    @state() private _showCropSize = false;
    @state() private _currentCropWidth = 0;
    @state() private _currentCropHeight = 0;
    @state() private _requiredCropWidth = 0;
    @state() private _requiredCropHeight = 0;
    @state() private _mediaId = '';

    get #cropDefinitions(): CropDefinition[] {
        // Config value is stored as an array of plain objects; cast to our interface.
        return (this.#config?.getValueByAlias('cropDefinitions') as CropDefinition[] | null | undefined) ?? [];
    }

    get #altTextEnabled(): boolean {
        const altText = this.#config?.getValueByAlias('altText');
        return altText === true || altText === '1' || altText === 1;
    }

    #loadShadowCss(href: string): void {
        const link = document.createElement('link');
        link.rel = 'stylesheet';
        link.href = href;
        this.renderRoot.appendChild(link);
    }

    override async firstUpdated(): Promise<void> {
        // cropperjs / formstone render into this shadow root, so their CSS must live here too.
        this.#loadShadowCss(`${PLUGIN_PATH}/cropperjs/cropper.min.css`);
        this.#loadShadowCss(`${PLUGIN_PATH}/formstone/upload.css`);

        // cropperjs is now bundled via npm — no loadScript needed for it.
        // Formstone remains vendored (jQuery dependency is a pending product decision).
        await loadScript(`${PLUGIN_PATH}/formstone/core.js`);
        await loadScript(`${PLUGIN_PATH}/formstone/upload.js`);

        if (this.#value) {
            this.#restoreCrops();
        }

        this.#initUpload();
    }

    #restoreCrops(): void {
        const cropDefinitionsLength = this.#cropDefinitions.length;

        if (this.#value === null) {
            return;
        }

        if (this.#value.crops?.length === cropDefinitionsLength && this.#value.cropBoxes?.length !== undefined) {
            this.#selectCrop(0);
        } else if (this.#value.crops?.length === cropDefinitionsLength && this.#value.cropBoxes?.length === undefined) {
            this.#value.cropBoxes = new Array<CropBoxData | null>(cropDefinitionsLength);

            for (let i = 0; i < this.#value.crops.length; i++) {
                this.#value.cropBoxes[i] = null;
            }

            for (let i = 0; i < this.#value.crops.length; i++) {
                const crop = this.#value.crops[i];

                if (crop) {
                    const left = crop.x / CONTAINER_RATIO;
                    const top = crop.y / CONTAINER_RATIO;
                    const width = MAX_CONTAINER_SIZE - (crop.width / CONTAINER_RATIO) / MAX_CONTAINER_SIZE;
                    const height = MAX_CONTAINER_SIZE - (crop.height / CONTAINER_RATIO) / MAX_CONTAINER_SIZE;

                    this.#value.cropBoxes[i] = { left, top, width, height };
                }
            }

            this.#selectCrop(0);
        } else {
            const isCropBoxNotSaved =
                this.#value.crops?.length === cropDefinitionsLength && this.#value.cropBoxes?.length === undefined;

            this.#value.crops = new Array<CropData | null>(cropDefinitionsLength);
            this.#value.cropBoxes = new Array<CropBoxData | null>(cropDefinitionsLength);

            for (let i = 0; i < this.#value.crops.length; i++) {
                this.#value.crops[i] = null;
                this.#value.cropBoxes[i] = null;
            }

            isCropBoxNotSaved ? this.#createCropTool(this.#value.crops.length - 1, false, false) : this.#selectCrop(0);
        }
    }

    #notify(): void {
        this.dispatchEvent(new UmbPropertyValueChangeEvent());
    }

    #createCropTool(cropIndex: number, hidden: boolean, restoreCropData: boolean): void {
        const cropDefinition = this.#cropDefinitions[cropIndex];

        this.#cropperLoading = true;
        this._cropIndex = null;

        const cropToolWrapper = this.renderRoot.querySelector('.crop-tool-wrapper');

        if (!cropToolWrapper) {
            return;
        }

        cropToolWrapper.querySelectorAll('.crop-tool').forEach((el) => el.remove());

        const cropTool = document.createElement('div');
        cropTool.className = 'crop-tool ' + (hidden ? 'hidden' : '');
        cropToolWrapper.prepend(cropTool);

        const img = document.createElement('img');
        img.src = this.#value?.src ?? '';
        cropTool.appendChild(img);

        // Cropper callback context uses `this` to access the cropper instance; we capture the
        // element reference in `self` to avoid conflicting with the callback's own `this`.
        // eslint-disable-next-line @typescript-eslint/no-this-alias
        const self = this;

        // The Cropper instance is held via the DOM img element; we don't need to keep a class ref.
        new Cropper(img, {
            aspectRatio: Number((cropDefinition.width / cropDefinition.height).toFixed(3)),
            autoCrop: true,
            guides: false,
            highlight: true,
            // dragCrop was renamed to dragMode in cropperjs v1 types; resizable → cropBoxResizable.
            dragMode: Cropper.DragMode.Crop,
            movable: true,
            cropBoxResizable: true,
            zoomable: false,
            viewMode: 2,
            minContainerWidth: MAX_CONTAINER_SIZE,
            minContainerHeight: MAX_CONTAINER_SIZE,
            crop: function (this: { cropper: Cropper }) {
                if (self.#cropperLoading && restoreCropData) {
                    return;
                }

                if (!self.#value) {
                    return;
                }

                self.#value.crops[cropIndex] = this.cropper.getData(true) as CropData;
                self.#value.cropBoxes![cropIndex] = this.cropper.getCropBoxData() as CropBoxData;

                const definition = self.#cropDefinitions[cropIndex];

                self._requiredCropWidth = definition.width;
                self._requiredCropHeight = definition.height;

                self._currentCropHeight = self.#value.crops[cropIndex]?.height ?? 0;
                self._currentCropWidth = self.#value.crops[cropIndex]?.width ?? 0;

                self._showCropSize =
                    self._currentCropHeight < self._requiredCropHeight ||
                    self._currentCropWidth < self._requiredCropWidth;

                if (!self.#cropperLoading) {
                    self.#notify();
                }
            },
            ready: function (this: { cropper: Cropper }) {
                self.#cropperLoading = false;
                self._cropIndex = cropIndex;

                if (restoreCropData && self.#value?.crops[cropIndex]) {
                    this.cropper.setCropBoxData(self.#value.cropBoxes?.[cropIndex] ?? {});
                }

                if (cropIndex - 1 >= 0 && self.#value?.crops[cropIndex - 1] === null) {
                    self.#createCropTool(cropIndex - 1, cropIndex - 1 !== 0, false);
                }
            },
        });
    }

    #selectCrop(cropIndex: number): void {
        this.#createCropTool(cropIndex, false, true);
    }

    #getCropButtonClass(cropIndex: number): string {
        return 'button cursor ' + (this._cropIndex !== null && this._cropIndex === cropIndex ? 'selected' : 'not-selected');
    }

    #getCropLabel(cropIndex: number): string {
        return this.#cropDefinitions[cropIndex].label;
    }

    #processResponse(errorMessage: string | null, json?: unknown): void {
        if (errorMessage === null) {
            let response = json as UploadResponse;

            if (typeof response === 'string') {
                response = JSON.parse(response) as UploadResponse;
            }

            const value: CropperValue = {
                src: response.urlPath,
                mediaId: response.mediaId,
                filename: response.filename,
                width: response.width,
                height: response.height,
                crops: new Array<CropData | null>(this.#cropDefinitions.length),
            };

            for (let i = 0; i < value.crops.length; i++) {
                value.crops[i] = null;
            }

            this.#value = value;
            this.requestUpdate();
            this.#notify();

            void this.updateComplete.then(() => {
                this.#createCropTool(this.#value!.crops.length - 1, false, false);
            });
        } else {
            this._errorMessage = errorMessage;
        }

        this._uploadInProgress = false;
    }

    async #loadMediaById(): Promise<void> {
        if (!this._mediaId || this._mediaId.length !== 17) {
            return;
        }

        try {
            const response = await fetch(`/umbraco/backoffice/api/cropper/media/${this._mediaId}`);

            if (response.ok) {
                const json: unknown = await response.json();
                this.#processResponse(null, json);
            } else {
                this.#processResponse('No media found with the specified ID');
            }
        } catch {
            this.#processResponse('No media found with the specified ID');
        }
    }

    // ⚠ FORMSTONE/JQUERY FLAG: Formstone upload requires global jQuery which Umbraco 17 does not
    // provide. This method guards for !jQuery and silently exits — upload will no-op at runtime
    // until jQuery is injected or the upload widget is replaced. Pending product decision.
    #initUpload(): void {
        const jQuery = window.jQuery ?? window.$;
        const uploadEl = this.renderRoot.querySelector('.upload');

        if (!jQuery || !uploadEl) {
            return;
        }

        const self = this;

        jQuery(uploadEl)
            .upload({
                action: '/umbraco/backoffice/api/cropper/upload',
                label:
                    'Drop an image, or click to select. Min. size ' +
                    this.#minimumImageWidth +
                    ' x ' +
                    this.#minimumImageHeight +
                    '.',
                maxSize: 104857600,
                maxQueue: 1,
                postData: {
                    minWidth: this.#minimumImageWidth,
                    minHeight: this.#minimumImageHeight,
                },
            })
            .on('filestart.upload', function () {
                self._uploadPercent = 0;
                self._uploadInProgress = true;
            })
            .on('fileprogress.upload', function (_e: unknown, _file: unknown, percent: unknown) {
                self._uploadPercent = percent as number;
            })
            .on('filecomplete.upload', function (_e: unknown, _file: unknown, response: unknown) {
                self.#processResponse(null, response);
            })
            .on('fileerror.upload', function () {
                self.#processResponse(
                    'The specified file is either not a valid image, exceeds the maximum allowed image size, or does not meet dimension constraints'
                );
            });
    }

    #copyToClipboard(text: string): void {
        navigator.clipboard?.writeText(text);
    }

    #startOver(showConfirmPrompt: boolean): void {
        if (showConfirmPrompt && !confirm('Are you sure?')) {
            return;
        }

        const cropToolWrapper = this.renderRoot.querySelector('.crop-tool-wrapper');
        cropToolWrapper?.querySelectorAll('.crop-tool').forEach((el) => el.remove());

        this.#value = null;
        this._errorMessage = null;
        this._cropIndex = null;
        this.requestUpdate();
        this.#notify();

        void this.updateComplete.then(() => this.#initUpload());
    }

    #onAltTextInput(e: Event): void {
        if (this.#value) {
            this.#value.altText = (e.target as HTMLInputElement).value;
            this.#notify();
        }
    }

    #onMediaIdInput(e: Event): void {
        this._mediaId = (e.target as HTMLInputElement).value;
        void this.#loadMediaById();
    }

    override render() {
        return html`
            <div class="n3o-umbraco-cropper">
                ${this.#value ? this.#renderEditor() : this.#renderUpload()}
            </div>
        `;
    }

    #renderUpload() {
        return html`
            ${this._uploadInProgress
                ? html`<div class="radial-progress" data-progress=${this._uploadPercent}>
                      <div class="circle">
                          <div class="mask full"><div class="fill"></div></div>
                          <div class="mask half"><div class="fill"></div><div class="fill fix"></div></div>
                          <div class="shadow"></div>
                      </div>
                      <div class="inset"><div class="percentage">${this._uploadPercent}%</div></div>
                  </div>`
                : nothing}

            ${!this._errorMessage && !this._uploadInProgress
                ? html`<div class="upload"></div>
                      <input
                          class="textBox media-id"
                          type="text"
                          placeholder="Load media by ID"
                          .value=${this._mediaId}
                          @input=${this.#onMediaIdInput}
                          @paste=${this.#onMediaIdInput} />`
                : nothing}

            ${this._errorMessage
                ? html`<p class="error">
                          Uploading of the file failed with the error:<br /><br />
                          ${this._errorMessage}
                      </p>
                      <p class="start-over">
                          <a @click=${() => this.#startOver(false)} class="cursor reset">Try again</a>
                      </p>`
                : nothing}
        `;
    }

    #renderEditor() {
        return html`
            <div class="crop-tool-wrapper"></div>

            <ul class="crops">
                ${(this.#value!.crops ?? []).map(
                    (_crop, index) => html`<li>
                        <a @click=${() => this.#selectCrop(index)} class=${this.#getCropButtonClass(index)}>
                            ${this.#getCropLabel(index)} </a
                        >&nbsp;&nbsp;
                    </li>`
                )}
            </ul>

            ${this.#altTextEnabled
                ? html`<p>
                      <input
                          class="textBox"
                          type="text"
                          placeholder="Alt text"
                          .value=${this.#value!.altText ?? ''}
                          required
                          @input=${this.#onAltTextInput} />
                  </p>`
                : nothing}

            ${this._showCropSize
                ? html`<p style="text-align: center;">
                      <span style="color: #ff0000; font-weight: bold;"
                          >${this._currentCropWidth} x ${this._currentCropHeight}</span
                      >
                      is less than the required
                      <span style="font-weight: bold;">${this._requiredCropWidth} x ${this._requiredCropHeight}</span>
                  </p>`
                : nothing}

            <div class="start-over">
                <div style="float: left;">
                    <a @click=${() => this.#copyToClipboard(this.#value!.mediaId)}>${this.#value!.mediaId}</a>
                    |
                    <a href=${this.#value!.src} target="_blank">Download</a>
                </div>
                <div style="float: right;">
                    <a @click=${() => this.#startOver(true)} class="reset cursor">Delete image</a>
                </div>
            </div>
        `;
    }

    static override styles = css`
        :host {
            display: block;
        }

        .n3o-umbraco-cropper {
            max-width: 500px;
        }

        .hidden {
            display: none;
        }

        .upload {
            padding: 10px;
            border: 1px dashed #666;
            border-radius: 5px;
            text-align: center;
        }

        .error {
            background: red;
            color: white;
            padding: 10px;
        }

        .textBox {
            margin-top: 5px;
            margin-bottom: 5px;
            height: 30px;
            width: 100%;
            font-size: 15px;
            font-family: Verdana;
            line-height: 30px;
            display: inline-block;
            vertical-align: middle;
        }

        .cursor {
            cursor: pointer;
            text-decoration: none;
        }

        .reset {
            color: red;
            font-size: 120%;
            font-weight: bold;
            text-decoration: none;
            text-align: right;
        }

        .crops {
            width: 100%;
            overflow-x: auto;
            overflow-y: hidden;
            padding: 0;
            margin: 0;
        }

        .crops li {
            display: inline-block;
            margin: 0.3em 0.3em 0.3em 0;
            vertical-align: middle;
            padding: 0;
        }

        .crops li img {
            height: 100%;
            width: auto;
        }

        .crops li .button {
            text-decoration: none;
            font: menu;
            display: inline-block;
            padding: 2px 8px;
            background: ButtonFace;
            color: ButtonText;
            border-style: solid;
            border-width: 2px;
            border-color: ButtonHighlight ButtonShadow ButtonShadow ButtonHighlight;
        }

        .crops li .button:active,
        .crops li .selected {
            border-color: ButtonShadow ButtonHighlight ButtonHighlight ButtonShadow;
        }

        .radial-progress {
            margin: 10px;
            width: 120px;
            height: 120px;
            background-color: #d6dadc;
            border-radius: 50%;
            position: relative;
        }

        .radial-progress .inset {
            width: 90px;
            height: 90px;
            position: absolute;
            margin-left: 15px;
            margin-top: 15px;
            background-color: #fbfbfb;
            border-radius: 50%;
            box-shadow: 6px 6px 10px rgba(0, 0, 0, 0.2);
        }

        .radial-progress .inset .percentage {
            position: absolute;
            top: 35px;
            width: 100%;
            text-align: center;
            font-weight: 800;
            font-size: 22px;
            color: #97a71d;
        }

        .start-over {
            overflow: hidden;
            margin-top: 10px;
        }
    `;
}

export default N3oCropperElement;

declare global {
    interface HTMLElementTagNameMap {
        [elementName]: N3oCropperElement;
    }
}
