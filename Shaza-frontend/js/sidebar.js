function highlightActiveLink() {
    const currentPath = window.location.pathname;
    const links = document.querySelectorAll(".side-link");

    links.forEach(link => {
        const href = link.getAttribute("href");
        if (currentPath.includes(href) && href !== "/") {
            link.classList.add("active");
        } else {
            link.classList.remove("active");
        }
    });
}