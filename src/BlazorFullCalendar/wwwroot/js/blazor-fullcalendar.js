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

    /**
     * Pointer resize for event blocks. Matches the idea of the reference calendar
     * (re-resizable client-side updates): coalesce pointer moves to animation frames,
     * capture the pointer, and await resize-start before tracking moves so Blazor state is ready.
     */
    initResize: function (dotNetRef, elementId, direction) {
        const el = document.getElementById(elementId);
        if (!el) return;

        const pixelsPerHour = 96;
        const minPerPixel = 60 / pixelsPerHour;

        el.addEventListener("pointerdown", async (e) => {
            if (e.button !== 0) return;
            e.preventDefault();
            e.stopPropagation();

            const startY = e.clientY;
            let latestY = startY;
            let rafId = null;
            let activePointerId = e.pointerId;
            let ended = false;

            try {
                el.setPointerCapture(e.pointerId);
            } catch { /* older browsers */ }

            await dotNetRef.invokeMethodAsync("OnResizeStart", direction);

            const flushMove = () => {
                rafId = null;
                const deltaMinutes = Math.round((latestY - startY) * minPerPixel);
                return dotNetRef.invokeMethodAsync("OnResizeMove", direction, deltaMinutes);
            };

            const onPointerMove = (ev) => {
                latestY = ev.clientY;
                if (rafId == null) {
                    rafId = requestAnimationFrame(() => {
                        void flushMove();
                    });
                }
            };

            const endResize = async () => {
                if (ended) return;
                ended = true;
                document.removeEventListener("pointermove", onPointerMove);
                document.removeEventListener("pointerup", endResize);
                document.removeEventListener("pointercancel", endResize);

                if (rafId != null) {
                    cancelAnimationFrame(rafId);
                    rafId = null;
                }
                const deltaMinutes = Math.round((latestY - startY) * minPerPixel);
                await dotNetRef.invokeMethodAsync("OnResizeMove", direction, deltaMinutes);

                try {
                    if (activePointerId != null && typeof el.releasePointerCapture === "function")
                        el.releasePointerCapture(activePointerId);
                } catch { }

                activePointerId = null;
                await dotNetRef.invokeMethodAsync("OnResizeEnd");
            };

            document.addEventListener("pointermove", onPointerMove);
            document.addEventListener("pointerup", endResize);
            document.addEventListener("pointercancel", endResize);
        });
    },

    isDarkMode: function () {
        return window.matchMedia && window.matchMedia("(prefers-color-scheme: dark)").matches;
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
