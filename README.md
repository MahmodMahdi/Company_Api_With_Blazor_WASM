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
Author: Mahmoud Amin بنفس البيانات دى محتاج حاجة بشكل افضل ل readme

تمام 😎، ممكن نعيد صياغة الـ README بشكل أكثر تنظيماً، احترافية وسهولة في القراءة، مع تقسيم واضح للـ sections وBullet points واضحة، مع إبراز النقاط المهمة باستخدام Markdown حديث. هنا اقتراح كامل:

🏢 Company Management System (.NET 10)
📌 Overview

Company Management System هو حل Full-Stack modular مبني على .NET 10، يتضمن RESTful Web API و Blazor WebAssembly (WASM) للواجهة الأمامية.
تم تصميمه باستخدام Repository Pattern على مستوى الـ backend والـ frontend لضمان كود منفصل، قابل للاختبار، وسهل الصيانة.

🏗 Architecture
🔹 1. Company.Shared (Contract Layer)

الـ "Source of Truth" المشترك بين الـ API وBlazor:

AuthModels: DTOs لتسجيل الدخول والتسجيل.

Models: الكيانات الأساسية (Employee, Department).

Responses: ملفات standard wrappers (GeneralResponse.cs, PagedResult.cs) لضمان توافق الردود على الـ API.

🔹 2. Company_Management_System (Backend API)

خلفية عالية الأداء مسؤولة عن data persistence و security:

Controllers: 3 Controllers مخصصة (Account, Departments, Employees).

Data Access: EF Core مع Repository Pattern منظم.

Service Layer: encapsulates business logic (مثل AccountService).

Mapping: Extension Methods لتحويل Entity ↔ DTO بسرعة عالية.

Authentication: ASP.NET Core Identity مع JWT Bearer Tokens.

🔹 3. MyCompany.BlazorUI (Frontend)

واجهة حديثة تعمل كـ SPA على المتصفح باستخدام WebAssembly:

Client Repositories: HttpClient abstraction لعزل الـ UI عن الـ API endpoints.

Authentication: Custom AuthStateProvider لإدارة JWT sessions.

Components: منظمة حسب الميزات (Account, Departments, Employees).

🔄 Request Lifecycle

التدفق بين الـ client و الـ API باستخدام الـ shared models:

Blazor Client → Shared Models → API Controller → Service Layer → Repository → Database → Response → Blazor Client

🛠 Tech Stack & Patterns

Runtime: .NET 10

Backend: ASP.NET Core Web API

Frontend: Blazor WebAssembly

Database: MS SQL Server + EF Core

Security: JWT Authentication & ASP.NET Core Identity

Patterns:

Repository Pattern (Backend + Frontend)

DTO (Data Transfer Object) Pattern

Extension-based Mapping (High Performance)

Dependency Injection

🚀 Key Features

✅ Decoupled Design: واجهة المستخدم والـ API منفصلين تمامًا.

✅ Shared Contracts: مشاركة Models/DTOs بدون تكرار.

✅ JWT Security: اتصال آمن مع صلاحيات role-based.

✅ Manual Mapping: أداء عالي باستخدام Extension Methods.

✅ Clean UI: Razor Components modular لكل CRUD operation.

📬 Contact

Author: Mahmoud Amin
Email: MahmoudElmahdy555@gmail.com
