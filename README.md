# NusaFx Currency Conversion Service

## 📖 Overview

NusaFx is a .NET 8 Web API service that provides currency conversion functionality.
It integrates **ExchangeRate‑API** for live rates, **Redis** for caching, and optionally compares results with **AI providers** (OpenAI or Google Gemini) to highlight differences between authoritative financial data and AI‑generated estimates.

---

## ⚙️ Approach

### 🔹 [Live Rate Integration](ca://s?q=ExchangeRate_API)

- Uses ExchangeRate‑API as the authoritative source for conversion rates.
- Responses are cached in Redis for 1 hour to reduce API calls and improve performance.
- Ensures consistent, reliable, and up‑to‑date currency data.

### 🔹 [Caching with Redis](ca://s?q=dotnet_Redis_cache)

- Rates stored under normalized keys (`rate:USD:MYR`).
- Redis provides persistence across app restarts and supports distributed scaling.
- If Redis is unavailable, the service returns a **Failure** result to ensure transparency.

### 🔹 [Error Handling](ca://s?q=dotnet_error_handling)

Robust error handling for:

- API downtime or error responses.
- Invalid currency codes (e.g., `XYZ`).
- Redis unavailability (treated as a hard failure).

### 🔹 [AI Comparison](ca://s?q=AI_currency_rate_comparison)

- Optional endpoint (`/convert-with-ai`) calls Gemini or OpenAI to request an AI‑generated exchange rate.
- Compares the AI rate with the live rate, calculates the difference, and returns both.
- Demonstrates the gap between **real‑time financial APIs** and **AI estimates**, educating users about when AI is suitable (explanations, trends) versus when live APIs are required (transactions).

---

## 📄 Why This Approach

- **Reliability**: Live API + Redis caching ensures accurate and performant conversions.
- **Transparency**: Error handling makes failures explicit instead of silent.
- **Educational Value**: AI comparison highlights the limitations of LLMs in financial contexts.
- **Scalability**: Redis enables distributed caching, making the service production‑ready.

---

## 🚀 Endpoints

- `GET /convert` → Convert currency using live API + Redis cache.
- `GET /convert-with-ai` → Convert currency and compare live rate with AI‑generated rate.

---

## 🏗️ Architecture Diagram

```mermaid
flowchart LR
    Client -->|Request| CurrencyController
    CurrencyController -->|Check| Redis[(Redis Cache)]
    CurrencyController -->|Fetch| ExchangeRateAPI[ExchangeRate-API]
    CurrencyController -->|Optional| AIProvider[Gemini / OpenAI]
    Redis --> CurrencyController
    ExchangeRateAPI --> CurrencyController
    AIProvider --> CurrencyController
    CurrencyController -->|Response| Client
```

---

### 📸 Snapshot

## 📸 Snapshots

To make the project easier to understand, here are some screenshots of the running application and API:

### 🔹 Swagger UI Endpoints

![Swagger UI Endpoints](src/snapshoot/Screenshot%202026-08-08%20at%2009-54-27%20Swagger%20UI.png)

### 🔹 Conversion with AI Rate

![Conversion AI Rate](src/snapshoot/ConvertWithAi.png)

### 🔹 Conversion (Project Structure)

![Conversion](src/snapshoot/Conversion.png)

---

### 🔑 Notes

- **WebApi** → Presentation layer (controllers, startup, configs).
- **Application** → Business logic, services, DTOs, middleware.
- **Infrastructure** → Persistence, repositories.

This format is clean, consistent, and matches GitHub/Markdown best practices.

---
