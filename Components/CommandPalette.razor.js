let dotNetRef = null;

export function register(ref) {
    dotNetRef = ref;
    document.addEventListener('keydown', onKeyDown);
}

export function cleanup() {
    document.removeEventListener('keydown', onKeyDown);
    dotNetRef = null;
}

function onKeyDown(e) {
    if ((e.metaKey || e.ctrlKey) && e.key === 'k') {
        e.preventDefault();
        if (dotNetRef) {
            dotNetRef.invokeMethodAsync('Toggle');
        }
    }
}

export function scrollItemIntoView(selector) {
    var el = document.querySelector(selector);
    if (el) el.scrollIntoView({ block: 'nearest' });
}
