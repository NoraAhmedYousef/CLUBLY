# Clubly — Backend

This branch contains the **backend** of Clubly, a smart club management platform that organizes and manages sports club operations through a centralized digital system.

## Overview

The backend handles all the business logic, data management, and communication between the client applications (Frontend, AI services) and the database. It's built to support multiple user roles — **Admin, Trainer, Member, and Guest** — each with their own permissions and workflows.

##  Tech Stack

- **ASP.NET Core Web API** — handles business logic and exposes API endpoints
- **Entity Framework Core** — ORM for database operations
- **RESTful APIs** — communication layer between client and server
- **SQL Server (SSMS)** — relational database management
- **Visual Studio** — development environment

##  Core Responsibilities

- **Authentication & Authorization** — sign in, register, and role-based access control (Admin / Trainer / Member / Guest)
- **Membership Management** — create, renew, and track memberships
- **Booking System** — automated activity/facility booking that prevents scheduling conflicts
- **Database Management** — centralized storage for users, memberships, activities, and reservations
- **Admin Operations** — endpoints supporting the admin dashboard (reporting, monitoring, decision-making tools)
- **AI Integration** — endpoints connecting the backend to the AI recommendation system, chatbot, and medical assistant
- **Data Validation** — ensures accurate and consistent data across the system
- **Repository Pattern** — abstracts data access logic through repositories and interfaces, keeping services decoupled from EF Core

## Project Structure

```
Clubly.Backend/
├── Controllers/        # API endpoints
├── Models/             # Entity/data models
├── Data/               # DbContext & database configuration
├── Repositories/       # Data access layer (implements Repository pattern)
├── Interfaces/         # Contracts for services & repositories
├── Services/           # Business logic layer
├── DTOs/               # Data transfer objects
├── Helpers/            # Utility/helper classes (mappers, extensions, etc.)
├── Migrations/         # EF Core migrations
└── Program.cs          # App entry point & configuration
```


