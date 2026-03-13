"""
🏆 Sports Recommendation System — Flask API
Hybrid Model: Content-Based (XGBoost) + Collaborative Filtering (SVD+KNN)
"""

import os
import warnings
warnings.filterwarnings('ignore')

import numpy as np
import pandas as pd
import joblib
from flask import Flask, request, jsonify, send_from_directory
from flask_cors import CORS

# ──────────────────────────────────────────────
# Load Models
# ──────────────────────────────────────────────
BASE = os.path.dirname(__file__)
MODELS = os.path.join(BASE, 'models')

le_sport     = joblib.load(os.path.join(MODELS, 'label_encoder_sport.pkl'))
scaler       = joblib.load(os.path.join(MODELS, 'scaler.pkl'))
xgb_model    = joblib.load(os.path.join(MODELS, 'xgb_content_based.pkl'))
knn_cf       = joblib.load(os.path.join(MODELS, 'knn_cf_model.pkl'))
svd_model    = joblib.load(os.path.join(MODELS, 'svd_model.pkl'))
user_factors = np.load(os.path.join(MODELS, 'user_factors.npy'))
user_item    = pd.read_csv(os.path.join(MODELS, 'user_item_matrix.csv'), index_col=0)
feature_cols = pd.read_csv(os.path.join(MODELS, 'feature_cols.csv')).iloc[:, 0].tolist()

num_features = ['age', 'weight_kg', 'height_cm', 'bmi',
                'hours_available_per_week', 'monthly_budget_usd',
                'prefers_team_sport', 'prefers_outdoor',
                'prefers_contact_sport', 'risk_tolerance', 'competitive_level',
                'coordination_level', 'striking_preference', 'grappling_preference',
                'speed_agility', 'endurance_level', 'preferred_distance',
                'activity_encoded', 'fitness_encoded',
                'bmi_score', 'age_score', 'activity_score', 'fitness_score',
                'hours_score', 'budget_score', 'health_score',
                'bmi_x_activity', 'age_x_fitness', 'hours_x_fitness',
                'budget_x_location', 'intensity_score']

print("✅ All models loaded successfully!")

# ──────────────────────────────────────────────
# Health Assessment Functions
# ──────────────────────────────────────────────
def assess_bmi(bmi):
    if bmi < 18.5:   return 'underweight', 'gain_weight', 1
    elif bmi < 25:   return 'normal', 'maintain', 2
    elif bmi < 30:   return 'overweight', 'lose_weight', 3
    else:            return 'obese', 'lose_weight_urgent', 4

def assess_age(age):
    if age < 12:     return 'child', 'low_intensity', 1
    elif age < 18:   return 'teen', 'moderate_intensity', 2
    elif age < 35:   return 'young', 'high_intensity', 3
    elif age < 55:   return 'adult', 'moderate_intensity', 2
    else:            return 'senior', 'low_intensity', 1

def assess_activity(level):
    m = {'Sedentary':         ('below_normal', 'increase_activity', 1),
         'Lightly Active':    ('slightly_low', 'slightly_increase', 2),
         'Moderately Active': ('normal', 'maintain', 3),
         'Very Active':       ('above_normal', 'maintain_or_rest', 4)}
    return m.get(level, ('normal', 'maintain', 3))

def assess_fitness(level):
    m = {'Beginner':     ('low', 'start_gradual', 1),
         'Intermediate': ('medium', 'progress', 2),
         'Advanced':     ('high', 'maintain_challenge', 3)}
    return m.get(level, ('medium', 'progress', 2))

def assess_hours(h):
    if h < 1.5:    return 'very_low', 'increase_hours', 1
    elif h < 2.5:  return 'low', 'slightly_increase', 2
    elif h <= 5:   return 'normal', 'maintain', 3
    else:          return 'high', 'ensure_recovery', 4

def assess_budget(b):
    if b == 0:     return 'zero', 'free_sports_only', 1
    elif b < 20:   return 'low', 'low_cost_sports', 2
    elif b < 80:   return 'medium', 'standard_sports', 3
    else:          return 'high', 'all_sports', 4

def assess_health_issue(issue):
    avoid = {
        'Knee Pain':       ['Running', 'Basketball', 'Football', 'Volleyball', 'Squash'],
        'Back Pain':       ['Weightlifting', 'Gymnastics'],
        'Asthma':          ['Marathon Running', 'Triathlon', 'CrossFit'],
        'Heart Condition': ['Boxing', 'Kick-boxing', 'CrossFit', 'Triathlon', 'Weightlifting'],
    }
    prefer = {
        'Knee Pain':       ['Swimming', 'Cycling', 'Yoga', 'Chess'],
        'Back Pain':       ['Swimming', 'Yoga', 'Cycling'],
        'Asthma':          ['Swimming', 'Yoga', 'Chess', 'Archery'],
        'Heart Condition': ['Yoga', 'Swimming', 'Chess', 'Cycling'],
    }
    return avoid.get(issue, []), prefer.get(issue, [])

def compute_health_score(row):
    score = 50
    bmi_adj  = {'normal': +20, 'underweight': -15, 'overweight': -10, 'obese': -20}
    act_adj  = {'normal': +15, 'above_normal': +10, 'slightly_low': -5, 'below_normal': -15}
    fit_adj  = {'high': +10, 'medium': +5, 'low': 0}
    hrs_adj  = {'normal': +10, 'high': +8, 'low': -5, 'very_low': -10}
    score += bmi_adj.get(row.get('bmi_status', ''), 0)
    score += act_adj.get(row.get('activity_status', ''), 0)
    score += fit_adj.get(row.get('fitness_status', ''), 0)
    score += hrs_adj.get(row.get('hours_status', ''), 0)
    if str(row.get('health_issue', '')) not in ['', 'None', 'nan']:
        score -= 5
    return max(0, min(100, score))

# Sport metadata
SPORT_META = {
    'Swimming':          {'emoji': '🏊', 'category': 'Aquatic',    'intensity': 'Medium'},
    'Yoga':              {'emoji': '🧘', 'category': 'Mind-Body',  'intensity': 'Low'},
    'Football':          {'emoji': '⚽', 'category': 'Team',       'intensity': 'High'},
    'Basketball':        {'emoji': '🏀', 'category': 'Team',       'intensity': 'High'},
    'Tennis':            {'emoji': '🎾', 'category': 'Racket',     'intensity': 'Medium'},
    'Boxing':            {'emoji': '🥊', 'category': 'Combat',     'intensity': 'Very High'},
    'Cycling':           {'emoji': '🚴', 'category': 'Endurance',  'intensity': 'Medium'},
    'Chess':             {'emoji': '♟️', 'category': 'Mind Sport', 'intensity': 'Very Low'},
    'Karate':            {'emoji': '🥋', 'category': 'Martial Art','intensity': 'High'},
    'Archery':           {'emoji': '🏹', 'category': 'Precision',  'intensity': 'Low'},
    'CrossFit':          {'emoji': '🏋️', 'category': 'Fitness',    'intensity': 'Very High'},
    'Running':           {'emoji': '🏃', 'category': 'Endurance',  'intensity': 'High'},
    'Volleyball':        {'emoji': '🏐', 'category': 'Team',       'intensity': 'Medium'},
    'Gymnastics':        {'emoji': '🤸', 'category': 'Acrobatic',  'intensity': 'High'},
    'Judo':              {'emoji': '🤼', 'category': 'Combat',     'intensity': 'High'},
    'Taekwondo':         {'emoji': '🦵', 'category': 'Martial Art','intensity': 'High'},
    'Weightlifting':     {'emoji': '🏋️', 'category': 'Strength',   'intensity': 'High'},
    'Badminton':         {'emoji': '🏸', 'category': 'Racket',     'intensity': 'Medium'},
    'Climbing':          {'emoji': '🧗', 'category': 'Adventure',  'intensity': 'High'},
    'Surfing':           {'emoji': '🏄', 'category': 'Water',      'intensity': 'High'},
    'Gym':               {'emoji': '💪', 'category': 'Fitness',    'intensity': 'Medium'},
    'Bodybuilding':      {'emoji': '🦾', 'category': 'Strength',   'intensity': 'High'},
    'Marathon Running':  {'emoji': '🏃', 'category': 'Endurance',  'intensity': 'Very High'},
    'Triathlon':         {'emoji': '🏊', 'category': 'Endurance',  'intensity': 'Very High'},
    'Brazilian Jiu-Jitsu': {'emoji': '🥋', 'category': 'Martial Art', 'intensity': 'High'},
    'Wrestling':         {'emoji': '🤼', 'category': 'Combat',     'intensity': 'Very High'},
    'Kick-boxing':       {'emoji': '🥊', 'category': 'Combat',     'intensity': 'Very High'},
    'Squash':            {'emoji': '🎾', 'category': 'Racket',     'intensity': 'High'},
    'Hockey':            {'emoji': '🏑', 'category': 'Team',       'intensity': 'High'},
    'American Football': {'emoji': '🏈', 'category': 'Team',       'intensity': 'Very High'},
    'Handball':          {'emoji': '🤾', 'category': 'Team',       'intensity': 'High'},
    'Athletics':         {'emoji': '🏃', 'category': 'Track',      'intensity': 'High'},
    'Diving':            {'emoji': '🤿', 'category': 'Aquatic',    'intensity': 'Medium'},
    'Shooting':          {'emoji': '🎯', 'category': 'Precision',  'intensity': 'Low'},
    'Billiards':         {'emoji': '🎱', 'category': 'Precision',  'intensity': 'Very Low'},
    'Bowling':           {'emoji': '🎳', 'category': 'Precision',  'intensity': 'Low'},
    'Dance Sport':       {'emoji': '💃', 'category': 'Dance',      'intensity': 'Medium'},
    'Ballet':            {'emoji': '🩰', 'category': 'Dance',      'intensity': 'High'},
    'Equestrian':        {'emoji': '🐎', 'category': 'Animal Sport','intensity': 'Medium'},
    'Parkour':           {'emoji': '🤸', 'category': 'Adventure',  'intensity': 'Very High'},
    'Bungee Jumping':    {'emoji': '🪂', 'category': 'Extreme',    'intensity': 'Extreme'},
    'Kids Martial Arts': {'emoji': '🥋', 'category': 'Martial Art','intensity': 'Low'},
    'Multi Martial Arts':{'emoji': '🥊', 'category': 'Combat',     'intensity': 'High'},
    'Mind Sports / Board Games': {'emoji': '🎯', 'category': 'Mind Sport', 'intensity': 'Very Low'},
    'eSports':           {'emoji': '🎮', 'category': 'Digital',    'intensity': 'Very Low'},
}

# ──────────────────────────────────────────────
# Recommendation Engine
# ──────────────────────────────────────────────
def recommend_sport(data):
    age            = int(data['age'])
    gender         = data['gender']
    weight         = float(data['weight_kg'])
    height         = float(data['height_cm'])
    activity_level = data['activity_level']
    fitness_level  = data['fitness_level']
    goal           = data['goal']
    personality    = data['personality']
    location       = data['location']
    hours_per_week = float(data['hours_per_week'])
    budget         = float(data['budget'])
    health_issue   = data.get('health_issue', 'None')
    prefers_team   = int(data.get('prefers_team', 0))
    prefers_outdoor = int(data.get('prefers_outdoor', 0))
    prefers_contact = int(data.get('prefers_contact', 2))
    risk_tolerance  = int(data.get('risk_tolerance', 2))
    competitive_level = int(data.get('competitive_level', 2))
    coordination_level = int(data.get('coordination_level', 3))
    striking_preference = int(data.get('striking_preference', 2))
    grappling_preference = int(data.get('grappling_preference', 2))
    speed_agility   = int(data.get('speed_agility', 3))
    endurance_level = int(data.get('endurance_level', 3))
    preferred_distance = int(data.get('preferred_distance', 3))
    top_n           = int(data.get('top_n', 5))

    bmi = round(weight / ((height / 100) ** 2), 1)
    bmi_s, bmi_g, bmi_sc     = assess_bmi(bmi)
    age_g, age_i, age_sc     = assess_age(age)
    act_s, act_a, act_sc     = assess_activity(activity_level)
    fit_s, fit_a, fit_sc     = assess_fitness(fitness_level)
    hrs_s, hrs_a, hrs_sc     = assess_hours(hours_per_week)
    bud_s, bud_a, bud_sc     = assess_budget(budget)
    avoid, prefer            = assess_health_issue(health_issue)
    health_sc = compute_health_score({
        'bmi_status': bmi_s, 'activity_status': act_s,
        'fitness_status': fit_s, 'hours_status': hrs_s, 'health_issue': health_issue
    })

    act_map = {'Sedentary': 1, 'Lightly Active': 2, 'Moderately Active': 3, 'Very Active': 4}
    fit_map = {'Beginner': 1, 'Intermediate': 2, 'Advanced': 3}
    loc_map = {'Rural': 1, 'Suburban': 2, 'Urban': 3}

    new_row = {
        'age': age, 'weight_kg': weight, 'height_cm': height, 'bmi': bmi,
        'hours_available_per_week': hours_per_week, 'monthly_budget_usd': budget,
        'prefers_team_sport': prefers_team, 'prefers_outdoor': prefers_outdoor,
        'prefers_contact_sport': prefers_contact, 'risk_tolerance': risk_tolerance,
        'competitive_level': competitive_level, 'coordination_level': coordination_level,
        'striking_preference': striking_preference, 'grappling_preference': grappling_preference,
        'speed_agility': speed_agility, 'endurance_level': endurance_level,
        'preferred_distance': preferred_distance,
        'activity_encoded': {'Sedentary': 0, 'Lightly Active': 1, 'Moderately Active': 2, 'Very Active': 3}.get(activity_level, 1),
        'fitness_encoded':  {'Beginner': 0, 'Intermediate': 1, 'Advanced': 2}.get(fitness_level, 0),
        'bmi_score': bmi_sc, 'age_score': age_sc, 'activity_score': act_sc,
        'fitness_score': fit_sc, 'hours_score': hrs_sc, 'budget_score': bud_sc,
        'health_score': health_sc,
        'bmi_x_activity':    bmi * act_map.get(activity_level, 1),
        'age_x_fitness':     age * fit_map.get(fitness_level, 1),
        'hours_x_fitness':   hours_per_week * fit_map.get(fitness_level, 1),
        'budget_x_location': budget * loc_map.get(location, 1),
        'intensity_score':   fit_map.get(fitness_level, 1) + act_map.get(activity_level, 1) +
                             (1 if hours_per_week < 3 else (2 if hours_per_week < 6 else 3)),
        f'gender_{gender}': 1,
        f'goal_{goal}': 1,
        f'personality_{personality}': 1,
        f'location_{location}': 1,
        f'health_issue_{health_issue}': 1,
    }

    new_df = pd.DataFrame([new_row])
    for col in feature_cols:
        if col not in new_df.columns:
            new_df[col] = 0
    new_df = new_df[feature_cols]
    new_df[num_features] = scaler.transform(new_df[num_features])

    proba  = xgb_model.predict_proba(new_df)[0]
    scores = sorted([(le_sport.classes_[i], float(proba[i])) for i in range(len(proba))],
                    key=lambda x: -x[1])
    filtered = [(s, sc) for s, sc in scores if s not in avoid]

    recommendations = []
    for rank, (sport, score) in enumerate(filtered[:top_n], 1):
        meta = SPORT_META.get(sport, {'emoji': '🏆', 'category': 'Sport', 'intensity': 'Medium'})
        recommendations.append({
            'rank':      rank,
            'sport':     sport,
            'score':     round(score * 100, 2),
            'emoji':     meta['emoji'],
            'category':  meta['category'],
            'intensity': meta['intensity'],
            'preferred': sport in prefer,
        })

    return {
        'recommendations': recommendations,
        'health_assessment': {
            'bmi':          round(bmi, 1),
            'bmi_status':   bmi_s,
            'bmi_goal':     bmi_g,
            'age_group':    age_g,
            'age_intensity':age_i,
            'activity_status': act_s,
            'fitness_status':  fit_s,
            'health_score': health_sc,
            'avoid_sports': avoid,
            'prefer_sports': prefer,
        }
    }


# ──────────────────────────────────────────────
# Flask App
# ──────────────────────────────────────────────
app = Flask(__name__, static_folder='static')
CORS(app)


@app.route('/')
def index():
    return send_from_directory('static', 'index.html')


@app.route('/api/recommend', methods=['POST'])
def api_recommend():
    try:
        data = request.get_json()
        if not data:
            return jsonify({'error': 'No data provided'}), 400

        required = ['age', 'gender', 'weight_kg', 'height_cm',
                    'activity_level', 'fitness_level', 'goal',
                    'personality', 'location', 'hours_per_week', 'budget']
        missing = [f for f in required if f not in data]
        if missing:
            return jsonify({'error': f'Missing fields: {missing}'}), 400

        result = recommend_sport(data)
        return jsonify({'status': 'success', **result})

    except Exception as e:
        return jsonify({'error': str(e)}), 500


@app.route('/api/sports', methods=['GET'])
def api_sports():
    sports = [{'name': s, **SPORT_META.get(s, {'emoji': '🏆', 'category': 'Sport', 'intensity': 'Medium'})}
              for s in le_sport.classes_]
    return jsonify({'sports': sports, 'count': len(sports)})


@app.route('/api/health', methods=['GET'])
def api_health():
    return jsonify({'status': 'healthy', 'models': 'loaded', 'sports_count': len(le_sport.classes_)})


if __name__ == '__main__':
    print("🚀 Starting Sports Recommendation API...")
    print("   → API:  http://localhost:5000/api/recommend")
    print("   → UI:   http://localhost:5000/")
    app.run(debug=True, host='0.0.0.0', port=5000)
