const newPassForm = document.getElementById('newPassForm');
const passwordInput = document.getElementById('password');
const strengthText = document.getElementById('pass-strength');

// 1. مراقبة قوة كلمة المرور أثناء الكتابة
passwordInput.addEventListener('input', function() {
    const val = passwordInput.value;
    let message = "";
    let color = "";

    if (val.length === 0) {
        message = "";
    } else if (val.length < 8) {
        message = "Weak (Minimum 8 chars) ❌";
        color = "red";
    } else if (!/[A-Z]/.test(val) || !/[0-9]/.test(val)) {
        message = "Medium (Add Uppercase & Number) ⚠️";
        color = "orange";
    } else {
        message = "Strong Password ✅";
        color = "green";
    }

    strengthText.textContent = message;
    strengthText.style.color = color;
});

// 2. التحقق النهائي عند الإرسال
newPassForm.addEventListener('submit', function(e) {
    const password = passwordInput.value;
    const confirmPass = document.getElementById('confirmPass').value;

    // التأكد من القوة
    const hasUpper = /[A-Z]/.test(password);
    const hasNumber = /[0-9]/.test(password);

    if (password.length < 8 || !hasUpper || !hasNumber) {
        alert("Password is too weak! Please follow the requirements.");
        e.preventDefault();
        return;
    }

    // التأكد من التطابق
    if (password !== confirmPass) {
        alert("Passwords do not match!");
        e.preventDefault();
        return;
    }

    alert("Success! Your password has been updated.");
    // window.location.href = "login.html"; // توجيه للمسجل بعد النجاح
});