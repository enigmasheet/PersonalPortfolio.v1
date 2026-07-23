(function () {
    const path = location.pathname + location.search + location.hash;
    if (path !== '/404.html' && path !== '/') {
        sessionStorage.setItem('redirectPath', path);
    }
})();
