# AbarrotesCloud - Backend API ☁️🛒

A robust, scalable RESTful API built with **.NET 8 (C#)**, serving as the core backend for the AbarrotesCloud SaaS Point of Sale (POS) system. 

## 🚀 Tech Stack
* **Framework:** .NET 8 / ASP.NET Core Web API
* **Language:** C#
* **Database:** PostgreSQL (Supabase) via Entity Framework Core
* **Deployment:** Microsoft Azure App Services (Linux)
* **Architecture:** Decoupled Architecture, REST API, DTO Pattern.

## ✨ Key Technical Achievements
* **Secure Data Handling:** Implementation of the DTO (Data Transfer Object) pattern to encapsulate data and protect the database schema.
* **Cloud-Native Database Connection:** Configured with IPv4/IPv6 Session Poolers to ensure stable, secure cloud-to-cloud communication between Azure and Supabase.
* **Cross-Origin Resource Sharing (CORS):** Fully configured to seamlessly accept and process decoupled requests from a Vercel-hosted React frontend.
* **Production Ready:** Environment variables and connection strings securely managed via Azure configuration, maintaining a clean `.gitignore` for repository security.

## 🛠️ Local Development Setup

1. Clone the repository:
```bash
git clone [https://github.com/aron-alv/abarrotes-cloud-backend.git](https://github.com/aron-alv/abarrotes-cloud-backend.git)
