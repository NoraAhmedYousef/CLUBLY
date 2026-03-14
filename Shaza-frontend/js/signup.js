const signupForm = document.querySelector('form');
const passwordInput = document.getElementById('password');

passwordInput.addEventListener('input', function() {
    const strengthText = document.getElementById('pass-strength');
    if (!strengthText) return; // لضمان عدم حدوث خطأ إذا لم يوجد العنصر

    const val = passwordInput.value;
    if (val.length === 0) { strengthText.textContent = ""; return; }

    const hasUpper = /[A-Z]/.test(val);
    const hasNumber = /[0-9]/.test(val);

    if (val.length < 8) {
        strengthText.textContent = "Weak (Too short) ❌";
        strengthText.style.color = "red";
    } else if (!hasUpper || !hasNumber) {
        strengthText.textContent = "Medium (Add Uppercase & Number) ⚠️";
        strengthText.style.color = "orange";
    } else {
        strengthText.textContent = "Strong Password ✅";
        strengthText.style.color = "green";
    }
});

signupForm.addEventListener('submit', function(e) {
    // جلب قيم الحقول
    const fullName = document.querySelector('input[placeholder="Full name *"]').value.trim();
    const nationalId = document.querySelector('input[placeholder="National ID *"]').value.trim();
    const email = document.querySelector('input[type="email"]').value.trim();
    const phone = document.querySelector('input[type="tel"]').value.trim();
    const password = document.getElementById('password').value;
    const confirmPass = document.getElementById('confirmPass').value;
    const termsChecked = document.querySelector('.terms input[type="checkbox"]').checked;

    // 1. التأكد من أن الحقول ليست فارغة (تمت إضافتها بواسطة required في HTML ولكن لزيادة الأمان)
    if (!fullName || !nationalId || !email || !phone || !password || !confirmPass) {
        alert("برجاء ملء جميع الحقول المطلوبة (*)");
        e.preventDefault();
        return;
    }

    // 2. التحقق من صحة الإيميل (Email Validation)
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(email)) {
        alert("برجاء إدخال بريد إلكتروني صحيح (مثال: name@example.com)");
        e.preventDefault();
        return;
    }

    // 3. التحقق من رقم التليفون المصري
    // الشروط: يبدأ بـ 01 ثم (0 أو 1 أو 2 أو 5) ثم 8 أرقام (إجمالي 11 رقم)
    const egyptPhoneRegex = /^01[0125][0-9]{8}$/;
    if (!egyptPhoneRegex.test(phone)) {
        alert("رقم الهاتف غير صحيح. يجب أن يكون رقم مصري مكون من 11 رقم ويبدأ بـ 010 أو 011 أو 012 أو 015");
        e.preventDefault();
        return;
    }

    // 4. التحقق من الرقم القومي (14 رقم)
    if (nationalId.length !== 14 || isNaN(nationalId)) {
        alert("الرقم القومي يجب أن يتكون من 14 رقم");
        e.preventDefault();
        return;
    }

    // 5. التحقق من تطابق كلمة المرور
    if (password !== confirmPass) {
        alert("كلمة المرور غير متطابقة!");
        e.preventDefault();
        return;
    }

    // 6. التحقق من طول كلمة المرور (أمان إضافي)
    if (password.length < 8) {
        alert("يجب أن تكون كلمة المرور 8 أحرف على الأقل");
        e.preventDefault();
        return;
    }

    // 7. التأكد من الموافقة على الشروط
    if (!termsChecked) {
        alert("يجب الموافقة على الشروط والأحكام للمتابعة");
        e.preventDefault();
        return;
    }

    // إذا وصل الكود هنا، يعني أن كل البيانات سليمة
    alert("تم تسجيل البيانات بنجاح!");
});