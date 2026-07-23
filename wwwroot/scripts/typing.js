(function () {
    if (globalThis.__typingInit) return;
    globalThis.__typingInit = true;

    function initTyping(el) {
        const phrases = (el.getAttribute('data-phrases') || '').split('|').filter(Boolean);
        if (!phrases.length) return;
        const speed = parseInt(el.getAttribute('data-speed')) || 80;
        const deleteSpeed = parseInt(el.getAttribute('data-delete-speed')) || 40;
        const pause = parseInt(el.getAttribute('data-pause')) || 2000;
        const delay = parseInt(el.getAttribute('data-delay')) || 0;
        let phraseIndex = 0;
        let charIndex = 0;
        let isDeleting = false;
        const cursor = document.createElement('span');
        cursor.className = 'typing-cursor';
        cursor.textContent = '|';
        el.after(cursor);

        function type() {
            const current = phrases[phraseIndex];
            if (isDeleting) {
                el.textContent = current.substring(0, charIndex--);
                if (charIndex < 0) {
                    isDeleting = false;
                    phraseIndex = (phraseIndex + 1) % phrases.length;
                    setTimeout(type, 500);
                    return;
                }
                setTimeout(type, deleteSpeed);
            } else {
                el.textContent = current.substring(0, charIndex++);
                if (charIndex > current.length) {
                    isDeleting = true;
                    setTimeout(type, pause);
                    return;
                }
                setTimeout(type, speed);
            }
        }

        setTimeout(type, delay);
    }

    const el = document.querySelector('[data-typing]');
    if (el) {
        if (document.readyState === 'loading') {
            document.addEventListener('DOMContentLoaded', () => { initTyping(el); });
        } else {
            initTyping(el);
        }
    }
})();
