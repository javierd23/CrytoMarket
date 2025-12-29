# Crypto Market API — ASP.NET Core (.NET 8)

## Overview

This project is an **ASP.NET Core Web API built with .NET 8** that integrates with the **Binance public REST API** to retrieve, cache, persist, and expose cryptocurrency market data.

It is designed as a **monolithic application with clean architectural separation**, demonstrating:

- SOLID principles
- Asynchronous programming
- Parallel execution
- Cache-aside strategy
- EF Core persistence
- Testability with mocked external dependencies

---

## Business Scenario

The API acts as a backend service for a crypto analytics platform.  
It efficiently retrieves market data from Binance while minimizing external API calls and exposing normalized data through REST endpoints.

---

## Technology Stack

- **.NET 8**
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **SQLite**
- **IMemoryCache**
- **xUnit & Moq**

---

## Architecture

The solution follows a **clean monolithic architecture**, separating responsibilities logically within a single service:

CryptoMarket
│
├── Controllers        → API endpoints
├── Application
│   ├── Interfaces     → Abstractions (services & external APIs)
│   └── Services       → Business logic
├── Domain
│   └── Entities       → Core models
├── Infrastructure
│   ├── Binance        → External API integration
│   └── Persistence   → EF Core DbContext
└── CryptoMarket.Tests → Unit tests

## API Endpoints

GET /api/symbols
POST /api/symbols/refresh


### Prices
GET /api/prices?symbols=BTCUSDT,ETHUSDT

### Candles
GET /api/candles/{symbol}?interval=1m&limit=100

