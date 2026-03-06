"""
recommend_app.py — Sports Recommendation UI (Mock Data)
=========================================================
Run:  python recommend_app.py
UI:   http://localhost:5000/recommend-ui
"""

from flask import Flask, request, jsonify, send_from_directory
from flask_cors import CORS
import random
import os

app  = Flask(__name__)
CORS(app)
BASE = os.path.dirname(os.path.abspath(__file__))

# ── بيانات الرياضات ──────────────────────────────
SPORTS = [
    {"sport": "Gym",                 "diff": "Medium", "env": "Indoor",  "budget": "Medium", "type": "Individual", "cal": 400},
    {"sport": "Swimming",            "diff": "Medium", "env": "Both",    "budget": "Medium", "type": "Individual", "cal": 500},
    {"sport": "Yoga",                "diff": "Low",    "env": "Indoor",  "budget": "Low",    "type": "Individual", "cal": 200},
    {"sport": "Football",            "diff": "High",   "env": "Outdoor", "budget": "Low",    "type": "Team",       "cal": 600},
    {"sport": "Basketball",          "diff": "High",   "env": "Indoor",  "budget": "Low",    "type": "Team",       "cal": 550},
    {"sport": "Cycling",             "diff": "Low",    "env": "Outdoor", "budget": "Medium", "type": "Individual", "cal": 480},
    {"sport": "CrossFit",            "diff": "High",   "env": "Indoor",  "budget": "High",   "type": "Both",       "cal": 700},
    {"sport": "Bodybuilding",        "diff": "High",   "env": "Indoor",  "budget": "Medium", "type": "Individual", "cal": 450},
    {"sport": "Marathon Running",    "diff": "High",   "env": "Outdoor", "budget": "Low",    "type": "Individual", "cal": 650},
    {"sport": "Tennis",              "diff": "Medium", "env": "Both",    "budget": "Medium", "type": "Individual", "cal": 450},
    {"sport": "Badminton",           "diff": "Low",    "env": "Indoor",  "budget": "Low",    "type": "Individual", "cal": 420},
    {"sport": "Volleyball",          "diff": "Medium", "env": "Both",    "budget": "Low",    "type": "Team",       "cal": 380},
    {"sport": "Boxing",              "diff": "High",   "env": "Indoor",  "budget": "Medium", "type": "Individual", "cal": 600},
    {"sport": "Climbing",            "diff": "High",   "env": "Both",    "budget": "Medium", "type": "Individual", "cal": 550},
    {"sport": "Weightlifting",       "diff": "High",   "env": "Indoor",  "budget": "Medium", "type": "Individual", "cal": 420},
    {"sport": "Taekwondo",           "diff": "High",   "env": "Indoor",  "budget": "Medium", "type": "Individual", "cal": 580},
    {"sport": "Dance Sport",         "diff": "Medium", "env": "Indoor",  "budget": "Medium", "type": "Individual", "cal": 380},
    {"sport": "Athletics",           "diff": "Medium", "env": "Outdoor", "budget": "Low",    "type": "Individual", "cal": 520},
    {"sport": "Brazilian Jiu-Jitsu", "diff": "High",   "env": "Indoor",  "budget": "Medium", "type": "Individual", "cal": 580},
    {"sport": "Karate",              "diff": "High",   "env": "Indoor",  "budget": "Medium", "type": "Individual", "cal": 560},
    {"sport": "Handball",            "diff": "High",   "env": "Indoor",  "budget": "Low",    "type": "Team",       "cal": 550},
    {"sport": "Judo",                "diff": "High",   "env": "Indoor",  "budget": "Medium", "type": "Individual", "cal": 570},
    {"sport": "Squash",              "diff": "High",   "env": "Indoor",  "budget": "Medium", "type": "Individual", "cal": 650},
    {"sport": "Triathlon",           "diff": "High",   "env": "Outdoor", "budget": "High",   "type": "Individual", "cal": 700},
    {"sport": "Hockey",              "diff": "High",   "env": "Both",    "budget": "High",   "type": "Team",       "cal": 560},
    {"sport": "Gymnastics",          "diff": "High",   "env": "Indoor",  "budget": "Medium", "type": "Individual", "cal": 480},
    {"sport": "Archery",             "diff": "Low",    "env": "Both",    "budget": "Medium", "type": "Individual", "cal": 200},
    {"sport": "Surfing",             "diff": "High",   "env": "Outdoor", "budget": "High",   "type": "Individual", "cal": 450},
    {"sport": "Diving",              "diff": "Medium", "env": "Both",    "budget": "High",   "type": "Individual", "cal": 350},
    {"sport": "Kick-boxing",         "diff": "High",   "env": "Indoor",  "budget": "Medium", "type": "Individual", "cal": 620},
    {"sport": "Chess",               "diff": "Low",    "env": "Indoor",  "budget": "Low",    "type": "Individual", "cal": 80},
    {"sport": "eSports",             "diff": "Low",    "env": "Indoor",  "budget": "Low",    "type": "Both",       "cal": 60},
    {"sport": "Bowling",             "diff": "Low",    "env": "Indoor",  "budget": "Low",    "type": "Individual", "cal": 200},
    {"sport": "Billiards",           "diff": "Low",    "env": "Indoor",  "budget": "Low",    "type": "Individual", "cal": 150},
    {"sport": "Equestrian",          "diff": "Medium", "env": "Outdoor", "budget": "High",   "type": "Individual", "cal": 320},
    {"sport": "Shooting",            "diff": "Low",    "env": "Indoor",  "budget": "High",   "type": "Individual", "cal": 150},
    {"sport": "Ballet",              "diff": "High",   "env": "Indoor",  "budget": "Medium", "type": "Individual", "cal": 400},
    {"sport": "Parkour",             "diff": "High",   "env": "Outdoor", "budget": "Low",    "type": "Individual", "cal": 600},
    {"sport": "Wrestling",           "diff": "High",   "env": "Indoor",  "budget": "Low",    "type": "Individual", "cal": 600},
    {"sport": "American Football",   "diff": "High",   "env": "Outdoor", "budget": "High",   "type": "Team",       "cal": 580},
    {"sport": "Multi Martial Arts",  "diff": "High",   "env": "Indoor",  "budget": "Medium", "type": "Individual", "cal": 600},
    {"sport": "Bungee Jumping",      "diff": "Medium", "env": "Outdoor", "budget": "High",   "type": "Individual", "cal": 300},
    {"sport": "Kids Martial Arts",   "diff": "Low",    "env": "Indoor",  "budget": "Low",    "type": "Individual", "cal": 400},
    {"sport": "Mind Sports / Board Games", "diff": "Low", "env": "Indoor", "budget": "Low",  "type": "Individual", "cal": 70},
]

# ── قواعد التوصية ────────────────────────────────
HEALTH_RESTRICTIONS = {
    "Knee Pain":       ["Marathon Running", "Football", "Basketball", "Squash", "Taekwondo", "Parkour", "Kick-boxing"],
    "Back Pain":       ["Wrestling", "Weightlifting", "Gymnastics", "CrossFit", "Bodybuilding", "Boxing"],
    "Heart Condition": [],  # هيتفلتر على الـ difficulty
    "Asthma":          ["Marathon Running", "Triathlon", "CrossFit", "Squash", "Athletics"],
}

DANGEROUS_KIDS = ["Bungee Jumping", "Squash", "Triathlon", "Boxing", "Kick-boxing",
                  "Wrestling", "American Football", "Multi Martial Arts", "Parkour",
                  "Surfing", "CrossFit", "Bodybuilding", "Weightlifting", "Marathon Running"]

def score_sport(sport, data):
    """بيحسب score لكل رياضة بناءً على بيانات المستخدم"""
    score = 50.0
    age          = data.get("age", 25)
    goal         = data.get("goal", "Stay Healthy")
    fitness      = data.get("fitness_level", "Beginner")
    activity     = data.get("activity_level", "Moderately Active")
    personality  = data.get("personality", "Ambivert")
    health       = data.get("health_issue", "None")
    outdoor_pref = data.get("prefers_outdoor", 0)
    team_pref    = data.get("prefers_team_sport", 0)
    budget       = data.get("monthly_budget_usd", 50)
    hours        = data.get("hours_available_per_week", 5)
    s            = sport

    # ── فلترة صارمة ──
    # أطفال
    if age < 14 and s["sport"] in DANGEROUS_KIDS:
        return -1
    if age < 10 and s["diff"] == "High":
        return -1

    # صحة
    banned = HEALTH_RESTRICTIONS.get(health, [])
    if s["sport"] in banned:
        return -1
    if health == "Heart Condition" and s["diff"] == "High":
        return -1

    # ميزانية
    budget_map = {"Low": 20, "Medium": 60, "High": 120}
    if budget < budget_map.get(s["budget"], 60):
        return -1

    # وقت
    if hours < 2 and s["sport"] in ["Triathlon", "Marathon Running", "CrossFit", "Bodybuilding"]:
        return -1

    # ── تسجيل النقاط ──
    # هدف
    if goal == "Lose Weight"       and s["cal"] >= 500:   score += 25
    if goal == "Lose Weight"       and s["cal"] >= 600:   score += 10
    if goal == "Build Muscle"      and s["sport"] in ["Gym","Weightlifting","Bodybuilding","CrossFit","Boxing","Brazilian Jiu-Jitsu","Climbing"]: score += 35
    if goal == "Improve Endurance" and s["sport"] in ["Swimming","Cycling","Marathon Running","Triathlon","Athletics"]: score += 30
    if goal == "Reduce Stress"     and s["sport"] in ["Yoga","Swimming","Cycling","Dance Sport","Tennis"]: score += 30
    if goal == "Have Fun"          and s["diff"] in ["Low","Medium"]: score += 20
    if goal == "Stay Healthy"      and s["diff"] != "High": score += 15

    # لياقة
    fit_diff = {"Beginner": "Low", "Intermediate": "Medium", "Advanced": "High"}
    if fit_diff.get(fitness) == s["diff"]: score += 20

    # نشاط
    act_diff = {"Sedentary": "Low", "Lightly Active": "Low", "Moderately Active": "Medium", "Very Active": "High"}
    if act_diff.get(activity) == s["diff"]: score += 15

    # شخصية
    if personality == "Introvert"  and s["type"] == "Individual": score += 15
    if personality == "Extrovert"  and s["type"] in ["Team","Both"]: score += 15
    if personality == "Ambivert"   and s["type"] == "Both": score += 10

    # تفضيلات
    if team_pref    == 1 and s["type"] in ["Team","Both"]:      score += 15
    if outdoor_pref == 1 and s["env"]  in ["Outdoor","Both"]:   score += 15
    if outdoor_pref == 0 and s["env"]  in ["Indoor","Both"]:    score += 10

    # إضافة عشوائية خفيفة للتنوع
    score += random.uniform(-3, 3)

    return max(0, score)


@app.route("/")
def index():
    return jsonify({"service": "Sports Recommendation UI", "status": "running"})

@app.route("/recommend-ui")
def recommend_ui():
    return send_from_directory(BASE, "recommendation.html")

@app.route("/recommend", methods=["POST"])
def recommend():
    data    = request.get_json(force=True)
    top_n   = int(data.get("top_n", 5))

    # احسب score لكل رياضة
    scored = []
    for s in SPORTS:
        sc = score_sport(s, data)
        if sc >= 0:
            scored.append((s["sport"], sc))

    # رتّب تنازلياً
    scored.sort(key=lambda x: -x[1])
    top     = scored[:top_n]
    max_sc  = top[0][1] if top else 1

    recommendations = [
        {
            "rank":       i + 1,
            "sport":      name,
            "confidence": round(sc / max_sc, 4)
        }
        for i, (name, sc) in enumerate(top)
    ]

    return jsonify({
        "status":          "success",
        "recommendations": recommendations,
        "note":            "Mock data — connect ML model for real predictions"
    })


if __name__ == "__main__":
    print("=" * 50)
    print("  🏆 Sports Recommendation UI")
    print("  http://localhost:5000/recommend-ui")
    print("=" * 50)
    app.run(debug=True, port=5000)
