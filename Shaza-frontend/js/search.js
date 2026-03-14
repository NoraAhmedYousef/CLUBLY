function initSearch() {
    const searchInput = document.querySelector(".search-container input");
    if (!searchInput) return;

    searchInput.addEventListener("input", (e) => {
        const term = e.target.value.toLowerCase();
        // بيبحث عن كروت التنبيهات (notif-card) أو كروت الحجوزات (card)
        const cards = document.querySelectorAll(".notif-card, .card");

        cards.forEach(card => {
            const text = card.innerText.toLowerCase();
            card.style.display = text.includes(term) ? "block" : "none";
        });
    });
}