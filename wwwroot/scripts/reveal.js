(function () {
    if (globalThis.__revealInit) return;
    globalThis.__revealInit = true;

    function initReveal() {
        let els = document.querySelectorAll('[data-reveal], [data-reveal-stagger]');
        if (!els.length) return;

        let observer = new IntersectionObserver(function (entries) {
            entries.forEach(function (entry) {
                if (entry.isIntersecting) {
                    entry.target.classList.add('revealed');
                    observer.unobserve(entry.target);
                }
            });
        }, { threshold: 0.1, rootMargin: '0px 0px -40px 0px' });

        els.forEach(function (el) {
            let staggerParent = el.closest('[data-reveal-stagger]');
            if (staggerParent) {
                let stagger = Number.parseInt(staggerParent.dataset.revealStagger) || 100;
                let children = staggerParent.querySelectorAll('[data-reveal]');
                children.forEach(function (child, i) {
                    child.style.transitionDelay = (i * stagger) + 'ms';
                    observer.observe(child);
                });
            } else {
                observer.observe(el);
            }
        });
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', initReveal);
    } else {
        initReveal();
    }
})();
