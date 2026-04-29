// ============================================================
// Clubly i18n — Language Toggle (English / Arabic)
// Usage: include this script in every HTML page, then call
//   applyLang()  on DOMContentLoaded (already done below)
// ============================================================

window.CLUBLY_TRANSLATIONS = {
    en: {
    // ── Nav ───────────────────────────────────────────────
    nav_home: "Home",
    nav_activities: "Activities",
    nav_facilities: "Facilities",
    nav_trainers: "Trainers",
    nav_plans: "Membership Plans",
    nav_about: "About Us",
    nav_faq: "FAQ",
    nav_contact: "Contact",
    nav_signin: "Sign In",
    nav_join: "Join Now",
    nav_signout: "Sign Out",
    nav_pricing: "Pricing",

    // ── Hero / Index ──────────────────────────────────────
    hero_tag: "Egypt's #1 Sports Club Platform",
    hero_title1: "Your Fitness Journey",
    hero_title2: "Starts",
    hero_title3: "Right Here",
    hero_sub: "Join thousands of members transforming their lives with world-class trainers, state-of-the-art facilities, and AI-powered personalized programs.",
    hero_cta1: "Start Your Journey",
    hero_cta2: "Explore Activities",
    hero_stat1: "Active Members",
    hero_stat2: "Expert Trainers",
    hero_stat3: "Activity Classes",
    hero_stat4: "Member Rating",

modal_welcome: "Welcome back 👋",
modal_select_role: "Select your role and enter your credentials",
modal_email: "Email Address",
modal_password: "Password",
modal_forgot: "Forgot password?",
modal_no_account: "Don't have an account?",
modal_register_guest: "Register as Guest",
modal_create_account: "Create an Account",
modal_guest_reg: "Guest Registration · Your request will be reviewed",
modal_first_name: "First Name",
modal_last_name: "Last Name",
modal_national_id: "National ID",
modal_dob: "Date of Birth",
modal_gender: "Gender",
modal_have_account: "Already have an account?",
modal_forgot_title: "Forgot Password?",
modal_forgot_sub: "Enter your email and we'll send you a reset code",
modal_remember: "Remember your password?",
modal_check_email: "Check Your Email",
modal_new_password: "Set New Password",
modal_new_password_sub: "Choose a strong password for your account",
modal_nid_placeholder: "14-digit Egyptian National ID",
divider_or: "or",
modal_from_nid: "From National ID",
auth_create: "Create Account",
modal_min_pass: "Min. 8 chars, A-Z, 0-9, symbol",
modal_ph_first: "John",
modal_ph_last: "Doe",

    // ── Features strip ───────────────────────────────────
    feat_ai_title: "AI-Powered Plans",
    feat_ai_desc: "Smart workout programs tailored to your goals",
    feat_book_title: "Easy Booking",
    feat_book_desc: "Book any class in seconds, anytime",
    feat_train_title: "Expert Trainers",
    feat_train_desc: "Certified professionals to guide every step",
    feat_track_title: "Track Progress",
    feat_track_desc: "Real-time analytics on your fitness journey",
    feat_fac_title: "World-Class Facilities",
    feat_fac_desc: "Premium gyms, pools, courts and more",

    // ── Why Clubly section ────────────────────────────────
    why_label: "Why Clubly",
    why_title1: "More Than Just a",
    why_title2: "Gym Membership",
    why_sub: "We believe fitness is a lifestyle. Clubly gives you everything you need to build healthy habits that last.",
    why_personal_title: "Personalized Experience",
    why_personal_desc: "Every plan is tailored to your body type, goals, and schedule.",
    why_community_title: "Vibrant Community",
    why_community_desc: "Join group challenges, events, and connect with fellow members.",
    why_safe_title: "Safe & Certified",
    why_safe_desc: "All trainers are certified, and facilities meet international safety standards.",
    why_cta: "View Membership Plans",
    why_mind_title: "Mind & Body",
why_mind_desc: "Yoga, Meditation, Pilates",
why_strength_title: "Strength & Power",
why_strength_desc: "CrossFit, Weights, HIIT",
why_aqua_title: "Aqua Fitness",
why_aqua_desc: "Swimming, Aqua Aerobics",
why_dance_title: "Dance & Fun",
why_dance_desc: "Zumba, Dance Fit",

    // ── Schedule ─────────────────────────────────────────
    sched_label: "This Week",
    sched_title: "Featured Classes Schedule",
    sched_sub: "Get a taste of what's happening at Clubly this week.",
    sched_col_class: "Class",
    sched_col_trainer: "Trainer",
    sched_col_day: "Day",
    sched_col_time: "Time",
    sched_col_spots: "Spots",
    sched_left: "left",
sched_full: "Full",
    sched_col_location: "Location",
    sched_full: "Full",
    sched_left: "left",

    // ── CTA banner ───────────────────────────────────────
    cta_title: "Ready to Transform Your Life?",
    cta_sub: "Join 5,000+ members who've already transformed their lives. Your journey starts with a single step.",
    cta_btn2: "Book a Tour",

    // ── About Page ───────────────────────────────────────
    about_label: "OUR STORY",
    about_brand: "CLUBLY",
    about_community: "Community",
    about_founded: "Founded in 2018, Clubly was built on one belief: fitness should be accessible, inspiring, and fun for everyone — from first-timers to elite athletes.",
    about_mission_title: "Our Mission",
    about_mission: "To empower every individual to reach their peak physical and mental fitness through world-class coaching, cutting-edge facilities, and a supportive community that celebrates every milestone.",
    about_vision_title: "Our Vision",
    about_vision: "To become Egypt's most loved sports community — where fitness technology, human connection, and expert guidance come together to transform lives one session at a time.",
    about_values_title: "Our Values",
    about_values: "Community first. Integrity always. Innovation in everything. We believe in inclusive fitness, honest coaching, and continuously pushing the boundaries of what a sports club can be.",
    about_stat1: "Happy Members",
    about_stat2: "Expert Trainers",
    about_stat3: "Activities Offered",
    about_stat4: "Years of Excellence",
    about_stat5: "Average Rating",
    about_journey_label: "Our Journey",
    about_journey_sub: "From a small vision to Egypt's premier fitness destination.",
    about_2018_label: "2018 — The Beginning",
    about_2018_title: "Clubly Opens Its Doors",
    about_2018_desc: "Started with a single branch in Maadi, Cairo, 12 trainers, and a dream to change how Egyptians experience fitness. Our first 200 members became our founding family.",
    about_2020_label: "2020 — Growth",
    about_2020_title: "Digital Transformation",
    about_2020_desc: "Launched the Clubly digital platform, enabling online bookings, virtual classes, and AI-powered fitness recommendations — keeping our community active through challenging times.",
    about_2022_label: "2022 — Expansion",
    about_2022_title: "3 New Branches + Olympic Pool",
    about_2022_desc: "Opened 3 new branches across Cairo and Giza, introduced our Olympic-standard swimming pool, and crossed the milestone of 3,000 active members.",
    about_2025_label: "2025 — Today",
    about_2025_title: "Egypt's #1 Sports Club Platform",
    about_2025_desc: "Over 5,000 members, 80+ certified trainers, 30+ activities — and we're just getting started. Our AI-powered member experience sets a new standard for sports clubs across the region.",
    about_leadership_title: "Meet the Leadership",
    about_leadership_sub: "The passionate team behind Clubly's vision.",
    about_why_title: "Why Choose Clubly?",
    about_why_sub: "What makes us different from every other gym.",
    about_ai_title: "AI-Powered Programs",
    about_ai_desc: "Personalized training plans that adapt to your progress, goals, and schedule in real time.",
    about_trainers_title: "Expert Trainers",
    about_trainers_desc: "All our trainers are certified, background-checked, and regularly rated by members to ensure top quality.",
    about_fac_title: "World-Class Facilities",
    about_fac_desc: "Olympic pool, cutting-edge gym equipment, multiple studios — all maintained to the highest standards.",
    about_family_title: "Family-Friendly",
    about_family_desc: "Special programs for kids, teenagers, and seniors — fitness for the whole family.",
    about_book_title: "Easy Booking",
    about_book_desc: "Book any class, trainer, or facility in seconds via our app or website — no waiting, no hassle.",
    about_safety_title: "Safety First",
    about_safety_desc: "Every trainer is CPR-certified. Our facilities meet international safety standards with 24/7 monitoring.",
    about_cta_title: "Ready to Be Part of Our Story?",
    about_cta_sub: "Join 5,000+ members who've already transformed their lives. Your journey starts with a single step.",
    about_cta_btn1: "Join Now",
    about_cta_btn2: "Talk to Us",

    // ── Activities Page ───────────────────────────────────
    act_title: "Sports Activities",
    act_sub: "Discover a variety of exciting activities designed for fun, fitness, and unforgettable moments.",
    act_filter_all: "All Activities",
    act_filter_cat: "All Categories",
    act_filter_exp: "Any Experience",
    act_filter_price: "Any Price",
    act_filter_cap: "Any Capacity",
    act_exp_junior: "Junior (1–3 yrs)",
    act_exp_mid: "Mid (4–7 yrs)",
    act_exp_senior: "Senior (8+ yrs)",
    act_price_budget: "Budget (≤100 EGP)",
    act_price_mid: "Mid (101–300 EGP)",
    act_price_premium: "Premium (300+ EGP)",
    act_cap_small: "Small (<50)",
    act_cap_medium: "Medium (50–100)",
    act_cap_large: "Large (>100)",
    act_clear: "Clear",
    act_mind: "Mind & Body",
    act_strength: "Strength & Power",
    act_aqua: "Aqua Fitness",
    act_dance: "Dance & Fun",
    act_yoga: "Yoga, Meditation, Pilates",
    act_crossfit: "CrossFit, Weights, HIIT",
    act_swim: "Swimming, Aqua Aerobics",
    act_zumba: "Zumba, Dance Fit",
act_search: "Search for an activity…",
act_book_now: "Book Now",
act_signin_to_book: "Sign in to book an",

    // ── Facilities Page ───────────────────────────────────
    fac_title: "Our Facilities",
    fac_sub: "Discover our world-class facilities designed to enhance your experience and meet all your recreational needs.",
    fac_filter_all: "All Facilities",
    fac_gym: "Gym Floor",
    fac_pool: "Olympic Pool",
    fac_studio_a: "Studio A",
    fac_studio_b: "Studio B",
    fac_main: "Main Hall",
    fac_hours: "Working Hours",
    fac_daily: "Daily 6:00 AM – 10:00 PM",
fac_search: "Search by name or description…",

    // ── Trainers Page ─────────────────────────────────────
    tr_title: "Our Trainers",
    tr_sub: "Meet our professional trainers who help you reach your goals.",
    tr_filter_all: "All Categories",
    tr_personal: "Personal Training",
    tr_certified: "All our trainers are certified, background-checked, and regularly rated by members to ensure top quality.",
    tr_book_signin: "Sign in to Book a Class",

    // ── Plans Page ───────────────────────────────────────
    plans_title: "Choose Your Plan",
    plans_sub: "Flexible memberships designed to fit your lifestyle and budget. Cancel anytime.",
    plans_cta: "Join 2,400+ members already on their fitness journey.",

    // ── Contact Page ─────────────────────────────────────
    contact_title: "Get In Touch",
    contact_sub: "Have questions? Our team is here to help, 7 days a week.",
    contact_name_first: "First Name",
    contact_name_last: "Last Name",
    contact_email: "Email Address",
    contact_phone: "Phone Number",
    contact_topic: "Topic",
    contact_topic_general: "General Inquiry",
    contact_topic_support: "Technical Support",
    contact_topic_other: "Other",
    contact_msg: "Send Us a Message",
    contact_send: "Send Message",
    contact_success: "Message sent! We'll get back to you within 24 hours.",
    contact_address: "123 Fitness St, Maadi, Cairo",
    contact_love: "We'd Love to Hear From You",
    contact_team: "Have questions? Our team is here to help, 7 days a week.",
    contact_location: "Location",
    contact_email_label: "Email",
    contact_phone_label: "Phone",

    // ── FAQ Page ─────────────────────────────────────────
    faq_title: "Frequently Asked Questions",
    faq_sub: "Everything you need to know about Clubly before joining.",
    faq_still: "Still have questions?",
    faq_contact: "Contact Us",

    // ── Auth / Sign In ───────────────────────────────────
    auth_welcome: "Welcome back 👋",
    auth_select_role: "Select your role and enter your credentials",
    auth_email: "Email",
    auth_password: "Password",
    auth_signin: "Sign In",
    auth_forgot: "Forgot password?",
    auth_no_account: "Don't have an account?",
    auth_create: "Create Account",
    auth_create_title: "Create an Account",
    auth_strong_pass: "Choose a strong password for your account",
    auth_first: "First Name",
    auth_last: "Last Name",
    auth_dob: "Date of Birth",
    auth_gender: "Gender",
    auth_national_id: "National ID",
    auth_phone: "Phone",
    auth_confirm_pass: "Confirm Password",
    auth_already: "Already have an account?",
    auth_forgot_title: "Forgot Password?",
    auth_forgot_sub: "Enter your email and we'll send you a reset code",
    auth_send_code: "Send Reset Code",
    auth_remember: "Remember your password?",
    auth_verify: "Verify Code",
    auth_check_email: "Check Your Email",
    auth_sent_code: "We sent a 6-digit code to",
    auth_didnt_receive: "Didn't receive it?",
    auth_resend: "Resend Code",
    auth_new_pass: "New Password",
    auth_set_pass: "Set New Password",
    auth_change_email: "← Change Email",
    auth_guest: "Register as Guest",
    auth_guest_label: "Guest",
    auth_guest_sub: "Guest Registration · Your request will be reviewed",
    auth_member: "Member",
    auth_trainer: "Trainer",
    auth_admin: "Admin",
    auth_weak: "Weak",
    auth_auto: "AUTO",
    auth_confirm_slot: "✔ Confirm Slot",

    // ── Dashboard ────────────────────────────────────────
    dash_community: "Community",
    dash_sports: "Sports Activities",
    dash_ai: "AI-Powered Plans",
    dash_pricing: "Pricing",

    // ── Footer ───────────────────────────────────────────
    footer_explore: "Explore",
    footer_help: "Help",
    footer_follow: "Follow Us",
    footer_rights: "© 2025 Clubly. All rights reserved.",
    footer_privacy: "Privacy Policy",
    footer_terms: "Terms of Service",
    footer_tagline: "Your family's fitness journey starts here.",
    footer_faq: "FAQ",
    footer_contact: "Contact Us",
    footer_join: "Join Now",
    footer_address: "123 Fitness Street, Maadi, Cairo, Egypt",
    footer_hours: "Daily 6AM – 10PM",
  },

  ar: {
    // ── Nav ───────────────────────────────────────────────
    nav_home: "الرئيسية",
    nav_activities: "الأنشطة",
    nav_facilities: "المرافق",
    nav_trainers: "المدربون",
    nav_plans: "خطط العضوية",
    nav_about: "من نحن",
    nav_faq: "الأسئلة الشائعة",
    nav_contact: "تواصل معنا",
    nav_signin: "تسجيل الدخول",
    nav_join: "انضم الآن",
    nav_signout: "تسجيل الخروج",
    nav_pricing: "الأسعار",

    // ── Hero / Index ──────────────────────────────────────
    hero_tag: "منصة النادي الرياضي #1 في مصر",
    hero_title1: "رحلتك نحو اللياقة",
    hero_title2: "تبدأ",
    hero_title3: "من هنا",
    hero_sub: "انضم إلى آلاف الأعضاء الذين يغيّرون حياتهم مع أفضل المدربين والمرافق المتطورة والبرامج الشخصية المدعومة بالذكاء الاصطناعي.",
    hero_cta1: "ابدأ رحلتك",
    hero_cta2: "استكشف الأنشطة",
    hero_stat1: "عضو نشط",
    hero_stat2: "مدرب متخصص",
    hero_stat3: "فصل رياضي",
    hero_stat4: "تقييم الأعضاء",


modal_welcome: "أهلاً بعودتك 👋",
modal_select_role: "اختر دورك وأدخل بياناتك",
modal_email: "البريد الإلكتروني",
modal_password: "كلمة المرور",
modal_forgot: "نسيت كلمة المرور؟",
modal_no_account: "ليس لديك حساب؟",
modal_register_guest: "سجل كضيف",
modal_create_account: "إنشاء حساب",
modal_guest_reg: "تسجيل ضيف · سيتم مراجعة طلبك",
modal_first_name: "الاسم الأول",
modal_last_name: "الاسم الأخير",
modal_national_id: "الرقم القومي",
modal_dob: "تاريخ الميلاد",
modal_gender: "الجنس",
modal_have_account: "لديك حساب بالفعل؟",
modal_forgot_title: "نسيت كلمة المرور؟",
modal_forgot_sub: "أدخل بريدك وسنرسل لك كود إعادة تعيين",
modal_remember: "تتذكر كلمة المرور؟",
modal_check_email: "تحقق من بريدك",
modal_new_password: "تعيين كلمة مرور جديدة",
modal_new_password_sub: "اختر كلمة مرور قوية لحسابك",
modal_nid_placeholder: "الرقم القومي المكون من 14 رقم",
divider_or: "أو",
modal_from_nid: "من الرقم القومي",
auth_create: "إنشاء حساب",
modal_min_pass: "الحد الأدنى 8 أحرف، من A إلى Z، من 0 إلى 9، رمز",
modal_ph_first: "محمد",
modal_ph_last: "أحمد",

    // ── Features strip ───────────────────────────────────
    feat_ai_title: "خطط مدعومة بالذكاء الاصطناعي",
    feat_ai_desc: "برامج تدريبية ذكية مصممة لأهدافك",
    feat_book_title: "حجز سهل",
    feat_book_desc: "احجز أي فصل في ثوانٍ، في أي وقت",
    feat_train_title: "مدربون متخصصون",
    feat_train_desc: "محترفون معتمدون لإرشادك في كل خطوة",
    feat_track_title: "تتبع تقدمك",
    feat_track_desc: "تحليلات فورية لرحلتك الرياضية",
    feat_fac_title: "مرافق عالمية",
    feat_fac_desc: "صالات رياضية فاخرة وحمامات سباحة وملاعب",

    // ── Why Clubly section ────────────────────────────────
    why_label: "لماذا كلابلي",
    why_title1: "أكثر من مجرد",
    why_title2: "عضوية نادي",
    why_sub: "نؤمن بأن اللياقة أسلوب حياة. كلابلي يمنحك كل ما تحتاجه لبناء عادات صحية تدوم.",
    why_personal_title: "تجربة شخصية",
    why_personal_desc: "كل خطة مصممة لجسمك وأهدافك وجدولك الزمني.",
    why_community_title: "مجتمع نابض",
    why_community_desc: "شارك في التحديات الجماعية والفعاليات وتواصل مع الأعضاء.",
    why_safe_title: "آمن ومعتمد",
    why_safe_desc: "جميع المدربين معتمدون والمرافق تستوفي معايير السلامة الدولية.",
    why_cta: "عرض خطط العضوية",
    why_mind_title: "العقل والجسم",
why_mind_desc: "يوغا، تأمل، بيلاتس",
why_strength_title: "القوة والأداء",
why_strength_desc: "كروس فيت، أوزان، هيت",
why_aqua_title: "لياقة مائية",
why_aqua_desc: "سباحة، إيروبيك مائي",
why_dance_title: "رقص ومرح",
why_dance_desc: "زومبا، دانس فيت",

    // ── Schedule ─────────────────────────────────────────
    sched_label: "هذا الأسبوع",
    sched_title: "جدول الفصول المميزة",
    sched_sub: "اكتشف ما يحدث في كلابلي هذا الأسبوع.",
    sched_col_class: "الفصل",
    sched_col_trainer: "المدرب",
    sched_col_day: "اليوم",
    sched_col_time: "الوقت",
    sched_col_spots: "الأماكن المتبقية",
    sched_left: "متبقي",
sched_full: "مكتمل",
    sched_col_location: "الموقع",
    sched_full: "مكتمل",
    sched_left: "متبقي",

    // ── CTA banner ───────────────────────────────────────
    cta_title: "هل أنت مستعد لتغيير حياتك؟",
    cta_sub: "انضم إلى أكثر من 5000 عضو غيّروا حياتهم بالفعل. رحلتك تبدأ بخطوة واحدة.",
    cta_btn2: "احجز جولة",

    // ── About Page ───────────────────────────────────────
    about_label: "قصتنا",
    about_brand: "كلابلي",
    about_community: "المجتمع",
    about_founded: "تأسست كلابلي عام 2018 على قناعة واحدة: اللياقة يجب أن تكون متاحة وملهمة وممتعة للجميع — من المبتدئين حتى الرياضيين المحترفين.",
    about_mission_title: "مهمتنا",
    about_mission: "تمكين كل فرد من الوصول إلى أعلى مستويات لياقته البدنية والذهنية من خلال تدريب عالمي المستوى ومرافق متطورة ومجتمع داعم يحتفل بكل إنجاز.",
    about_vision_title: "رؤيتنا",
    about_vision: "أن نصبح مجتمع اللياقة الأكثر محبة في مصر — حيث تتلاقى تقنية اللياقة والروابط الإنسانية والإرشاد المتخصص لتغيير الحياة جلسة تلو الأخرى.",
    about_values_title: "قيمنا",
    about_values: "المجتمع أولاً. النزاهة دائماً. الابتكار في كل شيء. نؤمن باللياقة الشاملة والتدريب الصادق والسعي المستمر لتجاوز حدود ما يمكن أن يكون عليه النادي الرياضي.",
    about_stat1: "عضو سعيد",
    about_stat2: "مدرب متخصص",
    about_stat3: "نشاط متاح",
    about_stat4: "سنوات من التميز",
    about_stat5: "متوسط التقييم",
    about_journey_label: "رحلتنا",
    about_journey_sub: "من رؤية صغيرة إلى الوجهة الرياضية الأولى في مصر.",
    about_2018_label: "2018 — البداية",
    about_2018_title: "كلابلي تفتح أبوابها",
    about_2018_desc: "بدأنا بفرع واحد في المعادي بالقاهرة، و12 مدرباً وحلم لتغيير تجربة المصريين مع اللياقة. أصبح أول 200 عضو عائلتنا المؤسسة.",
    about_2020_label: "2020 — النمو",
    about_2020_title: "التحول الرقمي",
    about_2020_desc: "أطلقنا المنصة الرقمية لكلابلي، مما يتيح الحجز الإلكتروني والفصول الافتراضية وتوصيات اللياقة بالذكاء الاصطناعي — لإبقاء مجتمعنا نشطاً في الأوقات الصعبة.",
    about_2022_label: "2022 — التوسع",
    about_2022_title: "3 فروع جديدة + حمام السباحة الأولمبي",
    about_2022_desc: "افتتحنا 3 فروع جديدة في القاهرة والجيزة، وأضفنا حمام سباحة بمستوى أولمبي، وتجاوزنا 3000 عضو نشط.",
    about_2025_label: "2025 — اليوم",
    about_2025_title: "منصة النادي الرياضي #1 في مصر",
    about_2025_desc: "أكثر من 5000 عضو و80+ مدرب معتمد و30+ نشاط — ونحن بدأنا فقط. تجربة الأعضاء المدعومة بالذكاء الاصطناعي تضع معياراً جديداً للأندية الرياضية في المنطقة.",
    about_leadership_title: "تعرف على القيادة",
    about_leadership_sub: "الفريق المتحمس وراء رؤية كلابلي.",
    about_why_title: "لماذا تختار كلابلي؟",
    about_why_sub: "ما يجعلنا مختلفين عن كل صالة رياضية أخرى.",
    about_ai_title: "برامج مدعومة بالذكاء الاصطناعي",
    about_ai_desc: "خطط تدريبية شخصية تتكيف مع تقدمك وأهدافك وجدولك الزمني في الوقت الفعلي.",
    about_trainers_title: "مدربون متخصصون",
    about_trainers_desc: "جميع مدربينا معتمدون ومدققون وتقييماتهم من الأعضاء لضمان أعلى جودة.",
    about_fac_title: "مرافق عالمية",
    about_fac_desc: "حمام سباحة أولمبي وأجهزة رياضية متطورة واستوديوهات متعددة — كلها تُصان بأعلى المعايير.",
    about_family_title: "مناسب للعائلة",
    about_family_desc: "برامج خاصة للأطفال والمراهقين وكبار السن — لياقة للعائلة بأكملها.",
    about_book_title: "حجز سهل",
    about_book_desc: "احجز أي فصل أو مدرب أو مرفق في ثوانٍ عبر تطبيقنا أو الموقع — بدون انتظار أو متاعب.",
    about_safety_title: "السلامة أولاً",
    about_safety_desc: "كل مدرب معتمد في الإسعافات الأولية. مرافقنا تستوفي معايير السلامة الدولية مع مراقبة على مدار الساعة.",
    about_cta_title: "هل أنت مستعد لتكون جزءاً من قصتنا؟",
    about_cta_sub: "انضم إلى أكثر من 5000 عضو غيّروا حياتهم. رحلتك تبدأ بخطوة واحدة.",
    about_cta_btn1: "انضم الآن",
    about_cta_btn2: "تحدث معنا",

    // ── Activities Page ───────────────────────────────────
    act_title: "الأنشطة الرياضية",
    act_sub: "اكتشف مجموعة متنوعة من الأنشطة المثيرة المصممة للمتعة واللياقة ولحظات لا تُنسى.",
    act_filter_all: "جميع الأنشطة",
    act_filter_cat: "جميع الفئات",
    act_filter_exp: "أي مستوى",
    act_filter_price: "أي سعر",
    act_filter_cap: "أي سعه",
    act_exp_junior: "مبتدئ (1–3 سنوات)",
    act_exp_mid: "متوسط (4–7 سنوات)",
    act_exp_senior: "متقدم (8+ سنوات)",
    act_price_budget: "اقتصادي (≤100 جنيه)",
    act_price_mid: "متوسط (101–300 جنيه)",
    act_price_premium: "مميز (+300 جنيه)",
    act_cap_small: "صغير (<50)",
    act_cap_medium: "متوسط (50–100)",
    act_cap_large: "كبير (>100)",
    act_clear: "مسح",
    act_mind: "العقل والجسد",
    act_strength: "القوة والطاقة",
    act_aqua: "اللياقة المائية",
    act_dance: "الرقص والمرح",
    act_yoga: "يوغا، تأمل، بيلاتس",
    act_crossfit: "كروس فيت، أوزان، هيت",
    act_swim: "سباحة، إيروبيك مائي",
    act_zumba: "زومبا، رقص لياقة",
act_search: "ابحث عن نشاط...",
act_book_now: "احجز الآن",
act_signin_to_book: "سجل الدخول لحجز",


    // ── Facilities Page ───────────────────────────────────
    fac_title: "مرافقنا",
    fac_sub: "اكتشف مرافقنا العالمية المصممة لتعزيز تجربتك وتلبية جميع احتياجاتك الترفيهية.",
    fac_filter_all: "جميع المرافق",
    fac_gym: "قاعة الأجهزة",
    fac_pool: "حمام السباحة الأولمبي",
    fac_studio_a: "الاستوديو A",
    fac_studio_b: "الاستوديو B",
    fac_main: "القاعة الرئيسية",
    fac_hours: "ساعات العمل",
    fac_daily: "يومياً 6:00 صباحاً – 10:00 مساءً",
fac_search: "ابحث بالاسم أو الوصف...",

    // ── Trainers Page ─────────────────────────────────────
    tr_title: "مدربونا",
    tr_sub: "تعرف على مدربينا المحترفين الذين يساعدونك على تحقيق أهدافك.",
    tr_filter_all: "جميع الفئات",
    tr_personal: "تدريب شخصي",
    tr_certified: "جميع مدربينا معتمدون ومدققون وتقييماتهم من الأعضاء لضمان أعلى جودة.",
    tr_book_signin: "سجل الدخول لحجز فصل",

    // ── Plans Page ───────────────────────────────────────
    plans_title: "اختر خطتك",
    plans_sub: "عضويات مرنة مصممة لتناسب أسلوب حياتك وميزانيتك. إلغاء في أي وقت.",
    plans_cta: "انضم إلى أكثر من 2400 عضو في رحلتهم الرياضية.",

    // ── Contact Page ─────────────────────────────────────
    contact_title: "تواصل معنا",
    contact_sub: "لديك أسئلة؟ فريقنا هنا للمساعدة 7 أيام في الأسبوع.",
    contact_name_first: "الاسم الأول",
    contact_name_last: "اسم العائلة",
    contact_email: "البريد الإلكتروني",
    contact_phone: "رقم الهاتف",
    contact_topic: "الموضوع",
    contact_topic_general: "استفسار عام",
    contact_topic_support: "الدعم التقني",
    contact_topic_other: "أخرى",
    contact_msg: "أرسل لنا رسالة",
    contact_send: "إرسال الرسالة",
    contact_success: "تم إرسال رسالتك! سنرد عليك خلال 24 ساعة.",
    contact_address: "123 شارع الفيتنس، المعادي، القاهرة",
    contact_love: "يسعدنا سماعك",
    contact_team: "لديك أسئلة؟ فريقنا هنا للمساعدة 7 أيام في الأسبوع.",
    contact_location: "الموقع",
    contact_email_label: "البريد الإلكتروني",
    contact_phone_label: "الهاتف",

    // ── FAQ Page ─────────────────────────────────────────
    faq_title: "الأسئلة الشائعة",
    faq_sub: "كل ما تحتاج معرفته عن كلابلي قبل الانضمام.",
    faq_still: "لا تزال لديك أسئلة؟",
    faq_contact: "تواصل معنا",

    // ── Auth / Sign In ───────────────────────────────────
    auth_welcome: "مرحباً بعودتك 👋",
    auth_select_role: "اختر دورك وأدخل بيانات الدخول",
    auth_email: "البريد الإلكتروني",
    auth_password: "كلمة المرور",
    auth_signin: "تسجيل الدخول",
    auth_forgot: "نسيت كلمة المرور؟",
    auth_no_account: "ليس لديك حساب؟",
    auth_create: "إنشاء حساب",
    auth_create_title: "إنشاء حساب جديد",
    auth_strong_pass: "اختر كلمة مرور قوية لحسابك",
    auth_first: "الاسم الأول",
    auth_last: "اسم العائلة",
    auth_dob: "تاريخ الميلاد",
    auth_gender: "الجنس",
    auth_national_id: "الرقم القومي",
    auth_phone: "رقم الهاتف",
    auth_confirm_pass: "تأكيد كلمة المرور",
    auth_already: "لديك حساب بالفعل؟",
    auth_forgot_title: "نسيت كلمة المرور؟",
    auth_forgot_sub: "أدخل بريدك الإلكتروني وسنرسل لك كود إعادة التعيين",
    auth_send_code: "إرسال كود إعادة التعيين",
    auth_remember: "تتذكر كلمة المرور؟",
    auth_verify: "التحقق من الكود",
    auth_check_email: "تحقق من بريدك الإلكتروني",
    auth_sent_code: "أرسلنا كوداً مكوناً من 6 أرقام إلى",
    auth_didnt_receive: "لم تستلمه؟",
    auth_resend: "إعادة الإرسال",
    auth_new_pass: "كلمة المرور الجديدة",
    auth_set_pass: "تعيين كلمة المرور الجديدة",
    auth_change_email: "← تغيير البريد الإلكتروني",
    auth_guest: "التسجيل كضيف",
    auth_guest_label: "ضيف",
    auth_guest_sub: "تسجيل الضيف · سيتم مراجعة طلبك",
    auth_member: "عضو",
    auth_trainer: "مدرب",
    auth_admin: "مدير",
    auth_weak: "ضعيف",
    auth_auto: "تلقائي",
    auth_confirm_slot: "✔ تأكيد الموعد",

    // ── Dashboard ────────────────────────────────────────
    dash_community: "المجتمع",
    dash_sports: "الأنشطة الرياضية",
    dash_ai: "خطط الذكاء الاصطناعي",
    dash_pricing: "الأسعار",

    // ── Footer ───────────────────────────────────────────
    footer_explore: "استكشف",
    footer_help: "المساعدة",
    footer_follow: "تابعنا",
    footer_rights: "© 2025 كلابلي. جميع الحقوق محفوظة.",
    footer_privacy: "سياسة الخصوصية",
    footer_terms: "شروط الخدمة",
    footer_tagline: "رحلة عائلتك نحو اللياقة تبدأ هنا.",
    footer_faq: "الأسئلة الشائعة",
    footer_contact: "تواصل معنا",
    footer_join: "انضم الآن",
    footer_address: "123 شارع الفيتنس، المعادي، القاهرة، مصر",
    footer_hours: "يومياً 6 صباحاً – 10 مساءً",
  }
};

// ── Core engine ───────────────────────────────────────────────
function clublyGetLang() {
  return localStorage.getItem('clubly_lang') || 'en';
}

function clublySetLang(lang) {
  localStorage.setItem('clubly_lang', lang);
  applyLang(lang);
}

function applyLang(lang) {
  lang = lang || clublyGetLang();
  const T = CLUBLY_TRANSLATIONS[lang];
  if (!T) return;

  const isAr = lang === 'ar';

  // RTL / LTR
  document.documentElement.lang = lang;
  document.documentElement.dir  = isAr ? 'rtl' : 'ltr';

  // Apply all data-i18n elements
  document.querySelectorAll('[data-i18n]').forEach(el => {
    const key = el.getAttribute('data-i18n');
    if (T[key] !== undefined) el.textContent = T[key];
  });

  // Apply placeholders
  document.querySelectorAll('[data-i18n-ph]').forEach(el => {
    const key = el.getAttribute('data-i18n-ph');
    if (T[key] !== undefined) el.placeholder = T[key];
  });

  // Update lang toggle button text
  document.querySelectorAll('.clubly-lang-btn').forEach(btn => {
    btn.textContent = isAr ? 'EN' : 'عر';
    btn.setAttribute('title', isAr ? 'Switch to English' : 'التبديل إلى العربية');
  });

  // Cairo font already works for both — add Tajawal for Arabic for better look
  if (isAr) {
    if (!document.getElementById('clubly-ar-font')) {
      const link = document.createElement('link');
      link.id = 'clubly-ar-font';
      link.rel = 'stylesheet';
      link.href = 'https://fonts.googleapis.com/css2?family=Tajawal:wght@400;500;700;800;900&display=swap';
      document.head.appendChild(link);
    }
    document.body.style.fontFamily = "'Tajawal', 'Cairo', sans-serif";
  } else {
    document.body.style.fontFamily = "'Cairo', sans-serif";
  }
}

// ── Inject lang button into .header-actions ───────────────────
function clublyInjectLangBtn() {
  const actions = document.querySelector('.header-actions');
  if (!actions || document.querySelector('.clubly-lang-btn')) return;

  const btn = document.createElement('button');
  btn.className = 'btn-theme clubly-lang-btn';
  btn.style.cssText = 'font-size:.8rem;font-weight:700;width:38px;height:38px;letter-spacing:.5px;font-family:inherit;';
  const lang = clublyGetLang();
  btn.textContent = lang === 'ar' ? 'EN' : 'عر';
  btn.title = lang === 'ar' ? 'Switch to English' : 'التبديل إلى العربية';
  btn.onclick = () => clublySetLang(clublyGetLang() === 'ar' ? 'en' : 'ar');

  // Insert before the first button (theme toggle)
  const themeBtn = actions.querySelector('.btn-theme');
  if (themeBtn) actions.insertBefore(btn, themeBtn);
  else actions.prepend(btn);
}

// ── Auto-init ─────────────────────────────────────────────────
document.addEventListener('DOMContentLoaded', () => {
  clublyInjectLangBtn();
  applyLang();
});

// Also run after any dynamic header rebuild (some pages rebuild header on login)
document.addEventListener('clubly-header-ready', () => {
  clublyInjectLangBtn();
  applyLang();
});
