Team Members:
1. Abdul Muqeet Ahmed
2. Mohammad Asim Ali
3. Mohammed Matheen Baba
4. Ruman saiyed


Video Demo:
https://www.youtube.com/watch?v=PiyZZmvpYRA

Design Document, API Endpoints List and Contributions are also uploaded in this directory for refernce.


# PollakLibrary API

**PollakLibrary** is a robust, production-ready RESTful Web API designed to handle modern library management needs. Built using **ASP.NET Core 8.0** and **Entity Framework Core**, it provides a streamlined interface for managing books, library members, and the lifecycle of borrowing transactions.

The system is designed with a focus on high availability, data integrity (via atomic operations to prevent "double borrow" concurrency issues), and developer productivity.

## 🚀 Features

- **Book Management**: Full CRUD operations for managing the library's physical inventory.
- **Member Management**: Registration and profile management for library members.
- **Borrowing System**: Concurrency-safe book checkout and return processing.
- **Data Integrity**: Atomic database updates ensure accurate inventory counts even under high concurrent load.
- **Performance Optimized**: Implements an in-memory caching strategy for high-traffic read operations.
- **Robust Error Handling**: Standardized RFC 7807 problem details responses for predictable client-side error handling.
- **Swagger Integration**: Interactive API documentation and testing out-of-the-box.

## 🛠️ Technology Stack

- **Language**: C# 12.0
- **Framework**: ASP.NET Core 8.0 (Web API)
- **ORM**: Entity Framework Core 8.0
- **Database**: SQLite (Portable, fast, no external server required)
- **Architecture**: N-Tier / Clean Architecture principles (Controllers, Services, Repositories)

## 📦 Prerequisites

To run this project, you will need:
- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) or later.
- An IDE such as Visual Studio 2022, JetBrains Rider, or VS Code.

## 🏃 Getting Started

1. **Clone the repository:**
   ```bash
   git clone <repository_url>
   cd PollakLibrary
   ```

2. **Run the Application:**
   You can run the application directly using the .NET CLI. The SQLite database runs in app directory and EF Core is configured to automatically create the database on startup if it doesn't already exist.
   ```bash
   dotnet run
   ```

3. **View the Swagger Documentation:**
   Once the application is running, open your browser and navigate to:
   - `http://localhost:5236/swagger` (or the port specified in your console output)
   This will provide an interactive UI to test all the API endpoints.

## 🔗 API Endpoints Overview

The API follows RESTful conventions.

### Books
- `GET /api/books` - Retrieve all books (Cached)
- `GET /api/books/{id}` - Retrieve a specific book
- `POST /api/books` - Add a new book
- `PUT /api/books/{id}` - Update a book
- `DELETE /api/books/{id}` - Delete a book

### Members
- `GET /api/members` - Retrieve all members
- `GET /api/members/{id}` - Retrieve a specific member
- `POST /api/members` - Register a new member
- `PUT /api/members/{id}` - Update member details
- `DELETE /api/members/{id}` - Delete a member

### Borrow Records
- `POST /api/borrow-records/borrow` - Checkout a book (decrements stock)
- `PUT /api/borrow-records/return/{id}` - Return a book (increments stock)
- `GET /api/borrow-records` - View all borrow history
- `GET /api/borrow-records/member/{id}` - View borrow history for a specific member

## 🏗️ Architecture

The project implements a decoupled **N-Tier Layered Architecture**:
1. **API Layer (Controllers)**: Handles HTTP routing, input parsing (DTOs), and standardized responses.
2. **Service Layer (Business Logic)**: Implements all library rules, validation, and handles the in-memory caching.
3. **Repository Layer (Data Access)**: Encapsulates all direct Entity Framework Core queries.
4. **Data Layer (DbContext)**: Manages SQLite connections and entity mappings.

This structure ensures that the system is easily testable, highly maintainable, and open to future expansion.
