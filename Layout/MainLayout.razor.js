export function registerScroll() {
    window.addEventListener('scroll', onScroll, { passive: true });
}

export function cleanupScroll() {
    window.removeEventListener('scroll', onScroll);
}

export function setBodyScroll(allow) {
    document.body.style.overflow = allow ? '' : 'hidden';
}

function onScroll() {
    var y = window.scrollY;
    var appbar = document.querySelector('.mud-appbar-custom');
    var toTop = document.querySelector('.mud-back-to-top');
    if (appbar) appbar.classList.toggle('mud-scrolled', y > 10);
    if (toTop) toTop.classList.toggle('visible', y > 300);
}
