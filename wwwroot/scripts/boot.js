(function () {
    const loader = document.getElementById('loader');
    const app = document.getElementById('app');
    const errorUi = document.getElementById('blazor-error-ui');

    function showError(msg) {
        if (errorUi) {
            const text = errorUi.childNodes[0];
            if (text) text.textContent = 'Error: ' + (msg || 'unknown');
            errorUi.style.display = 'block';
        }
        if (loader) loader.classList.add('hidden');
        if (app) app.classList.add('visible');
        console.error('[boot]', msg);
    }

    globalThis.addEventListener('error', function (e) {
        showError(e.error ? e.error.message : e.message);
    });
    globalThis.addEventListener('unhandledrejection', function (e) {
        showError(e.reason ? e.reason.message : String(e.reason));
    });

    function boot() {
        if (loader) loader.classList.add('hidden');
        if (app) app.classList.add('visible');
    }

    Blazor.start({
        logLevel: 1,
        configureRuntime() { },
        loadBootResource(type, name, defaultUri, integrity, behavior) {
            if (type === 'dotnetjs' && name === 'dotnet.js') {
                const link = document.querySelector('link[href*="dotnet."][href$=".js"]');
                if (link) return link.getAttribute('href');
            }
        }
    }).then(boot).catch(function (err) {
        showError(err ? err.message : String(err));
        boot();
    });

    globalThis.__destroyLoader = boot;
})();
