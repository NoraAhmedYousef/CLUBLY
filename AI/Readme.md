# 🤖 Clubly AI — Sports Intelligence Suite

<div align="center">

![Python](https://img.shields.io/badge/Python-3.11+-blue?style=for-the-badge&logo=python)
![Flask](https://img.shields.io/badge/Flask-3.0-black?style=for-the-badge&logo=flask)
![Groq](https://img.shields.io/badge/Groq-LLaMA%203.3%2070B-orange?style=for-the-badge)
![XGBoost](https://img.shields.io/badge/XGBoost-2.0-red?style=for-the-badge)
![scikit-learn](https://img.shields.io/badge/scikit--learn-1.3-F7931E?style=for-the-badge&logo=scikit-learn)
![Docker](https://img.shields.io/badge/Docker-Ready-2496ED?style=for-the-badge&logo=docker)

**A unified AI suite for the Clubly sports platform**  
*Three intelligent services — one seamless sports experience*

</div>

---

## 📋 Overview

**Clubly AI** is a monorepo housing three production-ready AI-powered services built for the Clubly sports platform. Together, they form a complete intelligent layer that guides users from discovering the right sport, to learning how to train, to staying safe and healthy while doing it.

| Service | Description |
|---|---|
| 🏆 [Recommendation System](#-recommendation-system) | Hybrid ML engine that recommends the best sports for each user |
| 🏋️ [Sports Chatbot (Coach AI)](#️-sports-chatbot--coach-ai) | AI coaching assistant for training, equipment, and sport advice |
| 🏥 [Medical Assistant](#-medical-assistant) | Sports medicine advisor for injuries, nutrition, and health conditions |

---

## 🗂️ Repository Structure

```
AI/
├── Recommendation-System/          # Hybrid ML recommendation engine
│   ├── app.py                      # Flask API
│   ├── static/
│   │   └── index.html              # Web UI
│   ├── models/                     # Pre-trained ML models
│   │   ├── hybrid_recommender.pkl
│   │   ├── xgb_content_based.pkl
│   │   ├── knn_cf_model.pkl
│   │   ├── svd_model.pkl
│   │   ├── user_factors.npy
│   │   ├── user_item_matrix.csv
│   │   ├── label_encoder_sport.pkl
│   │   ├── feature_cols.csv
│   │   └── scaler.pkl
│   ├── Dataset/
│   │   ├── sports_recommendation_dataset_v2.csv   # 66K user records
│   │   └── Readme.md
│   ├── Visualization/              # EDA & model evaluation charts
│   ├── sports_recommendation_96.ipynb             # Training notebook
│   └── requirements.txt
│
├── Chat-Bot/                       # Coach AI sports chatbot
│   ├── chatbot_app.py              # Flask API
│   ├── coach_widget.html           # Embeddable chat widget
│   └── requirements.txt
│
├── Medical-Assistant/              # Clubly Medical Assistant
│   ├── app.py                      # Flask API
│   ├── medical_widget.html         # Embeddable chat widget
│   ├── Dockerfile
│   └── requirements.txt
│
└── README.md
```

---

## 🏆 Recommendation System

A **hybrid machine learning** recommendation engine that analyzes a user's demographics, fitness level, health conditions, personality, and lifestyle — then recommends the most suitable sports from a catalog of 44 disciplines.

### How It Works

The system combines two complementary ML approaches:

```
User Profile Input
        │
        ▼
┌───────────────────────────────────┐
│  Content-Based Filtering          │
│  XGBoost Classifier               │
│  (31 engineered features)         │
└────────────────┬──────────────────┘
                 │
┌────────────────▼──────────────────┐
│  Collaborative Filtering          │
│  SVD + KNN                        │
│  (user-item matrix, 66K users)    │
└────────────────┬──────────────────┘
                 │
┌────────────────▼──────────────────┐
│  Hybrid Recommender               │
│  Health-aware filtering           │
│  (avoids contraindicated sports)  │
└────────────────┬──────────────────┘
                 │
                 ▼
        Top-N Sport Recommendations
        + Health Assessment Report
```

### Dataset

| Property | Value |
|---|---|
| Total Records | **66,000 users** |
| Total Features | 34 columns |
| Target Classes | **44 sports** |
| Age Range | 5 – 70 years |
| Gender Split | ~50% Female / ~50% Male |
| Health Issues Covered | None, Knee Pain, Back Pain, Heart Condition, Asthma |

### ML Models

| Model | Purpose |
|---|---|
| `xgb_content_based.pkl` | XGBoost classifier for content-based sport scoring |
| `knn_cf_model.pkl` | KNN collaborative filtering model |
| `svd_model.pkl` | SVD for matrix factorization |
| `user_factors.npy` | Latent user factor matrix |
| `hybrid_recommender.pkl` | Final ensemble model |
| `scaler.pkl` | StandardScaler for numerical features |
| `label_encoder_sport.pkl` | Sport label encoder (44 classes) |

### Health Intelligence

The engine includes a built-in health assessment layer that:
- Computes **BMI** and assigns fitness goals
- Evaluates **age group** suitability for sport intensity
- Filters out **contraindicated sports** based on health conditions
- Promotes **medically preferred** sports for specific conditions

| Condition | Avoided Sports | Preferred Sports |
|---|---|---|
| Knee Pain | Running, Basketball, Football, Volleyball, Squash | Swimming, Cycling, Yoga, Chess |
| Back Pain | Weightlifting, Gymnastics | Swimming, Yoga, Cycling |
| Asthma | Marathon, Triathlon, CrossFit | Swimming, Yoga, Chess, Archery |
| Heart Condition | Boxing, CrossFit, Triathlon, Weightlifting | Yoga, Swimming, Chess, Cycling |

### Visualizations

The `Visualization/` folder includes full EDA and model evaluation charts:

| Chart | Description |
|---|---|
| `eda_demographics.png` | Age, gender, and location distributions |
| `eda_sports_dist.png` | Sport frequency across the dataset |
| `eda_ratings.png` | User rating distributions |
| `eda_correlation.png` | Feature correlation heatmap |
| `feature_importance.png` | XGBoost top feature importances |
| `model_comparison.png` | Accuracy comparison across models |
| `confusion_matrix.png` | Multi-class classification confusion matrix |
| `cross_validation.png` | K-fold cross-validation scores |
| `svd_variance.png` | SVD explained variance curve |

### API

#### `POST /api/recommend`

Returns top-N sport recommendations with a health assessment.

**Request Body:**
```json
{
  "age": 25,
  "gender": "Male",
  "weight_kg": 75,
  "height_cm": 175,
  "activity_level": "Moderately Active",
  "fitness_level": "Intermediate",
  "goal": "Build Muscle",
  "personality": "Competitive",
  "location": "Urban",
  "hours_per_week": 4,
  "budget": 50,
  "health_issue": "None",
  "prefers_team": 1,
  "prefers_outdoor": 0,
  "top_n": 5
}
```

**Response:**
```json
{
  "status": "success",
  "recommendations": [
    {
      "rank": 1,
      "sport": "CrossFit",
      "score": 87.4,
      "emoji": "🏋️",
      "category": "Fitness",
      "intensity": "Very High",
      "preferred": false
    }
  ],
  "health_assessment": {
    "bmi": 24.5,
    "bmi_status": "normal",
    "age_group": "young",
    "health_score": 85,
    "avoid_sports": [],
    "prefer_sports": []
  }
}
```

#### `GET /api/sports`
Returns the full catalog of 44 supported sports with metadata.

#### `GET /api/health`
Health check — confirms all models are loaded.

### Running the Recommendation System

```bash
cd Recommendation-System
pip install -r requirements.txt
python app.py
# → UI:  http://localhost:5000/
# → API: http://localhost:5000/api/recommend
```

---

## 🏋️ Sports Chatbot — Coach AI

An AI-powered coaching assistant that answers questions about **40+ sports** — from training tips and equipment costs to injury prevention and sport comparisons. Powered by **Groq LLaMA 3.3 70B** and comes with an embeddable floating chat widget.

### Features

- 🤖 Conversational AI with session memory (up to 20 turns)
- 🌍 Auto-detects Arabic or English and responds accordingly
- 🎨 Embeddable floating widget for any webpage
- 🚫 Politely declines out-of-scope questions

### API

#### `POST /sports-chat`
```json
// Request
{ "message": "What equipment do I need for boxing?", "session_id": "user_123" }

// Response
{ "reply": "For boxing you'll need...", "session_id": "user_123" }
```

#### `POST /sports-chat/clear`
Clears the conversation history for a session.

### Running Coach AI

```bash
cd Chat-Bot
pip install -r requirements.txt
echo "GROQ_API_KEY=your_key" > .env
python chatbot_app.py
# → http://localhost:5000/chat
```

---

## 🏥 Medical Assistant

A specialized **sports medicine chatbot** that provides professional, evidence-based guidance on injuries, sports nutrition, and safe exercise for various medical conditions. Integrates with the Clubly API to recommend real activities and trainers available on the platform.

### Specializations

- 🩺 **Sports Injuries** — RICE protocol, recovery timelines, rehab exercises
- 🥗 **Sports Nutrition** — macro calculations, supplement guidance, hydration
- ❤️ **Medical Conditions** — safe exercise for diabetes, hypertension, asthma, post-surgery
- 🔗 **Clubly Integration** — fetches live activities and trainers from the platform

### Safety Protocols

- 🚨 Detects emergency symptoms → directs to emergency services immediately
- ⚠️ Always clarifies guidance vs. diagnosis
- 💊 Never recommends specific medications or dosages
- 🔬 References RICE, ACSM, and WHO guidelines where applicable

### API

#### `POST /medical-chat`
```json
// Request
{ "message": "I sprained my ankle during football, what should I do?", "session_id": "user_456" }

// Response
{ "reply": "For an ankle sprain, apply the RICE protocol immediately...", "session_id": "user_456" }
```

#### `POST /medical-chat/clear`
Clears session history.

### Running the Medical Assistant

```bash
cd Medical-Assistant
pip install -r requirements.txt
export GROQ_API_KEY="your_key"
python app.py
# → http://localhost:7860/chat
```

#### Docker

```bash
cd Medical-Assistant
docker build -t clubly-medical .
docker run -p 7860:7860 -e GROQ_API_KEY=your_key clubly-medical
```

---

## ⚙️ Environment Variables

| Variable | Used By | Required | Description |
|---|---|---|---|
| `GROQ_API_KEY` | Chat-Bot, Medical-Assistant | ✅ | Groq API key from [console.groq.com](https://console.groq.com/) |
| `CLUBLY_API_URL` | Medical-Assistant | ❌ | Clubly API base URL (has default) |
| `PORT` | Medical-Assistant | ❌ | Server port (default: 7860) |

---

## 📦 Dependencies Summary

| Service | Key Dependencies |
|---|---|
| Recommendation System | `flask`, `xgboost==2.0.3`, `scikit-learn==1.3.2`, `pandas`, `numpy`, `joblib` |
| Coach AI | `flask`, `groq`, `flask-cors`, `python-dotenv` |
| Medical Assistant | `flask`, `groq`, `flask-cors`, `requests` |

---

## 🧠 AI Models Summary

| Service | Model | Provider | Temp | Max Tokens |
|---|---|---|---|---|
| Coach AI | `llama-3.3-70b-versatile` | Groq | 0.7 | 1024 |
| Medical Assistant | `llama-3.3-70b-versatile` | Groq | 0.4 | 700 |
| Recommendation | XGBoost + SVD + KNN | scikit-learn / XGBoost | — | — |

---

## 🚀 Quick Start (All Services)

```bash
# Clone the repository
git clone https://github.com/your-org/clubly-ai.git
cd clubly-ai

# Terminal 1 — Recommendation System
cd Recommendation-System && pip install -r requirements.txt && python app.py

# Terminal 2 — Coach AI
cd Chat-Bot && pip install -r requirements.txt && python chatbot_app.py

# Terminal 3 — Medical Assistant
cd Medical-Assistant && pip install -r requirements.txt && python app.py
```

| Service | Port | URL |
|---|---|---|
| Recommendation System | 5000 | http://localhost:5000 |
| Coach AI | 5000 | http://localhost:5000/chat |
| Medical Assistant | 7860 | http://localhost:7860/chat |

> **Note:** Coach AI and Recommendation System both default to port 5000. Run them on separate ports using the `PORT` environment variable if deploying together.

---

## 🤝 Contributing

1. Fork the repository
2. Create your feature branch: `git checkout -b feature/your-feature`
3. Commit your changes: `git commit -m 'Add your feature'`
4. Push to the branch: `git push origin feature/your-feature`
5. Open a Pull Request

---

## 📄 License

This project is licensed under the MIT License — see the [LICENSE](LICENSE) file for details.

---

<div align="center">
  Built with ❤️ for the <strong>Clubly</strong> sports platform
</div>
