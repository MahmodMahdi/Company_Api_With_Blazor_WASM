🏢 Company Management System (.NET 10)
📌 Overview
Company Management System is a modular, full-stack solution built with .NET 10. It features a clean separation between a RESTful Web API and a Blazor WebAssembly (WASM) client. The project is designed with the Repository Pattern on both ends to ensure a decoupled, testable, and maintainable codebase.

🏗 Architecture Overview

🔹 1. Company.Shared (The Contract Layer)
The "Source of Truth" shared by both the API and Blazor.

AuthModels: DTOs for Login and Registration.

Models: Core entities (Employee, Department).

Responses: Standardized wrappers (GeneralResponse.cs, PagedResult.cs) for consistent API communication.

🔹 2. Company_Management_System (The API)
A high-performance backend responsible for data persistence and security.

Controllers: 3 Dedicated Controllers (Account, Departments, Employees).

Data Access: Uses EF Core with a structured Repository Pattern.

Service Layer: Encapsulates business logic (e.g., AccountService).

Mapping: Specialized Extension Methods for lightning-fast Entity-to-DTO conversion.

Authentication: ASP.NET Core Identity with JWT Bearer tokens.

3. MyCompany.BlazorUI (The Frontend)
A modern SPA (Single Page Application) running in the browser via WebAssembly.

Client Repositories: Abstracted HttpClient calls to decouple UI from API endpoints.

Authentication: Custom AuthStateProvider for persistent JWT session management.

Components: Organized by feature (Account, Departments, Employees).

🔄 Request Lifecycle (Integration Flow)
This diagram shows how the Blazor client interacts with the API using the Shared models:

🛠 Tech Stack & Patterns
Runtime: .NET 10

Backend: ASP.NET Core Web API

Frontend: Blazor WebAssembly

Database: MS SQL Server + EF Core

Security: JWT Authentication & ASP.NET Core Identity

Patterns: * Repository Pattern (Dual-sided)

DTO (Data Transfer Object) Pattern

Extension-based Mapping (High Performance)

Dependency Injection

🚀 Key Features
✅ Decoupled Design: API and UI are completely independent.

✅ Shared Contracts: Zero-redundancy by sharing Models/DTOs.

✅ JWT Security: Secure communication with role-based access.

✅ Manual Mapping: Optimized performance using Extension Methods instead of reflection.

✅ Clean UI: Modular Razor components for CRUD operations.

📬 Contact
Author: Mahmoud Amin
