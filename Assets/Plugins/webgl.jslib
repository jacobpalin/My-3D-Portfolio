mergeInto(LibraryManager.library, {
    IsMobile: function () {
        var isMobile = /Android|iPhone|iPad|iPod|Opera Mini|IEMobile|WPDesktop|Mobile/i.test(navigator.userAgent);
        return isMobile ? 1 : 0; // Return 1 for true (mobile) and 0 for false (desktop)
    }
});