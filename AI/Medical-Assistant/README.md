# 🏥 Clubly Medical Assistant

<div align="center">

![Python](https://img.shields.io/badge/Python-3.11-blue?style=for-the-badge&logo=python)
![Flask](https://img.shields.io/badge/Flask-3.0-black?style=for-the-badge&logo=flask)
![Groq](https://img.shields.io/badge/Groq-LLaMA%203.3%2070B-orange?style=for-the-badge)
![Docker](https://img.shields.io/badge/Docker-Ready-2496ED?style=for-the-badge&logo=docker)
![License](https://img.shields.io/badge/License-MIT-green?style=for-the-badge)

**مساعد طبي رياضي ذكي مدمج في منصة Clubly**  
*AI-powered Sports Medicine Assistant integrated into the Clubly platform*

</div>

---

## 📋 Overview

**Clubly Medical Assistant** is an AI-powered sports medicine chatbot built on top of the **Groq LLaMA 3.3 70B** model. It provides professional, evidence-based guidance on sports injuries, rehabilitation, nutrition, and medical conditions — tailored specifically for athletes and active individuals using the Clubly sports platform.

The assistant is context-aware: it pulls live data from the Clubly API (available activities and trainers) and incorporates it naturally into its responses, offering personalized recommendations based on what's actually available on the platform.

---

## ✨ Features

| Feature | Description |
|---|---|
| 🩺 **Sports Injury Guidance** | Assessment and first-aid for sprains, strains, fractures, concussions, and more |
| 🥗 **Sports Nutrition** | Macro calculations, supplement guidance, hydration strategies, and meal timing |
| ❤️ **Medical Conditions** | Safe exercise protocols for diabetes, hypertension, asthma, arthritis, and post-surgery recovery |
| 🔗 **Clubly Integration** | Live sync with Clubly API — recommends real activities and trainers available on the platform |
| 🌍 **Bilingual (AR / EN)** | Detects language automatically and responds 100% in Arabic or English accordingly |
| 🧠 **Session Memory** | Maintains per-session conversation history (up to 20 turns) for contextual follow-ups |
| 🚨 **Emergency Detection** | Flags life-threatening symptoms and directs users to emergency services immediately |
| 🐳 **Docker Ready** | Fully containerized for easy deployment anywhere |

---

## 🏗️ Architecture

```
┌─────────────────────────────────────────────────┐
│                  Client (Browser)                │
│              medical_widget.html                 │
└───────────────────┬─────────────────────────────┘
                    │ HTTP POST /medical-chat
                    ▼
┌─────────────────────────────────────────────────┐
│              Flask API  (app.py)                 │
│                                                  │
│  ┌─────────────┐     ┌──────────────────────┐   │
│  │  Session    │     │   Language Detector  │   │
│  │  Manager   │     │   (AR / EN)          │   │
│  └─────────────┘     └──────────────────────┘   │
│                                                  │
│  ┌──────────────────────────────────────────┐   │
│  │           Clubly API Fetcher             │   │
│  │    Activities  │  Trainers               │   │
│  └──────────────────────────────────────────┘   │
└───────────────────┬─────────────────────────────┘
                    │
          ┌─────────▼──────────┐
          │   Groq API         │
          │ LLaMA 3.3 70B      │
          └────────────────────┘
```

---

## 🚀 Getting Started

### Prerequisites

- Python 3.11+
- A [Groq API Key](https://console.groq.com/)
- (Optional) Docker

---

### 🐍 Local Setup

```bash
# 1. Clone the repository
git clone https://github.com/your-org/clubly-medical-assistant.git
cd clubly-medical-assistant

# 2. Install dependencies
pip install -r requirements.txt

# 3. Set environment variables
export GROQ_API_KEY="your_groq_api_key_here"
export CLUBLY_API_URL="http://clublywebsite.runasp.net/api"   # optional — has a default

# 4. Run the application
python app.py
```

The server starts at: `http://localhost:7860`  
Chat UI available at: `http://localhost:7860/chat`

---

### 🐳 Docker Setup

```bash
# Build the image
docker build -t clubly-medical-assistant .

# Run the container
docker run -d \
  -p 7860:7860 \
  -e GROQ_API_KEY="your_groq_api_key_here" \
  --name medical-assistant \
  clubly-medical-assistant
```

---

## 🔌 API Reference

### `POST /medical-chat`

Send a message and receive a medical response.

**Request Body:**
```json
{
  "message": "كيف أتعامل مع التواء الكاحل؟",
  "session_id": "user_12345"
}
```

**Response:**
```json
{
  "reply": "التواء الكاحل من أكثر الإصابات الرياضية شيوعاً...",
  "session_id": "user_12345"
}
```

---

### `POST /medical-chat/clear`

Clear the conversation history for a given session.

**Request Body:**
```json
{
  "session_id": "user_12345"
}
```

**Response:**
```json
{
  "status": "cleared"
}
```

---

### `GET /chat`

Serves the built-in chat widget UI (`medical_widget.html`).

---

### `GET /`

Health check — returns service status.

```json
{
  "service": "Clubly Medical Assistant",
  "status": "running"
}
```

---

## 📁 Project Structure

```
medical Assistant/
├── app.py                  # Main Flask application & API logic
├── medical_widget.html     # Frontend chat widget (served by Flask)
├── requirements.txt        # Python dependencies
├── Dockerfile              # Container configuration
└── README.md               # This file
```

---

## ⚙️ Environment Variables

| Variable | Required | Default | Description |
|---|---|---|---|
| `GROQ_API_KEY` | ✅ Yes | — | Your Groq API key |
| `CLUBLY_API_URL` | ❌ No | `http://clublywebsite.runasp.net/api` | Base URL for Clubly API |
| `PORT` | ❌ No | `7860` | Port the server listens on |

---

## 🧠 AI Model

| Property | Value |
|---|---|
| Provider | [Groq](https://groq.com/) |
| Model | `llama-3.3-70b-versatile` |
| Max Tokens | 700 per response |
| Temperature | 0.4 (precise, low randomness) |
| Session Memory | Last 20 turns per session |

---

## 🌍 Language Detection

The assistant automatically detects whether the user's message is in **Arabic** or **English** by analyzing character ratios, then responds entirely in that language — no mixing. If ≥ 40% of alphabetic characters are Arabic, the response is in Arabic (فصحى واضحة).

---

## 🛡️ Safety Guidelines

The assistant follows strict safety protocols:

- 🚨 **Emergency situations** (chest pain, loss of consciousness, severe injury) → instructs the user to contact emergency services immediately
- ⚠️ **Scope disclaimer** → always clarifies it provides guidance, not a diagnosis or prescription
- 💊 **No medication recommendations** → never suggests specific drugs or dosages
- 🔬 **Evidence-based** → references established guidelines (RICE protocol, ACSM, WHO) where applicable
- 🚫 **Out-of-scope deflection** → politely redirects non-medical questions

---

## 📦 Dependencies

```txt
flask>=3.0.0
flask-cors>=4.0.0
groq>=0.9.0
requests>=2.31.0
gunicorn>=21.2.0
```

---

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/your-feature`
3. Commit your changes: `git commit -m 'Add some feature'`
4. Push to the branch: `git push origin feature/your-feature`
5. Open a Pull Request

---

## ⚠️ Disclaimer

This assistant is designed to provide **general health and sports medicine guidance only**. It is **not a substitute for professional medical advice, diagnosis, or treatment**. Always consult a qualified physician or sports medicine doctor for any medical concerns.

---

## 📄 License

This project is licensed under the MIT License — see the [LICENSE](LICENSE) file for details.

---

<div align="center">
  Built with ❤️ for the <strong>Clubly</strong> sports platform
</div>
