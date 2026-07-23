let bar = null;
let ticking = false;

export function registerProgress() {
    bar = document.querySelector('.progress-bar');
    if (!bar) return;
    window.addEventListener('scroll', onScroll, { passive: true });
}

export function cleanupProgress() {
    window.removeEventListener('scroll', onScroll);
    bar = null;
}

function onScroll() {
    if (!ticking) {
        window.requestAnimationFrame(() => {
            if (!bar) return;
            var h = document.documentElement.scrollHeight - window.innerHeight;
            var pct = h > 0 ? (window.scrollY / h * 100) : 0;
            bar.style.width = pct + '%';
            bar.classList.toggle('visible', pct > 1);
            ticking = false;
        });
        ticking = true;
    }
}
