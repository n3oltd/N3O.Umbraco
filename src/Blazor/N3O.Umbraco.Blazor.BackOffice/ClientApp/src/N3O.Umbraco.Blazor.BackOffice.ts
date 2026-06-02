/*
 * DEPENDENCY FLAG — Global jQuery ($)
 * ------------------------------------
 * This loader uses the global `$` (jQuery) to check whether the Blazor script tag has
 * already been injected.  Umbraco v17 (Bellissima) does NOT ship jQuery as part of the
 * backoffice — the global is only present because the consuming website loads it
 * separately (e.g. via a traditional front-end bundle or a legacy plugin that
 * vendored it).
 *
 * Action required (product decision pending):
 *   - If the consuming site is guaranteed to load jQuery before this bundle runs, the
 *     current behaviour is safe and no change is needed.
 *   - If that guarantee cannot be made, replace the jQuery selector with a native
 *     DOM equivalent, e.g.:
 *
 *       function blazorIsLoaded(): boolean {
 *           return Array.from(document.querySelectorAll('script'))
 *               .filter(s => s.getAttribute('src') === blazorJsFile)
 *               .length !== 1;
 *       }
 *
 *   Do NOT make that change without an explicit product decision — this file is
 *   intentionally kept as a minimal, faithful TypeScript port of the original JS.
 */

// Minimal ambient declaration for the global jQuery `$` function.
// Only the subset actually used by this loader is declared.
// jQuery is expected to be loaded by the consuming site before this script runs.
interface JQuerySet {
    filter(fn: (this: HTMLElement) => boolean): JQuerySet;
    attr(name: string): string | undefined;
    length: number;
}

declare function $(selector: string): JQuerySet;
declare function $(element: HTMLElement): JQuerySet;

// Ambient declarations for the global Blazor object injected by blazor.server.js.
interface SignalRBuilder {
    withUrl(url: string): SignalRBuilder;
    withAutomaticReconnect(reconnectDelays: number[]): SignalRBuilder;
    build(): SignalRConnection;
}

interface SignalRConnection {
    serverTimeoutInMilliseconds: number;
}

interface BlazorStartOptions {
    configureSignalR(builder: SignalRBuilder): void;
}

declare const Blazor: {
    start(options: BlazorStartOptions): void;
};

const blazorJsFile = '/_framework/blazor.server.js';

function blazorIsLoaded(): boolean {
    const scripts = $('script').filter(function (this: HTMLElement) {
        return $(this).attr('src') === blazorJsFile;
    });

    return scripts.length !== 1;
}

if (blazorIsLoaded()) {
    const scriptElement = document.createElement('script');
    scriptElement.src = blazorJsFile;
    scriptElement.onload = startBlazor;

    scriptElement.setAttribute('autostart', 'false');

    document.body.appendChild(scriptElement);
}

async function startBlazor(): Promise<void> {
    Blazor.start({
        configureSignalR: function (builder: SignalRBuilder): void {
            builder.withUrl('/_blazor');
            builder.withAutomaticReconnect([0, 2000, 10000, 15000, 20000, 30000, 60000]);

            const connection = builder.build();
            connection.serverTimeoutInMilliseconds = 30_000;
        }
    });
}
