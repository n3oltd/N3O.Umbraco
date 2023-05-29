var blazorJsFile = '/_framework/blazor.server.js';

function blazorIsLoaded(){
    let scripts = $('script').filter(function () {
        return ($(this).attr('src') === blazorJsFile);
    });

    return scripts.length !== 1;
}

if (blazorIsLoaded()) {
    let scriptElement = document.createElement('script');
    scriptElement.src = blazorJsFile;
    scriptElement.onload = startBlazor;

    scriptElement.setAttribute("autostart", "false");

    document.body.appendChild(scriptElement);
}

async function startBlazor() {
    Blazor.start({
        configureSignalR: function (builder) {
            builder.withUrl("/_blazor");
            builder.serverTimeoutInMilliseconds = 120_000;
        }
    });
}
