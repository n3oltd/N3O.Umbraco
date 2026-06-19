/*
 * NOTE: React shell is overhead here (this is a non-UI JS boot loader, not a custom element) —
 * kept as the existing loader per migration decision. This bundle registers no element and renders
 * no UI; it only runs side effects at import time (injects the blazor.server.js <script> tag and
 * starts the SignalR circuit). There is nothing for React to render, so it is intentionally NOT
 * wrapped in a React root — doing so would add a runtime dependency with zero UI to mount. Left as
 * a minimal, faithful TypeScript port of the original loader.
 *
 * jQuery was removed in the v17 RCL conversion: Umbraco 17 (Bellissima) does not ship a global `$`,
 * so the "is the blazor script already injected" check now uses native DOM (querySelectorAll).
 */

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
    return Array.from(document.querySelectorAll('script')).some(
        (s) => s.getAttribute('src') === blazorJsFile
    );
}

if (!blazorIsLoaded()) {
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
