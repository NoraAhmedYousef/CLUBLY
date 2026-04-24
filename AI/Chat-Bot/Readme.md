# 🏋️ Coach AI — Sports Chatbot

<div align="center">

![Python](https://img.shields.io/badge/Python-3.11+-blue?style=for-the-badge&logo=python)
![Flask](https://img.shields.io/badge/Flask-2.3+-black?style=for-the-badge&logo=flask)
![Groq](https://img.shields.io/badge/Groq-LLaMA%203.3%2070B-orange?style=for-the-badge)
![Bilingual](https://img.shields.io/badge/Language-AR%20%7C%20EN-purple?style=for-the-badge)
![License](https://img.shields.io/badge/License-MIT-green?style=for-the-badge)

**An AI-powered sports coaching chatbot built with Flask & Groq**  
*Covers 40+ sports — responds in Arabic & English*

</div>

---

## 📋 Overview

**Coach AI** is an intelligent sports assistant chatbot powered by **Groq's LLaMA 3.3 70B** model. It provides friendly, expert-level guidance across **40+ sports disciplines** — from Football and Basketball to Chess, eSports, and Parkour.

The chatbot comes with a polished, embeddable floating-widget UI (`coach_widget.html`) that can be dropped into any website, and a clean REST API backend built with Flask. It automatically detects whether the user is writing in Arabic or English and responds in kind.

---

## ✨ Features

| Feature | Description |
|---|---|
| 🤖 **AI Sports Coach** | Powered by LLaMA 3.3 70B via Groq for fast, accurate responses |
| 🏅 **40+ Sports Covered** | Football, Swimming, Gym, Chess, eSports, Boxing, Yoga, and more |
| 🌍 **Bilingual (AR / EN)** | Auto-detects language and responds in Arabic or English |
| 💬 **Session Memory** | Remembers up to 20 turns per session for contextual conversations |
| 🎨 **Embeddable Widget** | Floating chat bubble UI — paste into any webpage in minutes |
| 🔌 **Simple REST API** | Clean endpoints for easy integration with any frontend or app |
| ⚡ **Fast Inference** | Groq's ultra-low-latency API ensures near-instant replies |

---

## 🏅 Supported Sports

<details>
<summary>Click to expand the full list (40+ sports)</summary>

| Category | Sports |
|---|---|
| **Combat Sports** | Boxing, Kickboxing, Karate, Taekwondo, Judo, Brazilian Jiu-Jitsu, Wrestling, Multi Martial Arts, Kids Martial Arts |
| **Team Sports** | Football, Basketball, Volleyball, Handball, Hockey, American Football |
| **Water Sports** | Swimming, Surfing, Diving |
| **Racket & Precision** | Tennis, Squash, Badminton, Billiards, Bowling, Archery, Shooting |
| **Fitness & Training** | Gym, Bodybuilding, CrossFit, Weightlifting, Gymnastics, Ballet, Dance Sport |
| **Mind & Strategy** | Chess, Mind Sports / Board Games, eSports |
| **Endurance & Adventure** | Athletics, Marathon Running, Triathlon, Cycling, Climbing, Parkour, Bungee Jumping |
| **Other** | Yoga, Equestrian |

</details>

The assistant covers questions such as:
- Benefits of each sport and how to get started
- Required equipment and estimated costs
- Difficulty level and age suitability
- Training programs and workout plans
- Common injuries and how to prevent them
- Side-by-side sport comparisons

---

## 🗂️ Project Structure

```
Chat-Bot/
├── chatbot_app.py        # Flask backend & API routes
├── coach_widget.html     # Embeddable floating chat widget (UI)
├── requirements.txt      # Python dependencies
└── README.md             # This file
```

---

## 🚀 Getting Started

### Prerequisites

- Python 3.8+
- A [Groq API Key](https://console.groq.com/)

---

### 🐍 Installation

```bash
# 1. Clone the repository
git clone https://github.com/your-org/coach-ai-chatbot.git
cd coach-ai-chatbot

# 2. Install dependencies
pip install -r requirements.txt

# 3. Create a .env file
echo "GROQ_API_KEY=your_groq_api_key_here" > .env

# 4. Run the server
python chatbot_app.py
```

The server starts at: `http://localhost:5000`  
Chat UI available at: `http://localhost:5000/chat`

---

### 🌐 Embedding the Widget

The `coach_widget.html` is designed to be embedded in any existing website. Just copy the relevant sections into your project:

```html
<!-- 1. Add the CSS from coach_widget.html into your stylesheet -->
<!-- 2. Add the HTML div#coach-widget into your page body -->
<!-- 3. Add the JS from coach_widget.html into your scripts -->

<!-- Point the API URL to your deployed backend -->
<script>
  const API_URL = "https://your-deployed-backend.com";
</script>
```

The widget renders as a **floating action button** in the bottom corner of your page, opening a full chat panel on click.

---

## 🔌 API Reference

### `POST /sports-chat`

Send a message and receive a sports coaching response.

**Request Body:**
```json
{
  "message": "What are the benefits of swimming?",
  "session_id": "user_abc123"
}
```

**Response:**
```json
{
  "reply": "Swimming is a full-body low-impact exercise that...",
  "session_id": "user_abc123"
}
```

---

### `POST /sports-chat/clear`

Clear the conversation history for a given session.

**Request Body:**
```json
{
  "session_id": "user_abc123"
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

Serves the built-in chat widget UI (`coach_widget.html`).

---

### `GET /`

Health check — returns service status.

```json
{
  "service": "Coach AI",
  "status": "running"
}
```

---

## ⚙️ Configuration

Create a `.env` file in the project root:

```env
GROQ_API_KEY=your_groq_api_key_here
```

| Variable | Required | Description |
|---|---|---|
| `GROQ_API_KEY` | ✅ Yes | Your API key from [console.groq.com](https://console.groq.com/) |

---

## 🧠 AI Model

| Property | Value |
|---|---|
| Provider | [Groq](https://groq.com/) |
| Model | `llama-3.3-70b-versatile` |
| Max Tokens | 1024 per response |
| Temperature | 0.7 (balanced creativity & accuracy) |
| Session Memory | Last 20 messages per session |

---

## 📦 Dependencies

```txt
flask>=2.3.0
flask-cors>=4.0.0
groq>=0.9.0
python-dotenv>=1.0.0
```

Install with:
```bash
pip install -r requirements.txt
```

---

## 🚢 Deployment

### Heroku / Railway / Render

```bash
# Use gunicorn for production
pip install gunicorn
gunicorn chatbot_app:app --bind 0.0.0.0:$PORT --workers 2
```

### Docker (optional)

```dockerfile
FROM python:3.11-slim
WORKDIR /app
COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt gunicorn
COPY . .
EXPOSE 5000
CMD ["gunicorn", "chatbot_app:app", "--bind", "0.0.0.0:5000", "--workers", "2"]
```

```bash
docker build -t coach-ai .
docker run -p 5000:5000 -e GROQ_API_KEY=your_key coach-ai
```

---

## 🤝 Contributing

1. Fork the repository
2. Create your feature branch: `git checkout -b feature/amazing-feature`
3. Commit your changes: `git commit -m 'Add amazing feature'`
4. Push to the branch: `git push origin feature/amazing-feature`
5. Open a Pull Request

---

## 📄 License

This project is licensed under the MIT License — see the [LICENSE](LICENSE) file for details.

---

<div align="center">
  Built with ❤️ for sports enthusiasts everywhere — <strong>Coach AI</strong>
</div>
