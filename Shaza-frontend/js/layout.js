async function loadLayout() {
    try {
        // 1. استدعاء السايدبار
        const sidebarRes = await fetch('/components/sidebar.html');
        const sidebarHtml = await sidebarRes.text();
        const sidebarPlace = document.getElementById('sidebar-placeholder');
        if (sidebarPlace) {
            sidebarPlace.innerHTML = sidebarHtml;
            // بعد ما السايدبار يحمل.. لونه!
            if (typeof highlightActiveLink === "function") highlightActiveLink();
        }

        // 2. استدعاء الهيدر (Search & Profile)
        const headerRes = await fetch('/components/header.html');
        const headerHtml = await headerRes.text();
        const headerPlace = document.getElementById('header-placeholder');
        if (headerPlace) {
            headerPlace.innerHTML = headerHtml;
            // بعد ما الهيدر يحمل.. شغل البحث!
            if (typeof initSearch === "function") initSearch();
        }

    } catch (error) {
        console.error("مشكلة في تحميل الملفات:", error);
    }
}

// تشغيل الكل بمجرد فتح الصفحة
document.addEventListener("DOMContentLoaded", loadLayout);