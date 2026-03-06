"""
chatbot_app.py — Sports Chatbot using Groq API
================================================
Run:  python chatbot_app.py
UI:   http://localhost:5000/chat
"""

from flask import Flask, request, jsonify, send_from_directory
from flask_cors import CORS
from groq import Groq
from dotenv import load_dotenv
import os

load_dotenv()

app    = Flask(__name__)
CORS(app)
client = Groq(api_key=os.getenv("GROQ_API_KEY"))
BASE   = os.path.dirname(os.path.abspath(__file__))

SYSTEM_PROMPT = """أنت مساعد رياضي ذكي اسمك Coach AI، متخصص فقط في الرياضات التالية:

Swimming, Cycling, Chess, Squash, Yoga, Mind Sports / Board Games, Tennis,
Dance Sport, Bowling, Equestrian, Wrestling, Billiards, Surfing, Parkour,
Archery, Shooting, eSports, Athletics, Gym, Basketball, Diving,
Brazilian Jiu-Jitsu, Weightlifting, Taekwondo, Triathlon, Football, Handball,
Judo, Volleyball, Kick-boxing, Boxing, Ballet, Karate, American Football,
Hockey, Badminton, Bodybuilding, Marathon Running, Multi Martial Arts,
CrossFit, Gymnastics, Climbing, Bungee Jumping, Kids Martial Arts

تجيب على أي سؤال متعلق بهذه الرياضات فقط مثل:
- فوائد كل رياضة وطريقة ممارستها
- المعدات المطلوبة والتكلفة
- مستوى الصعوبة والمناسبة لكل عمر
- التمارين والبرامج التدريبية
- الإصابات الشائعة والوقاية منها
- مقارنة بين رياضتين من القائمة

قواعد مهمة:
- لو سألوك عن رياضة مش في القائمة قول بلطف: 'أنا متخصص فقط في الرياضات المتاحة في منصتنا' واعرض عليهم القائمة
- رد بنفس لغة السؤال — عربي أو إنجليزي
- أسلوبك ودود ومحفز ومختصر وعملي"""
sessions = {}

@app.route("/")
def index():
    return jsonify({"service": "Coach AI", "status": "running"})

@app.route("/chat")
def chat_ui():
    return send_from_directory(BASE, "coach_widget.html")

@app.route("/sports-chat", methods=["POST"])
def sports_chat():
    data = request.get_json(force=True)
    if not data or not data.get("message", "").strip():
        return jsonify({"error": "Missing message"}), 400

    message    = data["message"].strip()
    session_id = data.get("session_id", "default")

    if session_id not in sessions:
        sessions[session_id] = []

    history = sessions[session_id]
    history.append({"role": "user", "content": message})

    if len(history) > 20:
        sessions[session_id] = history[-20:]
        history = sessions[session_id]

    try:
        response = client.chat.completions.create(
            model="llama-3.3-70b-versatile",
            messages=[{"role": "system", "content": SYSTEM_PROMPT}] + history,
            max_tokens=1024,
            temperature=0.7
        )

        reply = response.choices[0].message.content
        history.append({"role": "assistant", "content": reply})
        return jsonify({"reply": reply, "session_id": session_id})

    except Exception as e:
        return jsonify({"error": str(e)}), 500


@app.route("/sports-chat/clear", methods=["POST"])
def clear():
    data = request.get_json(force=True) or {}
    sessions.pop(data.get("session_id", "default"), None)
    return jsonify({"status": "cleared"})


if __name__ == "__main__":
    print("=" * 40)
    print("  🏋️  Coach AI — Sports Chatbot")
    print("  http://localhost:5000/chat")
    print("=" * 40)
    app.run(debug=True, port=5000)