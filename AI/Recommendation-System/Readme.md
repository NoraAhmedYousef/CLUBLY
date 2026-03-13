# 🏆 Sports Recommendation System

> An AI-powered hybrid recommendation engine that suggests the most suitable sports for users based on their personal profile, fitness level, health conditions, preferences, and skill attributes.

---

## 📌 Table of Contents

- [Overview](#overview)
- [Architecture](#architecture)
- [Dataset](#dataset)
- [Models](#models)
- [API Endpoints](#api-endpoints)
- [UI](#ui)
- [Installation](#installation)
- [Project Structure](#project-structure)
- [Tech Stack](#tech-stack)

---

## Overview

The Sports Recommendation System is a graduation project that combines **Content-Based Filtering** and **Collaborative Filtering** in a hybrid approach to recommend the top-N most suitable sports for any user.

The system takes into account:

- **Demographics** — age, gender, weight, height, BMI
- **Fitness profile** — fitness level, activity level, weekly hours
- **Goals** — lose weight, build muscle, reduce stress, have fun, compete professionally, etc.
- **Health conditions** — knee pain, back pain, heart condition, asthma
- **Preferences** — indoor/outdoor, team/individual, contact sports, budget
- **Personality** — introvert, ambivert, extrovert
- **Location** — urban, suburban, rural
- **Skill & aptitude** — risk tolerance, coordination, striking/grappling preference, speed, endurance, competitive level

---

## Architecture

```
┌─────────────────────────────────────────────────────────┐
│                     Frontend (HTML/JS)                   │
│                   Recommendation UI                      │
└────────────────────────┬────────────────────────────────┘
                         │ HTTP (REST API)
┌────────────────────────▼────────────────────────────────┐
│                   Flask API (app.py)                     │
│                                                          │
│   /recommend        ──►  Content-Based Model (XGBoost)  │
│   /recommend/hybrid ──►  Hybrid Model (XGBoost + SVD)   │
└──────────┬──────────────────────┬───────────────────────┘
           │                      │
┌──────────▼──────────┐  ┌────────▼──────────────────────┐
│  Content-Based      │  │  Collaborative Filtering       │
│  ─────────────────  │  │  ─────────────────────────────│
│  XGBoost Classifier │  │  SVD Matrix Factorization      │
│  44 sport classes   │  │  KNN User Similarity           │
│  51 features        │  │  User-Item Interaction Matrix  │
│  Health Assessment  │  │  Engagement Score              │
└──────────┬──────────┘  └────────┬──────────────────────┘
           │                      │
           └──────────┬───────────┘
                      │  alpha blend (CB + CF)
              ┌───────▼────────┐
              │  Hybrid Score  │
              │  Top-N Sports  │
              └────────────────┘
```

### How it Works

1. **User submits their profile** via the UI form or API
2. **Health Assessment Layer** evaluates BMI, age, activity level, budget, and health issue — generating smart health-aware features
3. **Content-Based model (XGBoost)** predicts a probability score for all 44 sports based on 51 engineered user features
4. **Collaborative Filtering (SVD + KNN)** finds similar users via cosine similarity on SVD latent factors and aggregates their engagement scores
5. **Hybrid blending** combines both scores: `final = α × CB_score + (1-α) × CF_score`
6. **Top-N sports** are returned ranked by final score with confidence percentages

---

## Dataset

| Property | Value |
|---|---|
| Total Records | 66,000 users |
| Sports | 44 sports |
| Features | 34 columns (raw) → 51 input features (after engineering) |
| Source | Synthetic (custom generated) |

### Features

| Column | Type | Description |
|---|---|---|
| `user_id` | string | Unique user identifier |
| `age` | int | User age (5–70) |
| `gender` | string | Male / Female |
| `weight_kg` | float | Weight in kilograms |
| `height_cm` | float | Height in centimeters |
| `bmi` | float | Calculated BMI |
| `activity_level` | string | Sedentary / Lightly Active / Moderately Active / Very Active |
| `fitness_level` | string | Beginner / Intermediate / Advanced |
| `goal` | string | Stay Healthy / Lose Weight / Build Muscle / Improve Endurance / Reduce Stress / Have Fun / Compete Professionally / Social & Meet People |
| `personality` | string | Introvert / Ambivert / Extrovert |
| `location` | string | Urban / Suburban / Rural |
| `hours_available_per_week` | float | Weekly hours available for sport |
| `monthly_budget_usd` | float | Monthly budget in USD |
| `health_issue` | string | None / Knee Pain / Back Pain / Heart Condition / Asthma |
| `prefers_team_sport` | int | 0 or 1 |
| `prefers_outdoor` | int | 0 or 1 |
| `prefers_contact_sport` | int | 0–5 scale (preference for contact/combat sports) |
| `risk_tolerance` | int | 0–5 scale |
| `competitive_level` | int | 0–5 scale |
| `coordination_level` | int | 0–5 scale |
| `striking_preference` | int | 0–5 scale |
| `grappling_preference` | int | 0–5 scale |
| `speed_agility` | int | 0–5 scale |
| `endurance_level` | int | 0–5 scale |
| `preferred_distance` | int | 0–5 scale |
| `recommended_sport` | string | Target label (44 classes) |
| `sport_difficulty` | string | Low / Medium / High |
| `sport_type` | string | Individual / Team / Both |
| `sport_environment` | string | Indoor / Outdoor / Both |
| `sport_budget_level` | string | Low / Medium / High |
| `calories_burned_per_hour` | int | Estimated calories burned |
| `user_rating` | float | User rating 3.5–5.0 |
| `clicked` | int | Whether user clicked the recommendation |
| `practiced` | int | Whether user practiced the sport |

### Data Cleaning

The dataset went through **comprehensive cleaning** to fix logical inconsistencies:

- ✅ Kids < 14 assigned only age-appropriate safe sports
- ✅ Health conditions mapped to restricted sports (knee pain → no marathon, etc.)
- ✅ Sedentary users capped at realistic activity hours
- ✅ BMI recalculated from height/weight
- ✅ Introvert/extrovert personality aligned with sport type
- ✅ Outdoor preference aligned with sport environment
- ✅ Team preference aligned with sport type
- ✅ Budget constraints enforced per sport cost level
- ✅ Ratings recalculated based on user-sport compatibility
- ✅ Skill attributes (striking, grappling, endurance, etc.) validated per assigned sport

---

## Models

### 1. Health Assessment Layer

Before any ML prediction, the system runs a health assessment that converts raw inputs into smart numeric scores:

```python
assess_bmi(bmi)          → (status, goal, score)
assess_age(age)          → (group, intensity, score)
assess_activity(level)   → (status, advice, score)
assess_fitness(level)    → (status, advice, score)
assess_hours(hours)      → (status, advice, score)
assess_budget(budget)    → (status, advice, score)
assess_health_issue(issue) → (avoid_list, prefer_list)
compute_health_score(row)  → numeric score (0-100)
```

These derived features (`bmi_score`, `age_score`, `health_score`, `intensity_score`, interaction terms, etc.) are passed alongside the raw features to the ML model, boosting accuracy.

### 2. Content-Based Model — XGBoost (Best Model)

Four models were trained and compared. The best-performing was selected automatically:

| Model | Notes |
|---|---|
| XGBoost | Tuned with 700 estimators, depth 12, sample weights |
| Random Forest | 500 estimators, max depth 25, balanced class weights |
| Neural Network (MLP) | 4 hidden layers (512→256→128→64), early stopping |
| Voting Ensemble | Soft voting across all three models |

```python
XGBClassifier(
    n_estimators=700,
    max_depth=12,
    learning_rate=0.03,
    subsample=0.85,
    colsample_bytree=0.85,
    reg_alpha=0.1,
    reg_lambda=1.0,
    objective='multi:softprob',
    num_class=44
)
```

- **Input:** 51 engineered features (one-hot encoded + StandardScaler)
- **Output:** Probability distribution over 44 sport classes
- **Evaluation:** 5-Fold Stratified Cross-Validation

### 3. Collaborative Filtering — SVD + KNN

```
User-Item Matrix (66,000 × 44)  — engagement_score = rating×0.6 + clicked×0.8 + practiced×1.5
         ↓
   TruncatedSVD Decomposition
   (Latent Factors: 15)
         ↓
   KNN (k=10, cosine similarity)
         ↓
   Weighted Average of Similar Users' Engagement Scores
```

- Finds the 10 most similar users using cosine similarity on SVD latent factors
- Aggregates engagement scores weighted by similarity
- Falls back to pure Content-Based for new users not in training data

### 4. Hybrid Recommender

```python
final_score = alpha * cb_score + (1 - alpha) * cf_score
# alpha tuned automatically on test set (default ≈ 0.6)
```

- **Alpha tuning:** evaluated at α ∈ {0.3, 0.4, 0.5, 0.6, 0.65, 0.7, 0.8, 0.9, 1.0} using Hit@3
- Uses CF only when `user_id` exists in training data
- Falls back to pure Content-Based for new users

### Saved Model Files

```
models/
├── xgb_content_based.pkl      # XGBoost classifier (best content-based model)
├── scaler.pkl                 # StandardScaler (51 features)
├── label_encoder_sport.pkl    # LabelEncoder (44 classes)
├── hybrid_recommender.pkl     # Hybrid recommender object
├── svd_model.pkl              # TruncatedSVD (15 components)
├── knn_cf_model.pkl           # KNN (k=10, cosine)
├── user_item_matrix.csv       # User-item engagement matrix (66000 × 44)
├── user_factors.npy           # SVD latent user factors
└── feature_cols.csv           # 51 feature column names
```

---

## API Endpoints

### `POST /recommend`
Content-based recommendation for any user (new or existing).

**Request:**
```json
{
  "age": 25,
  "gender": "Male",
  "weight_kg": 75,
  "height_cm": 178,
  "activity_level": "Moderately Active",
  "fitness_level": "Intermediate",
  "goal": "Build Muscle",
  "personality": "Introvert",
  "location": "Urban",
  "hours_available_per_week": 6,
  "monthly_budget_usd": 80,
  "health_issue": "None",
  "prefers_team_sport": 0,
  "prefers_outdoor": 0,
  "prefers_contact_sport": 2,
  "risk_tolerance": 2,
  "competitive_level": 3,
  "coordination_level": 3,
  "striking_preference": 1,
  "grappling_preference": 2,
  "speed_agility": 3,
  "endurance_level": 3,
  "preferred_distance": 2,
  "top_n": 5
}
```

**Response:**
```json
{
  "status": "success",
  "recommendations": [
    {"rank": 1, "sport": "Gym",                 "confidence": 0.9991},
    {"rank": 2, "sport": "Brazilian Jiu-Jitsu", "confidence": 0.0004},
    {"rank": 3, "sport": "Bodybuilding",        "confidence": 0.0002}
  ],
  "user_profile": {
    "age": 25, "goal": "Build Muscle", "fitness_level": "Intermediate"
  }
}
```

### `POST /recommend/hybrid`
Hybrid recommendation — requires `user_id` from training data.

**Extra field:** `"user_id": "U00042"`

### `GET /sports`
Returns all 44 available sports.

### `GET /health`
API health check.

---

## UI

### Recommendation Page (`recommendation.html`)
- Compact form with all model inputs including the 9 new skill/aptitude sliders
- Toggle buttons for fitness level, activity, goal, personality
- Real-time results with confidence bars and medal ranking
- Sport detail panel with calories, difficulty, environment, budget info

---

## Installation

### Prerequisites
- Python 3.9+
- pip

### Steps

```bash
# 1. Clone the repository
git clone https://github.com/your-username/sports-recommendation.git
cd sports-recommendation

# 2. Install dependencies
pip install -r requirements.txt

# 3. Train the models (run the notebook first)
jupyter notebook sports_recommendation_96.ipynb

# 4. Run the API
python app.py

# 5. Open the UI
# http://localhost:5000/recommend-ui
```

---

## Project Structure

```
sports-recommendation/
│
├── 📓 sports_recommendation_96.ipynb   # Full ML pipeline notebook
│
├── 🐍 app.py                           # Main Flask API
├── 🐍 recommend_app.py                 # Recommendation API (mock, no ML needed)
│
├── 🌐 recommendation.html              # Recommendation UI
│
├── 📊 sports_data_strict_v2.csv        # Cleaned dataset (66,000 rows × 34 cols)
│
├── 📁 models/                          # Saved ML models (after training)
│   ├── xgb_content_based.pkl
│   ├── scaler.pkl
│   ├── label_encoder_sport.pkl
│   ├── hybrid_recommender.pkl
│   ├── svd_model.pkl
│   ├── knn_cf_model.pkl
│   ├── user_item_matrix.csv
│   ├── user_factors.npy
│   └── feature_cols.csv
│
├── 📄 requirements.txt
└── 📄 README.md
```

---

## Tech Stack

| Layer | Technology |
|---|---|
| **ML Models** | XGBoost, Scikit-learn (TruncatedSVD, KNN, RandomForest, MLP, StandardScaler, LabelEncoder) |
| **Backend** | Python, Flask, Flask-CORS |
| **Frontend** | HTML5, CSS3, Vanilla JavaScript |
| **Fonts** | Tajawal (Arabic), Bebas Neue |
| **Data** | Pandas, NumPy |
| **Notebook** | Jupyter |
| **Version Control** | Git, GitHub |

---

## Sports Catalog

The system covers **44 sports** across different categories:

| Category | Sports |
|---|---|
| **Combat** | Boxing, Kick-boxing, Karate, Taekwondo, Judo, Wrestling, Brazilian Jiu-Jitsu, Multi Martial Arts, Kids Martial Arts |
| **Water** | Swimming, Diving, Surfing, Triathlon |
| **Team** | Football, Basketball, Volleyball, Handball, Hockey, American Football |
| **Fitness** | Gym, CrossFit, Bodybuilding, Weightlifting, Gymnastics, Athletics, Marathon Running |
| **Racket** | Tennis, Squash, Badminton |
| **Mind** | Chess, Mind Sports / Board Games, eSports, Billiards |
| **Outdoor** | Cycling, Climbing, Parkour, Archery, Equestrian, Bungee Jumping |
| **Wellness** | Yoga, Dance Sport, Ballet |
| **Precision** | Shooting, Bowling |

---

## Health Restriction Rules

The system enforces safety rules for users with health conditions:

| Condition | Restricted Sports |
|---|---|
| **Knee Pain** | Marathon Running, Football, Basketball, Squash, Taekwondo, Parkour, Kick-boxing |
| **Back Pain** | Wrestling, Weightlifting, Gymnastics, CrossFit, Bodybuilding, Boxing |
| **Heart Condition** | All High-difficulty sports |
| **Asthma** | Marathon Running, Triathlon, CrossFit, Squash, Athletics |

---

## License

This project was developed as a graduation project. All rights reserved.
