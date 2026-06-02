// ┌──────────────────────────────────────────────────────────────────────────────────────────┐
// │ PENDING DECISION (Umbraco 17 migration): this editor was ported 1:1 from AngularJS and      │
// │ still relies on cropperjs + Formstone (which needs a global jQuery the v17 backoffice no     │
// │ longer ships). It MAY OR MAY NOT be re-migrated to use Umbraco's native media/image picker   │
// │ + built-in Image Cropper instead of the bundled libs + custom upload widget. Not decided     │
// │ yet — do NOT rewrite this until Talha confirms the direction. Leaving the faithful port in    │
// │ place for now. See BELLISSIMA_MIGRATION_LOG.md / MIGRATION_PLAN.md (BLOCKER-07).              │
// └──────────────────────────────────────────────────────────────────────────────────────────┘
import { LitElement, html, css, nothing } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import { UmbPropertyValueChangeEvent } from '@umbraco-cms/backoffice/property-editor';

const PLUGIN_PATH = '/App_Plugins/N3O.Umbraco.Cropper';
const MAX_CONTAINER_SIZE = 500;
const CONTAINER_RATIO = 5;

// Loads a classic (non-module) script once and resolves when ready. cropperjs exposes the global
// `Cropper`; formstone is a jQuery plugin that registers `$.fn.upload` and depends on global jQuery.
const scriptPromises = {};
function loadScript(src) {
    if (scriptPromises[src]) {
        return scriptPromises[src];
    }

    scriptPromises[src] = new Promise((resolve, reject) => {
        const script = document.createElement('script');
        script.src = src;
        script.onload = () => resolve();
        script.onerror = () => reject(new Error('Failed to load ' + src));
        document.head.appendChild(script);
    });

    return scriptPromises[src];
}

class N3oCropperElement extends UmbElementMixin(LitElement) {
    static properties = {
        value: { type: Object },
        _uploadInProgress: { state: true },
        _uploadPercent: { state: true },
        _errorMessage: { state: true },
        _cropIndex: { state: true },
        _showCropSize: { state: true },
        _currentCropWidth: { state: true },
        _currentCropHeight: { state: true },
        _requiredCropWidth: { state: true },
        _requiredCropHeight: { state: true },
        _mediaId: { state: true },
    };

    #value;
    #config;
    #cropper;
    #cropperLoading = false;
    #minimumImageWidth = 0;
    #minimumImageHeight = 0;
    #ready = false;

    constructor() {
        super();

        this._uploadInProgress = false;
        this._uploadPercent = 0;
        this._errorMessage = null;
        this._cropIndex = null;
        this._showCropSize = false;
        this._currentCropWidth = 0;
        this._currentCropHeight = 0;
        this._requiredCropWidth = 0;
        this._requiredCropHeight = 0;
        this._mediaId = '';
    }

    get value() {
        return this.#value;
    }

    set value(v) {
        const oldValue = this.#value;
        this.#value = v;
        this.requestUpdate('value', oldValue);
    }

    // Configuration (prevalues) arrives as an UmbPropertyEditorConfigCollection.
    set config(config) {
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

    get #cropDefinitions() {
        return this.#config?.getValueByAlias('cropDefinitions') ?? [];
    }

    get #altTextEnabled() {
        const altText = this.#config?.getValueByAlias('altText');

        return altText === true || altText === '1' || altText === 1;
    }

    #loadShadowCss(href) {
        const link = document.createElement('link');
        link.rel = 'stylesheet';
        link.href = href;
        this.renderRoot.appendChild(link);
    }

    async firstUpdated() {
        // cropperjs / formstone render into this shadow root, so their CSS must live here too.
        this.#loadShadowCss(`${PLUGIN_PATH}/cropperjs/cropper.min.css`);
        this.#loadShadowCss(`${PLUGIN_PATH}/formstone/upload.css`);

        await loadScript(`${PLUGIN_PATH}/cropperjs/cropper.min.js`);
        await loadScript(`${PLUGIN_PATH}/formstone/core.js`);
        await loadScript(`${PLUGIN_PATH}/formstone/upload.js`);

        this.#ready = true;

        if (this.#value) {
            this.#restoreCrops();
        }

        this.#initUpload();
    }

    #restoreCrops() {
        const cropDefinitionsLength = this.#cropDefinitions.length;

        if (this.#value.crops?.length === cropDefinitionsLength && this.#value.cropBoxes?.length !== undefined) {
            this.#selectCrop(0);
        } else if (this.#value.crops?.length === cropDefinitionsLength && this.#value.cropBoxes?.length === undefined) {
            this.#value.cropBoxes = new Array(cropDefinitionsLength);

            for (let i = 0; i < this.#value.crops.length; i++) {
                this.#value.cropBoxes[i] = null;
            }

            for (let i = 0; i < this.#value.crops.length; i++) {
                const left = this.#value.crops[i].x / CONTAINER_RATIO;
                const top = this.#value.crops[i].y / CONTAINER_RATIO;
                const width = MAX_CONTAINER_SIZE - (this.#value.crops[i].width / CONTAINER_RATIO) / MAX_CONTAINER_SIZE;
                const height = MAX_CONTAINER_SIZE - (this.#value.crops[i].height / CONTAINER_RATIO) / MAX_CONTAINER_SIZE;

                this.#value.cropBoxes[i] = { left, top, width, height };
            }

            this.#selectCrop(0);
        } else {
            const isCropBoxNotSaved =
                this.#value.crops?.length === cropDefinitionsLength && this.#value.cropBoxes?.length === undefined;

            this.#value.crops = new Array(cropDefinitionsLength);
            this.#value.cropBoxes = new Array(cropDefinitionsLength);

            for (let i = 0; i < this.#value.crops.length; i++) {
                this.#value.crops[i] = null;
                this.#value.cropBoxes[i] = null;
            }

            isCropBoxNotSaved ? this.#createCropTool(this.#value.crops.length - 1, false, false) : this.#selectCrop(0);
        }
    }

    #notify() {
        this.dispatchEvent(new UmbPropertyValueChangeEvent());
    }

    #createCropTool(cropIndex, hidden, restoreCropData) {
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
        img.src = this.#value.src;
        cropTool.appendChild(img);

        const self = this;

        this.#cropper = new Cropper(img, {
            aspectRatio: Number((cropDefinition.width / cropDefinition.height).toFixed(3)),
            autoCrop: true,
            strict: true,
            guides: false,
            highlight: true,
            dragCrop: true,
            movable: true,
            resizable: true,
            zoomable: false,
            viewMode: 2,
            minContainerWidth: MAX_CONTAINER_SIZE,
            minContainerHeight: MAX_CONTAINER_SIZE,
            crop: function () {
                if (self.#cropperLoading && restoreCropData) {
                    return;
                }

                self.#value.crops[cropIndex] = this.cropper.getData(true);
                self.#value.cropBoxes[cropIndex] = this.cropper.getCropBoxData();

                const definition = self.#cropDefinitions[cropIndex];

                self._requiredCropWidth = definition.width;
                self._requiredCropHeight = definition.height;

                self._currentCropHeight = self.#value.crops[cropIndex].height;
                self._currentCropWidth = self.#value.crops[cropIndex].width;

                self._showCropSize =
                    self._currentCropHeight < self._requiredCropHeight ||
                    self._currentCropWidth < self._requiredCropWidth;

                if (!self.#cropperLoading) {
                    self.#notify();
                }
            },
            ready: function () {
                self.#cropperLoading = false;

                self._cropIndex = cropIndex;

                if (restoreCropData && self.#value.crops[cropIndex]) {
                    this.cropper.setCropBoxData(self.#value.cropBoxes[cropIndex]);
                }

                if (cropIndex - 1 >= 0 && self.#value.crops[cropIndex - 1] === null) {
                    self.#createCropTool(cropIndex - 1, cropIndex - 1 !== 0, false);
                }
            },
        });
    }

    #selectCrop(cropIndex) {
        this.#createCropTool(cropIndex, false, true);
    }

    #getCropButtonClass(cropIndex) {
        return 'button cursor ' + (this._cropIndex !== null && this._cropIndex === cropIndex ? 'selected' : 'not-selected');
    }

    #getCropLabel(cropIndex) {
        return this.#cropDefinitions[cropIndex].label;
    }

    #processResponse(errorMessage, json) {
        if (errorMessage === null) {
            let response = json;

            if (typeof response === 'string' || response instanceof String) {
                response = JSON.parse(response);
            }

            const value = {
                src: response.urlPath,
                mediaId: response.mediaId,
                filename: response.filename,
                width: response.width,
                height: response.height,
                crops: new Array(this.#cropDefinitions.length),
            };

            for (let i = 0; i < value.crops.length; i++) {
                value.crops[i] = null;
            }

            this.#value = value;
            this.requestUpdate();
            this.#notify();

            this.updateComplete.then(() => {
                this.#createCropTool(this.#value.crops.length - 1, false, false);
            });
        } else {
            this._errorMessage = errorMessage;
        }

        this._uploadInProgress = false;
    }

    async #loadMediaById() {
        if (!this._mediaId || this._mediaId.length !== 17) {
            return;
        }

        try {
            const response = await fetch(`/umbraco/backoffice/api/cropper/media/${this._mediaId}`);

            if (response.ok) {
                const json = await response.json();
                this.#processResponse(null, json);
            } else {
                this.#processResponse('No media found with the specified ID');
            }
        } catch {
            this.#processResponse('No media found with the specified ID');
        }
    }

    #initUpload() {
        const jQuery = window.jQuery || window.$;
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
            .on('fileprogress.upload', function (e, file, percent) {
                self._uploadPercent = percent;
            })
            .on('filecomplete.upload', function (e, file, response) {
                self.#processResponse(null, response);
            })
            .on('fileerror.upload', function () {
                self.#processResponse(
                    'The specified file is either not a valid image, exceeds the maximum allowed image size, or does not meet dimension constraints'
                );
            });
    }

    #copyToClipboard(text) {
        navigator.clipboard?.writeText(text);
    }

    #startOver(showConfirmPrompt) {
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

        this.updateComplete.then(() => this.#initUpload());
    }

    #onAltTextInput(e) {
        if (this.#value) {
            this.#value.altText = e.target.value;
            this.#notify();
        }
    }

    #onMediaIdInput(e) {
        this._mediaId = e.target.value;
        this.#loadMediaById();
    }

    render() {
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
                ${(this.#value.crops ?? []).map(
                    (crop, index) => html`<li>
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
                          .value=${this.#value.altText ?? ''}
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
                    <a @click=${() => this.#copyToClipboard(this.#value.mediaId)}>${this.#value.mediaId}</a>
                    |
                    <a href=${this.#value.src} target="_blank">Download</a>
                </div>
                <div style="float: right;">
                    <a @click=${() => this.#startOver(true)} class="reset cursor">Delete image</a>
                </div>
            </div>
        `;
    }

    static styles = css`
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

customElements.define('n3o-cropper', N3oCropperElement);

export default N3oCropperElement;
export { N3oCropperElement };
