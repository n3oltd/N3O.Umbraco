// ┌──────────────────────────────────────────────────────────────────────────────────────────┐
// │ PENDING DECISION (Umbraco 17 migration): this editor was ported 1:1 from AngularJS and      │
// │ still relies on the Formstone upload widget (which needs a global jQuery the v17 backoffice   │
// │ no longer ships — currently loaded on demand from a CDN). It MAY OR MAY NOT be re-migrated    │
// │ to use Umbraco's native media/image picker instead of the bundled jQuery+Formstone uploader.  │
// │ Not decided yet — do NOT rewrite this until Talha confirms the direction. Leaving the         │
// │ faithful port in place for now. See BELLISSIMA_MIGRATION_LOG.md / MIGRATION_PLAN.md.          │
// └──────────────────────────────────────────────────────────────────────────────────────────┘
import { LitElement, html, css, nothing } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import { UmbPropertyValueChangeEvent } from '@umbraco-cms/backoffice/property-editor';

const elementName = 'n3o-uploader';

const PLUGIN_PATH = '/App_Plugins/N3O.Umbraco.Uploader';

// jQuery is a hard dependency of Formstone (core.js / upload.js are UMD modules that fall back to the
// global jQuery). The Bellissima backoffice no longer ships jQuery globally, so we load it (and then the
// Formstone scripts) once, on demand, as classic scripts attached to the document.
let scriptsPromise;

function loadScript(src) {
    return new Promise((resolve, reject) => {
        const existing = document.querySelector(`script[data-n3o-uploader="${src}"]`);

        if (existing) {
            resolve();

            return;
        }

        const script = document.createElement('script');
        script.src = src;
        script.async = false;
        script.dataset.n3oUploader = src;
        script.onload = () => resolve();
        script.onerror = () => reject(new Error(`Failed to load ${src}`));

        document.head.appendChild(script);
    });
}

async function loadFormstone() {
    if (!scriptsPromise) {
        scriptsPromise = (async () => {
            if (!window.jQuery) {
                await loadScript('https://code.jquery.com/jquery-3.7.1.min.js');
            }

            await loadScript(`${PLUGIN_PATH}/formstone/core.js`);
            await loadScript(`${PLUGIN_PATH}/formstone/upload.js`);
        })();
    }

    return scriptsPromise;
}

function makeId() {
    let text = '';
    const possible = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz';

    for (let i = 0; i < 10; i++) {
        text += possible.charAt(Math.floor(Math.random() * possible.length));
    }

    return text;
}

// Property editor UI for the N3O Uploader. Ports the AngularJS controller/view to Lit: drives the
// Formstone upload widget, shows radial upload progress, supports loading existing media by id, and
// persists { urlPath, mediaId, extension, sizeMb, filename, altText } as the property value.
class N3oUploaderElement extends UmbElementMixin(LitElement) {
    static properties = {
        value: { type: Object },
        config: { attribute: false },
        _uploadInProgress: { state: true },
        _errorMessage: { state: true },
        _progress: { state: true },
        _mediaId: { state: true },
    };

    #value;
    #config = {};
    #uniqueId = makeId();
    #initialised = false;

    constructor() {
        super();

        this._uploadInProgress = false;
        this._errorMessage = null;
        this._progress = 0;
        this._mediaId = '';
    }

    get value() {
        return this.#value;
    }

    set value(v) {
        const old = this.#value;
        this.#value = v;
        this.requestUpdate('value', old);
    }

    set config(c) {
        this.#config = c;
    }

    get config() {
        return this.#config;
    }

    #cfg(alias) {
        if (this.#config && typeof this.#config.getValueByAlias === 'function') {
            return this.#config.getValueByAlias(alias);
        }

        return this.#config ? this.#config[alias] : undefined;
    }

    get #imageMode() {
        // Mirrors AngularJS: imagesOnly defaults to true unless explicitly "0"/false.
        const imagesOnly = this.#cfg('imagesOnly');

        return !(imagesOnly === '0' || imagesOnly === 0 || imagesOnly === false);
    }

    #setValue(v) {
        this.value = v;
        this.dispatchEvent(new UmbPropertyValueChangeEvent());
    }

    #startOver(showConfirmPrompt) {
        if (showConfirmPrompt && !confirm('Are you sure?')) {
            return;
        }

        this._errorMessage = null;
        this.#setValue(null);
    }

    #processResponse(errorMessage, json) {
        if (errorMessage === null) {
            let response = json;

            if (typeof response === 'string' || response instanceof String) {
                response = JSON.parse(response);
            }

            this.#setValue({
                urlPath: response.urlPath,
                mediaId: response.mediaId,
                extension: response.extension,
                sizeMb: response.sizeMb,
                filename: response.filename,
            });
        } else {
            this._errorMessage = errorMessage;
        }

        this._uploadInProgress = false;
    }

    #onMediaIdInput(event) {
        this._mediaId = event.target.value;
        this.#loadMediaById();
    }

    #loadMediaById() {
        if (!this._mediaId || this._mediaId.length !== 17) {
            return;
        }

        fetch(`/umbraco/backoffice/api/uploader/media/${this._mediaId}`)
            .then((response) => {
                if (!response.ok) {
                    throw new Error('not found');
                }

                return response.json();
            })
            .then((json) => this.#processResponse(null, json))
            .catch(() => this.#processResponse('No media found with the specified ID'));
    }

    #onAltTextInput(event) {
        this.#setValue({ ...this.#value, altText: event.target.value });
    }

    #copyToClipboard(text) {
        const temp = document.createElement('input');
        document.body.appendChild(temp);
        temp.value = text;
        temp.select();
        document.execCommand('copy');
        temp.remove();
    }

    updated() {
        // Initialise the Formstone upload widget once, after the upload target has rendered.
        if (this.#initialised || this.#value) {
            return;
        }

        const uploadEl = this.renderRoot.querySelector('.upload');

        if (!uploadEl) {
            return;
        }

        this.#initialised = true;

        loadFormstone()
            .then(() => {
                const $ = window.jQuery;

                $(uploadEl)
                    .upload({
                        action: '/umbraco/backoffice/api/uploader/upload',
                        label: 'Drop and drop a file, or click to select',
                        maxSize: 5368709120,
                        maxQueue: 1,
                        postData: {
                            allowedExtensions: this.#cfg('allowedExtensions'),
                            maxFileSizeMb: this.#cfg('maxFileSizeMb'),
                            imagesOnly: this.#imageMode,
                            minImageWidth: this.#cfg('minImageWidth'),
                            maxImageWidth: this.#cfg('maxImageWidth'),
                            minImageHeight: this.#cfg('minImageHeight'),
                            maxImageHeight: this.#cfg('maxImageHeight'),
                        },
                    })
                    .on('filestart.upload', () => {
                        this._progress = 0;
                        this._uploadInProgress = true;
                    })
                    .on('fileprogress.upload', (e, file, percent) => {
                        this._progress = percent;
                    })
                    .on('filecomplete.upload', (e, file, response) => {
                        this.#processResponse(null, response);
                    })
                    .on('fileerror.upload', () => {
                        this.#processResponse(
                            'The specified file either has an invalid extensions, exceeds the maximum allowed size, or does not meet dimension constraints'
                        );
                    });
            })
            .catch(() => {
                this._errorMessage = 'Failed to load the uploader';
            });
    }

    #renderProgress() {
        if (!this._uploadInProgress) {
            return nothing;
        }

        const numbers = [];
        numbers.push(html`<span>-</span>`);

        for (let i = 0; i <= 100; i++) {
            numbers.push(html`<span>${i}%</span>`);
        }

        return html`
            <div class="radial-progress" data-progress=${this._progress}>
                <div class="circle">
                    <div class="mask full"><div class="fill"></div></div>
                    <div class="mask half">
                        <div class="fill"></div>
                        <div class="fill fix"></div>
                    </div>
                    <div class="shadow"></div>
                </div>
                <div class="inset">
                    <div class="percentage">
                        <div class="numbers">${numbers}</div>
                    </div>
                </div>
            </div>
        `;
    }

    #renderUpload() {
        return html`
            ${this.#renderProgress()}
            ${!this._errorMessage && !this._uploadInProgress
                ? html`
                      <div class="upload"></div>

                      <p>
                          <br />
                          Allowed file types : ${this.#cfg('allowedExtensions')} <br />
                          Maximum file size : ${this.#cfg('maxFileSizeMb')}MB
                      </p>

                      <input
                          class="textBox media-id"
                          type="text"
                          placeholder="Load media by ID"
                          .value=${this._mediaId}
                          @input=${this.#onMediaIdInput}
                          @paste=${this.#onMediaIdInput} />
                  `
                : nothing}
            ${this._errorMessage
                ? html`
                      <p class="error">
                          Uploading of the file failed with the error:<br /><br />
                          ${this._errorMessage}
                      </p>

                      <p class="start-over">
                          <a @click=${() => this.#startOver(false)} class="cursor reset">Try Again</a>
                      </p>
                  `
                : nothing}
        `;
    }

    #renderValue() {
        const value = this.#value;

        return html`
            <a href=${value.urlPath} target="_blank">${value.filename} (${value.sizeMb}MB)</a>

            ${this.#imageMode
                ? html`
                      <br />
                      <img src=${value.urlPath} style="max-width: 280px; margin: 10px; background-color: #eeeeee;" />
                      <br /><br />
                  `
                : nothing}
            ${this.#imageMode && this.#cfg('altTextRequired')
                ? html`
                      <p>
                          <input
                              class="textBox"
                              type="text"
                              placeholder="Alt text"
                              .value=${value.altText ?? ''}
                              @input=${this.#onAltTextInput} />
                      </p>
                  `
                : nothing}

            <div class="start-over">
                <div style="float: left;">
                    <a class="cursor" @click=${() => this.#copyToClipboard(value.mediaId)}>${value.mediaId}</a>
                    |
                    <a href=${value.urlPath} target="_blank">Download</a>
                </div>

                <div style="float: right;">
                    <a @click=${() => this.#startOver(true)} class="reset cursor">Delete file</a>
                </div>
            </div>
        `;
    }

    render() {
        return html`
            <link rel="stylesheet" href="${PLUGIN_PATH}/radial-progress.css" />
            <link rel="stylesheet" href="${PLUGIN_PATH}/formstone/upload.css" />
            <div class="n3o-uploader">
                <div id=${this.#uniqueId}>
                    ${!this.#value ? this.#renderUpload() : nothing}
                    ${this.#value && !this._uploadInProgress ? this.#renderValue() : nothing}
                </div>
            </div>
        `;
    }

    static styles = css`
        .n3o-uploader {
            max-width: 500px;
        }

        .n3o-uploader .hidden {
            display: none;
        }

        .n3o-uploader .upload {
            padding: 10px;
            border: 1px dashed #666;
            border-radius: 5px;
            text-align: center;
        }

        .n3o-uploader .error {
            background: red;
            color: white;
            padding: 10px;
        }

        .n3o-uploader .textBox {
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

        .n3o-uploader .cursor {
            cursor: pointer;
            text-decoration: none;
        }

        .n3o-uploader .reset {
            color: red;
            font-size: 120%;
            font-weight: bold;
            text-decoration: none;
            text-align: right;
        }
    `;
}

customElements.define(elementName, N3oUploaderElement);

export default N3oUploaderElement;
export { N3oUploaderElement };
