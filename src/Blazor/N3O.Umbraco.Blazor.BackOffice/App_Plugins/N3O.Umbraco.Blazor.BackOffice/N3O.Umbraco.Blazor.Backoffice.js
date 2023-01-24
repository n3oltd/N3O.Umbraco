var blazorJsFile = '/_framework/blazor.server.js';

function blazorIsLoaded(){
    let scripts = $('script').filter(function () {
        return ($(this).attr('src') === blazorJsFile);
    });

    return scripts.length !== 0;
}

if (blazorIsLoaded()) {
    let scriptElement = document.createElement('script');
    scriptElement.src = blazorJsFile;
    scriptElement.onload = startBlazor;

    scriptElement.setAttribute("autostart", "false");

    document.head.appendChild(blazorScript);
}

async function startBlazor() {
    Blazor.start({
        configureSignalR: function (builder) {
            builder.withUrl("/_blazor");
        }
    });
}
