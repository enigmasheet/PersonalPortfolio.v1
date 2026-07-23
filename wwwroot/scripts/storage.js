globalThis.getLocalStorage = function (key) {
    try { return localStorage.getItem(key); } catch { return null; }
};

globalThis.setLocalStorage = function (key, value) {
    try { localStorage.setItem(key, value); } catch { /* localStorage may be blocked */ }
};
