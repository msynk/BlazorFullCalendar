window.calendarInterop = {
    scrollToHour: function (elementId, hour, pixelsPerHour) {
        const el = document.getElementById(elementId);
        if (!el) return;
        const pxPerHour = pixelsPerHour ?? 96;
        const top = hour * pxPerHour;
        if (typeof el.scrollTo === "function") {
            el.scrollTo({ top: top, behavior: "auto" });
        } else {
            el.scrollTop = top;
        }
    },

    initResize: function (dotNetRef, elementId, direction) {
        const el = document.getElementById(elementId);
        if (!el) return;

        const pixelsPerHour = 96;
        const minPerPixel = 60 / pixelsPerHour;

        let startY = 0;
        let startHeight = 0;

        const onPointerMove = (e) => {
            const deltaY = e.clientY - startY;
            const deltaMinutes = Math.round(deltaY * minPerPixel);
            dotNetRef.invokeMethodAsync('OnResizeMove', direction, deltaMinutes);
        };

        const onPointerUp = () => {
            document.removeEventListener('pointermove', onPointerMove);
            document.removeEventListener('pointerup', onPointerUp);
            dotNetRef.invokeMethodAsync('OnResizeEnd');
        };

        el.addEventListener('pointerdown', (e) => {
            e.preventDefault();
            startY = e.clientY;
            startHeight = el.parentElement?.offsetHeight || 0;
            document.addEventListener('pointermove', onPointerMove);
            document.addEventListener('pointerup', onPointerUp);
            dotNetRef.invokeMethodAsync('OnResizeStart');
        });
    },

    isDarkMode: function () {
        return window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches;
    },

    isMobile: function () {
        return window.innerWidth <= 768;
    },

    getLocalStorage: function (key) {
        return localStorage.getItem(key);
    },

    setLocalStorage: function (key, value) {
        localStorage.setItem(key, value);
    }
};
