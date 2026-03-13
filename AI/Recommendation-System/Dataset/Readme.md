# 🏅 Sports Recommendation Dataset

> A synthetic dataset of **66,000 users** designed for training and evaluating sports recommendation systems. Each record maps a user's demographic profile, fitness level, health conditions, personal preferences, and skill attributes to a recommended sport from a catalog of **44 sports**.

---

## 📌 Table of Contents

- [Overview](#overview)
- [Dataset Statistics](#dataset-statistics)
- [Features Description](#features-description)
- [Target Label](#target-label)
- [Sports Catalog](#sports-catalog)
- [Data Cleaning](#data-cleaning)
- [Business Rules & Constraints](#business-rules--constraints)
- [Sample Record](#sample-record)
- [Usage](#usage)

---

## Overview

| Property | Value |
|---|---|
| **Total Records** | 66,000 |
| **Total Features** | 34 columns |
| **Target Classes** | 44 sports |
| **File Format** | CSV |
| **File Name** | `sports_data_strict_v2.csv` |
| **Language** | English |
| **Source** | Synthetically generated |

The dataset was carefully designed to reflect **realistic demographic distributions** and **medically sound constraints**, making it suitable for building hybrid recommendation systems that combine Content-Based Filtering and Collaborative Filtering.

---

## Dataset Statistics

| Feature | Distribution |
|---|---|
| **Age Range** | 5 – 70 years |
| **Gender** | ~50% Female / ~50% Male |
| **Fitness Levels** | Beginner 22% / Intermediate 40% / Advanced 38% |
| **Activity Levels** | Sedentary 6% / Lightly Active 14% / Moderately Active 37% / Very Active 43% |
| **Health Issues** | None ~81% / Knee Pain ~8% / Back Pain ~6% / Heart Condition ~3% / Asthma ~2% |
| **Goals** | Have Fun / Compete Professionally / Build Muscle / Improve Endurance / Social & Meet People / Reduce Stress / Stay Healthy / Lose Weight |
| **Locations** | Urban ~34% / Suburban ~33% / Rural ~33% |
| **User Ratings** | 3.5 – 5.0 (mean ≈ 4.25) |

---

## Features Description

### 👤 Demographics

| Column | Type | Values | Description |
|---|---|---|---|
| `user_id` | string | U00000 – U65999 | Unique user identifier |
| `age` | int | 5 – 70 | User age in years |
| `gender` | string | Male, Female | Biological gender |
| `weight_kg` | float | 23.0 – 100.0 | Weight in kilograms |
| `height_cm` | float | 122.0 – 200.0 | Height in centimeters |
| `bmi` | float | 10.0 – 40.0 | Body Mass Index — calculated as `weight / (height/100)²` |

> **Note:** Height and weight ranges are age- and gender-appropriate following WHO pediatric and adult growth standards.

---

### 🏃 Fitness Profile

| Column | Type | Values | Description |
|---|---|---|---|
| `fitness_level` | string | Beginner, Intermediate, Advanced | Self-reported fitness level |
| `activity_level` | string | Sedentary, Lightly Active, Moderately Active, Very Active | Daily physical activity level |
| `hours_available_per_week` | float | 1.0 – 20.0 | Weekly hours available for sports |
| `monthly_budget_usd` | float | 10.0 – 300.0 | Monthly budget in USD for sports activities |

---

### 🎯 Goals & Preferences

| Column | Type | Values | Description |
|---|---|---|---|
| `goal` | string | Stay Healthy, Lose Weight, Build Muscle, Improve Endurance, Reduce Stress, Have Fun, Compete Professionally, Social / Meet People | Primary fitness goal |
| `personality` | string | Introvert, Ambivert, Extrovert | Social personality type |
| `location` | string | Urban, Suburban, Rural | Residential area type |
| `prefers_team_sport` | int | 0, 1 | Whether user prefers team sports |
| `prefers_outdoor` | int | 0, 1 | Whether user prefers outdoor sports |
| `prefers_contact_sport` | int | 0 – 5 | Preference for contact/combat sports (0 = none, 5 = very high) |

---

### 🧠 Skill & Aptitude Attributes

| Column | Type | Values | Description |
|---|---|---|---|
| `risk_tolerance` | int | 0 – 5 | Willingness to engage in risky or extreme activities |
| `competitive_level` | int | 0 – 5 | Desire to compete at a structured or professional level |
| `coordination_level` | int | 0 – 5 | Physical coordination and motor skill ability |
| `striking_preference` | int | 0 – 5 | Preference for striking-based combat sports (boxing, karate, etc.) |
| `grappling_preference` | int | 0 – 5 | Preference for grappling-based sports (judo, wrestling, BJJ, etc.) |
| `speed_agility` | int | 0 – 5 | Speed and agility level |
| `endurance_level` | int | 0 – 5 | Cardiovascular and stamina endurance |
| `preferred_distance` | int | 0 – 5 | Preference for long-distance or endurance-type sports |

> **Note:** All skill attributes use a 0–5 ordinal scale (0 = lowest, 5 = highest).

---

### 🏥 Health

| Column | Type | Values | Description |
|---|---|---|---|
| `health_issue` | string | None, Knee Pain, Back Pain, Heart Condition, Asthma | Known health condition that restricts certain sports |

---

### 🏆 Recommendation & Sport Properties

| Column | Type | Values | Description |
|---|---|---|---|
| `recommended_sport` | string | 44 classes | **Target label** — the recommended sport |
| `sport_difficulty` | string | Low, Medium, High | Physical difficulty level of the sport |
| `sport_type` | string | Individual, Team, Both | Whether sport is solo, team-based, or flexible |
| `sport_environment` | string | Indoor, Outdoor, Both | Where the sport is typically practiced |
| `sport_budget_level` | string | Low, Medium, High | Estimated monthly cost category |
| `calories_burned_per_hour` | int | 80 – 750 | Estimated calories burned per hour |

---

### ⭐ Interaction Signals

| Column | Type | Values | Description |
|---|---|---|---|
| `user_rating` | float | 3.5 – 5.0 | User rating for the recommended sport |
| `clicked` | int | 0, 1 | Whether user clicked / showed interest (1 if rating ≥ 3.0) |
| `practiced` | int | 0, 1 | Whether user practiced the sport (1 if rating ≥ 4.0) |

> **Note:** Ratings are calculated based on user-sport compatibility score, not random. A higher compatibility between user profile and sport properties results in a higher rating.

---

## Target Label

The `recommended_sport` column is the **target variable** with **44 unique classes**:

```
Swimming, Cycling, Chess, Squash, Yoga, Mind Sports / Board Games,
Tennis, Dance Sport, Bowling, Equestrian, Wrestling, Billiards,
Surfing, Parkour, Archery, Shooting, eSports, Athletics, Gym,
Basketball, Diving, Brazilian Jiu-Jitsu, Weightlifting, Taekwondo,
Triathlon, Football, Handball, Judo, Volleyball, Kick-boxing, Boxing,
Ballet, Karate, American Football, Hockey, Badminton, Bodybuilding,
Marathon Running, Multi Martial Arts, CrossFit, Gymnastics, Climbing,
Bungee Jumping, Kids Martial Arts
```

### Sport Properties Reference

| Sport | Difficulty | Type | Environment | Budget | Cal/hr |
|---|---|---|---|---|---|
| Gym | Medium | Individual | Indoor | Medium | 400 |
| Swimming | Medium | Individual | Indoor | Medium | 500 |
| CrossFit | High | Individual | Indoor | High | 700 |
| Triathlon | High | Individual | Outdoor | High | 750 |
| Marathon Running | High | Individual | Outdoor | Low | 700 |
| Football | High | Team | Outdoor | Low | 600 |
| Boxing | High | Individual | Indoor | Medium | 650 |
| Yoga | Low | Individual | Indoor | Low | 250 |
| Chess | Low | Individual | Indoor | Low | 100 |
| eSports | Low | Team | Indoor | Low | 80 |
| Badminton | Medium | Individual | Indoor | Low | 480 |
| Volleyball | Medium | Team | Both | Low | 450 |
| Basketball | High | Team | Outdoor | Low | 550 |
| Climbing | High | Individual | Outdoor | High | 550 |
| Bodybuilding | High | Individual | Indoor | Medium | 380 |
| Kick-boxing | High | Individual | Indoor | Medium | 680 |
| Multi Martial Arts | High | Individual | Indoor | Medium | 700 |
| Squash | High | Individual | Indoor | Medium | 700 |
| American Football | High | Team | Outdoor | High | 620 |
| Handball | High | Team | Indoor | Low | 580 |

*Full reference available in dataset.*

---

## Data Cleaning

The raw dataset went through comprehensive validation checks and cleaning to remove all logical inconsistencies:

### Age-Related Rules
| Rule | Status |
|---|---|
| Kids < 14 assigned dangerous sports (wrestling, boxing, parkour, etc.) | ✅ Fixed |
| Kids < 10 assigned High difficulty sports | ✅ Fixed |
| Kids < 10 with Heart Condition | ✅ Removed |
| Kids < 14 with Weightlifting / Bodybuilding | ✅ Fixed |
| Kids < 14 with unrealistic goals (Build Muscle, Compete Professionally) | ✅ Fixed |
| Kids < 14 with Advanced fitness level | ✅ Fixed |
| Kids < 10 with Very Active activity level | ✅ Fixed |

### Health-Related Rules
| Condition | Restricted Sports | Status |
|---|---|---|
| Knee Pain | Marathon, Football, Basketball, Squash, Taekwondo, Parkour, Kick-boxing | ✅ Fixed |
| Back Pain | Wrestling, Weightlifting, Gymnastics, CrossFit, Bodybuilding, Boxing | ✅ Fixed |
| Heart Condition | All High-difficulty sports | ✅ Fixed |
| Asthma | Marathon, Triathlon, CrossFit, Squash, Athletics | ✅ Fixed |

### Fitness & Activity Consistency
| Rule | Status |
|---|---|
| Sedentary + Advanced fitness level | ✅ Fixed → Intermediate |
| Very Active + Beginner fitness level | ✅ Fixed → Intermediate |
| Sedentary + > 10 hours/week | ✅ Fixed |
| Very Active + < 2 hours/week | ✅ Fixed |

### Preference Alignment
| Rule | Status |
|---|---|
| Introvert assigned Team sports | ✅ Fixed → Individual/Both |
| Extrovert assigned antisocial sports (Chess, eSports, Billiards) | ✅ Fixed → Team/Both |
| Prefers Outdoor but assigned Indoor-only sport | ✅ Fixed |
| Prefers Indoor but assigned Outdoor-only sport | ✅ Fixed |
| Prefers Team sport but assigned Individual sport | ✅ Fixed |

### Goal Alignment
| Rule | Status |
|---|---|
| Lose Weight + sport burning < 300 cal/hr | ✅ Fixed |
| Build Muscle + non-muscle sport (Chess, eSports, Billiards) | ✅ Fixed |

### Other
| Rule | Status |
|---|---|
| BMI recalculated from height/weight for all rows | ✅ Fixed |
| Budget < $30 assigned High-budget sports | ✅ Fixed |
| < 2 hrs/week assigned time-heavy sports (Triathlon, Marathon, CrossFit) | ✅ Fixed |
| Ratings recalculated based on user-sport compatibility | ✅ Fixed |

---

## Business Rules & Constraints

The dataset enforces the following domain-specific rules that any model trained on it should ideally learn:

```
IF age < 14 → No dangerous sports (wrestling, boxing, parkour, surfing, etc.)
IF age < 10 → Only Low difficulty sports

IF health_issue = Knee Pain    → No marathon, football, basketball, squash, taekwondo
IF health_issue = Back Pain    → No wrestling, weightlifting, crossfit, bodybuilding
IF health_issue = Heart        → No High difficulty sports
IF health_issue = Asthma       → No marathon, triathlon, crossfit, squash

IF personality = Introvert     → sport_type ∈ {Individual, Both}
IF personality = Extrovert     → sport_type ∈ {Team, Both}

IF prefers_outdoor = 1         → sport_environment ∈ {Outdoor, Both}
IF prefers_outdoor = 0         → sport_environment ∈ {Indoor, Both}

IF prefers_team_sport = 1      → sport_type ∈ {Team, Both}

IF goal = Lose Weight          → calories_burned_per_hour ≥ 300
IF goal = Build Muscle         → sport ∈ muscle-building category

IF activity_level = Sedentary  → fitness_level ≠ Advanced
IF activity_level = Very Active → fitness_level ≠ Beginner

IF risk_tolerance = 0          → No extreme/high-risk sports (Bungee Jumping, Parkour, etc.)
IF striking_preference > 3     → Prioritize striking sports (Boxing, Karate, Kick-boxing, etc.)
IF grappling_preference > 3    → Prioritize grappling sports (Judo, BJJ, Wrestling, etc.)
IF endurance_level < 2         → No long-distance sports (Marathon, Triathlon)
```

---

## Sample Record

```csv
age,gender,weight_kg,height_cm,bmi,activity_level,fitness_level,goal,personality,location,
hours_available_per_week,monthly_budget_usd,health_issue,prefers_team_sport,prefers_outdoor,
prefers_contact_sport,risk_tolerance,competitive_level,coordination_level,striking_preference,
grappling_preference,speed_agility,endurance_level,preferred_distance,recommended_sport,
sport_difficulty,sport_type,sport_environment,sport_budget_level,calories_burned_per_hour,
user_id,user_rating,clicked,practiced

25,Male,75.0,178.0,23.7,Moderately Active,Intermediate,Build Muscle,Introvert,Urban,
6.0,80.0,,0,0,2,2,3,3,1,2,3,2,1,Gym,Medium,Individual,Indoor,Medium,400,U00042,4.5,1,1
```

---

## Usage

### Load Dataset

```python
import pandas as pd

df = pd.read_csv('sports_data_strict_v2.csv', low_memory=False)
print(df.shape)        # (66000, 34)
print(df.dtypes)
print(df['recommended_sport'].nunique())  # 44
```

### Train / Test Split

```python
from sklearn.model_selection import train_test_split

X = df.drop(columns=['recommended_sport', 'user_id', 'bmi', 'sport_difficulty',
                      'sport_type', 'sport_environment', 'sport_budget_level',
                      'calories_burned_per_hour', 'user_rating', 'clicked', 'practiced'])
y = df['recommended_sport']

X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)
```

### Build User-Item Matrix (for Collaborative Filtering)

```python
user_item = df.pivot_table(
    index='user_id',
    columns='recommended_sport',
    values='user_rating',
    fill_value=0
)
print(user_item.shape)  # (66000, 44)
```

---

## Citation

If you use this dataset in your research or project, please cite:

```
Sports Recommendation Dataset v2 (2025)
Synthetically generated for graduation project purposes.
Features: 66,000 users × 44 sports × 34 attributes
```
