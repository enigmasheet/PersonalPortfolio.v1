let dotNetRef = null;
let observer = null;

export function observeLazySections(ref) {
    dotNetRef = ref;
    var sections = document.querySelectorAll('.lazy-section');
    if (!sections.length) return;

    observer = new IntersectionObserver(function (entries) {
        entries.forEach(function (entry) {
            if (entry.isIntersecting) {
                var id = entry.target.id;
                dotNetRef.invokeMethodAsync('ShowSection', id);
                observer.unobserve(entry.target);
            }
        });
    }, { rootMargin: '200px' });

    sections.forEach(function (el) {
        observer.observe(el);
    });
}

export function cleanup() {
    if (observer) {
        observer.disconnect();
        observer = null;
    }
    dotNetRef = null;
}
