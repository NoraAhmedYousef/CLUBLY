const loginForm = document.querySelector('form');

loginForm.addEventListener('submit', function(e) {
    e.preventDefault(); // منع التحميل الافتراضي للتجربة

    const email = document.querySelector('input[type="email"]').value;
    const password = document.querySelector('input[type="password"]').value;

    console.log("Login Attempt:", { email, password });

    // هنا يمكنك إضافة كود الربط مع الـ API مستقبلاً
    alert("Logging in with: " + email);
});

// وظيفة زر جوجل (تجريبية)
const googleBtn = document.querySelector('.google-btn');
googleBtn.addEventListener('click', function() {
    alert("Redirecting to Google Login...");
});