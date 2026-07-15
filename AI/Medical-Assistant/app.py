"""
app.py — Clubly Medical Assistant
مساعد طبي رياضي متخصص في الإصابات، التغذية، والحالات المرضية
"""
from flask import Flask, request, jsonify, send_from_directory
from flask_cors import CORS
from groq import Groq
import os, re, requests

app  = Flask(__name__)
CORS(app)
BASE = os.path.dirname(os.path.abspath(__file__))

# ── In-Memory Sessions ────────────────────────────────────────────────────────
sessions = {}

# ── Clubly API ────────────────────────────────────────────────────────────────
CLUBLY_API = os.environ.get("CLUBLY_API_URL", "http://clublywebsite.runasp.net/api")

def fetch_endpoint(path: str, retries: int = 3) -> list:
    for attempt in range(retries):
        try:
            res = requests.get(f"{CLUBLY_API}/{path}", timeout=10)
            res.raise_for_status()
            data = res.json()
            if data:
                return data
        except Exception:
            if attempt < retries - 1:
                import time; time.sleep(1)
    return []

def fetch_activities() -> list: return fetch_endpoint("Activities")
def fetch_trainers()   -> list: return fetch_endpoint("Trainers")

def format_activities_for_medical(activities: list) -> str:
    if not activities:
        return ""
    active = [a for a in activities if a.get("status") == "Active"]
    if not active:
        return ""
    lines = ["الأنشطة الرياضية المتاحة في Clubly:"]
    for a in active:
        line = f"- {a.get('name','')} في {a.get('facilityName','')}"
        if a.get("price"):
            line += f" بسعر {a.get('price')} جنيه"
        lines.append(line)
    return "\n".join(lines)

def format_trainers_for_medical(trainers: list) -> str:
    if not trainers:
        return ""
    active = [t for t in trainers if t.get("isActive")]
    if not active:
        return ""
    lines = ["المدربون المتاحون في Clubly:"]
    for t in active:
        lines.append(f"- {t.get('fullName','')} متخصص في {t.get('activities','')} | خبرة {t.get('yearsOfExperience','')} سنوات")
    return "\n".join(lines)

# ── System Prompt ─────────────────────────────────────────────────────────────
BASE_SYSTEM_PROMPT = """You are Clubly Medical Assistant — a professional sports medicine and health advisor integrated into the Clubly sports platform.

## Your Identity:
- Name: Clubly Medical Assistant
- Role: Professional sports medicine advisor and health consultant
- Tone: Clinical, empathetic, precise, and trustworthy — like a knowledgeable doctor friend

## Your Specializations:

### Sports Injuries & First Aid:
- Assess and guide on common sports injuries: sprains, strains, muscle tears, fractures, dislocations, concussions
- Provide immediate first-aid steps (RICE protocol, immobilization, when to go to ER)
- Estimate recovery timelines and return-to-sport guidelines
- Recommend rehabilitation exercises for post-injury recovery
- Identify injury-specific risks for each sport (swimming, football, tennis, boxing, etc.)

### Sports Nutrition & Diet:
- Pre-workout, intra-workout, and post-workout nutrition plans
- Macronutrient calculation (protein, carbohydrates, fats) based on goals
- Evidence-based supplement guidance (creatine, protein, BCAAs, vitamins) — benefits and risks
- Hydration strategies for different sports and climates
- Nutrition protocols for specific goals: weight loss, muscle gain, endurance, performance
- Healthy daily eating habits for active individuals

### Medical Conditions & Safe Exercise:
- **Cardiovascular disease**: safe exercise types, warning signs, contraindications
- **Diabetes (Type 1 & 2)**: exercise safety, blood glucose monitoring, hypoglycemia prevention
- **Hypertension / Hypotension**: safe sports, forbidden activities, danger signs
- **Asthma & respiratory conditions**: exercise-induced bronchospasm prevention, safe sports
- **Joint pain & arthritis**: low-impact alternatives, exercises to avoid
- **Post-surgery rehabilitation**: when to return to sport, progressive loading principles
- **Pregnancy**: trimester-specific safe exercises, contraindications
- **Elderly individuals**: age-appropriate activities, fall prevention, bone health

### General Health & Wellness:
- Sleep and recovery optimization for athletes
- Stress management and mental health in sports
- Overtraining syndrome recognition and prevention
- Body composition assessment and guidance
- Pain management strategies (non-pharmacological)

{clubly_context}

## Critical Safety Rules:
- 🚨 If symptoms suggest a medical emergency (chest pain, severe shortness of breath, loss of consciousness, severe head injury, numbness in limbs) — instruct the user to call emergency services IMMEDIATELY before anything else
- ⚠️ Always clarify you are providing guidance only, not a medical diagnosis or prescription
- Never recommend specific medications or dosages
- Always advise consulting a qualified physician or sports medicine doctor for serious conditions
- Be evidence-based — cite established medical guidelines when relevant (e.g., RICE, ACSM, WHO)

## Response Style:
- Be structured and clear — use headers and bullet points for complex answers
- Be concise for simple questions, detailed for complex ones
- Show empathy especially when someone is in pain or anxious
- Be proactive — if you notice a pattern (e.g., repeated knee injuries), address the root cause
- Always end with one relevant follow-up question or actionable next step

## STRICT Language Rule:
- If the question is in Arabic → respond 100% in Arabic (فصحى واضحة) — zero English words
- If the question is in English → respond 100% in English — zero Arabic words
- Never mix languages under any circumstances
- Detect language from the question itself, not previous messages

## Boundaries — What You Do NOT Do:
- Answer questions unrelated to health, medicine, sports, or nutrition
- Provide legal, financial, or relationship advice
- Discuss politics, religion, or controversial non-medical topics
- If asked about something outside your scope, politely redirect: "I specialize in sports medicine and health — I'd be happy to help with any health or fitness questions."
"""

# ── Utils ─────────────────────────────────────────────────────────────────────
def clean_reply(text: str) -> str:
    REMOVE = re.compile(
        u"[\u0400-\u04FF\u4E00-\u9FFF\u3040-\u30FF"
        u"\uAC00-\uD7AF\u0100-\u024F\u0250-\u02AF"
        u"\u0370-\u03FF\u0500-\u05FF]"
    )
    return REMOVE.sub("", text).strip()

def detect_lang(text: str) -> str:
    arabic  = len([c for c in text if '\u0600' <= c <= '\u06ff'])
    english = len([c for c in text if 'a' <= c.lower() <= 'z'])
    total   = arabic + english
    return 'ar' if (arabic / total >= 0.4 if total else True) else 'en'

# ── Routes ────────────────────────────────────────────────────────────────────
@app.route("/")
def index():
    return jsonify({"service": "Clubly Medical Assistant", "status": "running"})

@app.route("/chat")
def chat_ui():
    return send_from_directory(BASE, "medical_widget.html")

@app.route("/medical-chat", methods=["POST"])
def medical_chat():
    groq_key = os.environ.get("GROQ_API_KEY")
    if not groq_key:
        return jsonify({"error": "GROQ_API_KEY not set"}), 500

    data = request.get_json(force=True)
    if not data or not data.get("message", "").strip():
        return jsonify({"error": "Missing message"}), 400

    message    = data["message"].strip()
    session_id = data.get("session_id", "default")

    # ── In-memory history ──
    if session_id not in sessions:
        sessions[session_id] = []
    history = sessions[session_id]
    history.append({"role": "user", "content": message})
    if len(history) > 20:
        sessions[session_id] = history[-20:]
        history = sessions[session_id]

    # ── Fetch Clubly data (activities + trainers only) ──
    activities_ctx = format_activities_for_medical(fetch_activities())
    trainers_ctx   = format_trainers_for_medical(fetch_trainers())

    clubly_context = ""
    if activities_ctx or trainers_ctx:
        clubly_context = "## بيانات Clubly المتاحة:\n"
        if activities_ctx:
            clubly_context += activities_ctx + "\n\n"
        if trainers_ctx:
            clubly_context += trainers_ctx
        clubly_context += "\n\nلو نصحت بممارسة رياضة معينة، اذكر إذا كانت متاحة في Clubly."

    system_prompt = BASE_SYSTEM_PROMPT.format(clubly_context=clubly_context)

    # ── Language detection ──
    lang = detect_lang(message)
    lang_instruction = (
        "ردك يجب أن يكون بالعربية الفصحى 100% بدون أي كلمة إنجليزية."
        if lang == 'ar' else
        "Your reply must be 100% in English with NO Arabic."
    )

    messages_to_send = [
        {"role": "system", "content": system_prompt + "\n\n" + lang_instruction},
    ] + history + [
        {"role": "system", "content": "اكتب بالعربية فقط الآن." if lang == 'ar' else "Reply in English only now."}
    ]

    try:
        client   = Groq(api_key=groq_key)
        response = client.chat.completions.create(
            model="llama-3.3-70b-versatile",
            messages=messages_to_send,
            max_tokens=700,
            temperature=0.4,
        )
        reply = clean_reply(response.choices[0].message.content)
        history.append({"role": "assistant", "content": reply})
        return jsonify({"reply": reply, "session_id": session_id})

    except Exception as e:
        return jsonify({"error": str(e)}), 500

@app.route("/medical-chat/clear", methods=["POST"])
def clear():
    data       = request.get_json(force=True) or {}
    session_id = data.get("session_id", "default")
    sessions.pop(session_id, None)
    return jsonify({"status": "cleared"})

if __name__ == "__main__":
    port = int(os.environ.get("PORT", 7860))
    print("=" * 40)
    print("  🏥  Clubly Medical Assistant")
    print(f"  http://localhost:{port}/chat")
    print("=" * 40)
    app.run(debug=False, host="0.0.0.0", port=port)
