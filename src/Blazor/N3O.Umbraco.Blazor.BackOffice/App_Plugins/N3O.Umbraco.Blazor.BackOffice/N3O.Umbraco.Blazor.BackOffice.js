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
            builder.withAutomaticReconnect([0, 2000, 10000, 15000, 20000, 30000, 60000]);

            const connection = builder.build();
            connection.serverTimeoutInMilliseconds = 30_000;
        }
    });
}
