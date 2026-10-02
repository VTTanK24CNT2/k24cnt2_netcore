// custom.js - Lập trình tương tác JavaScript cho Custom Layout (Session 07)
document.addEventListener("DOMContentLoaded", function () {
    console.log("custom.js đã được tải thành công từ wwwroot/js/custom.js!");

    // 1. Xử lý đổi màu khi click vào menu link (như trong slide 21)
    const navLinks = document.querySelectorAll(".custom-nav-link");
    navLinks.forEach(function (link) {
        link.addEventListener("click", function () {
            navLinks.forEach(l => l.classList.remove("active"));
            this.classList.add("active");
        });
    });

    // 2. Xử lý nút Demo tương tác JS
    const btnDemo = document.getElementById("btnJsDemo");
    if (btnDemo) {
        btnDemo.addEventListener("click", function () {
            const timeStr = new Date().toLocaleTimeString();
            alert("Thông báo từ custom.js: Bạn đã kích hoạt sự kiện click lúc " + timeStr);
        });
    }

    // 3. Xử lý nút Thêm vào giỏ hàng
    const addToCartBtns = document.querySelectorAll(".btn-add-to-cart");
    addToCartBtns.forEach(function (btn) {
        btn.addEventListener("click", function (e) {
            e.preventDefault();
            const productName = this.getAttribute("data-product-name") || "Sản phẩm";
            showToastMessage("Đã thêm thành công: " + productName + " vào giỏ hàng!");
        });
    });

    // Hàm hiển thị thông báo góc màn hình (Toast)
    function showToastMessage(msg) {
        let toast = document.getElementById("customToast");
        if (!toast) {
            toast = document.createElement("div");
            toast.id = "customToast";
            toast.style.position = "fixed";
            toast.style.bottom = "20px";
            toast.style.right = "20px";
            toast.style.backgroundColor = "#10b981";
            toast.style.color = "#ffffff";
            toast.style.padding = "12px 20px";
            toast.style.borderRadius = "8px";
            toast.style.boxShadow = "0 4px 12px rgba(0,0,0,0.15)";
            toast.style.zIndex = "9999";
            toast.style.transition = "opacity 0.3s ease";
            document.body.appendChild(toast);
        }
        toast.textContent = msg;
        toast.style.opacity = "1";
        toast.style.display = "block";

        setTimeout(function () {
            toast.style.opacity = "0";
            setTimeout(function () {
                toast.style.display = "none";
            }, 300);
        }, 2500);
    }
});
