// admin.js - Xử lý tương tác khu vực Quản trị Admin
document.addEventListener("DOMContentLoaded", function () {
    const sidebarToggle = document.getElementById("sidebarToggle");
    const sidebar = document.querySelector(".admin-sidebar");

    if (sidebarToggle && sidebar) {
        sidebarToggle.addEventListener("click", function () {
            sidebar.classList.toggle("open");
        });
    }

    // Đánh dấu active cho menu admin dựa trên URL hiện tại
    const currentPath = window.location.pathname.toLowerCase();
    const adminLinks = document.querySelectorAll(".admin-nav-link");
    adminLinks.forEach(function (link) {
        const href = link.getAttribute("href")?.toLowerCase();
        if (href && currentPath === href) {
            adminLinks.forEach(l => l.classList.remove("active"));
            link.classList.add("active");
        }
    });
});
