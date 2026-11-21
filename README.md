# 🏋️ Power Fitness — Gym Management System

A web-based **Gym Management System** built with **ASP.NET Core MVC**, **Entity Framework Core**, and **Microsoft SQL Server**. The application provides a centralized platform for managing gym members, trainers, training plans, memberships, scheduled sessions, bookings, and attendance.

The solution follows a **Three-Tier Architecture**, separating the MVC presentation layer, business services, and data-access implementation into dedicated projects. It also uses Repository and Unit of Work patterns, Dependency Injection, AutoMapper, ASP.NET Core Identity, and EF Core Code-First migrations.

## 🌐 Live Deployment

**[Open Power Fitness — Live Website](https://power-fitnesss.runasp.net)**

The application is published and hosted on **MonsterASP.NET**. The deployed ASP.NET Core MVC application connects to its production SQL Server database, while member profile photos are stored through the application's attachment service.

## 🖥️ Homepage Preview

![Power Fitness live homepage preview](https://image.thum.io/get/width/1600/crop/1000/noanimate/maxAge/0/https://power-fitnesss.runasp.net/?readme-capture=20261121-1246)

> This preview requests a refreshed screenshot of the currently deployed homepage, so the dashboard reflects the latest data available from the live application.

## ✨ Project Highlights

- 🏗️ **Three-Tier Architecture:** Separates Presentation, Business Logic, and Data Access responsibilities.
- 👥 **Member Management:** Registers members, maintains personal information, profile photos, addresses, and health records.
- 🏋️ **Trainer Management:** Maintains trainer profiles and specialties, and validates trainer eligibility for session categories.
- 💳 **Plan Management:** Manages membership plans, durations, prices, and activation status.
- 🪪 **Membership Management:** Assigns plans to members, calculates expiration dates, and prevents overlapping active memberships.
- 📅 **Session Scheduling:** Creates and manages training sessions with a trainer, category, capacity, and date/time range.
- 🎟️ **Session Booking:** Books members into future sessions while checking membership validity, duplicate bookings, and available capacity.
- ✅ **Attendance Tracking:** Displays members booked into ongoing sessions and records attendance.
- 📊 **Dashboard Analytics:** Summarizes members, trainers, active memberships, and session statuses.
- 🔐 **Authentication & Authorization:** Uses ASP.NET Core Identity with `SuperAdmin` and `Admin` roles.
- 📎 **Attachment Service:** Validates, stores, retrieves, and deletes member profile photos.
- 🗄️ **EF Core Code-First:** Uses Fluent API configurations, SQL Server constraints, migrations, and initial data seeding.
- 🕒 **Egypt Time-Zone Handling:** Accepts session times in Egypt local time, stores them in UTC, and converts them back for display.

## 🧭 System Overview

The application is organized around the following functional modules:

| Module | Description |
| --- | --- |
| **Dashboard** | Displays live operational statistics, including total members, trainers, active memberships, and upcoming, ongoing, and completed sessions. |
| **Members** | Registers and manages member profiles, contact information, addresses, profile photos, and health records. |
| **Trainers** | Manages trainer information, contact details, addresses, and training specialties. |
| **Plans** | Lists available plans, displays plan details, updates plan information, and activates or deactivates plans. |
| **Memberships** | Assigns a plan to a member, calculates the membership end date, lists active memberships, and supports cancellation. |
| **Sessions** | Creates, lists, views, edits, and deletes scheduled training sessions. |
| **Bookings & Attendance** | Shows upcoming and ongoing sessions, manages member bookings, and records attendance. |
| **Authentication** | Handles staff login/logout and applies role-based access rules. |

## 🏗️ Architecture

The solution is divided into three projects. Each project has a focused responsibility and references the layer below it.

### 🖥️ Presentation Layer — `GymManagementSystem`

The ASP.NET Core MVC web application is responsible for handling HTTP requests and presenting information to users.

It contains:

- MVC Controllers and action methods.
- Razor Views and shared layouts.
- View rendering, form submission, and model validation.
- Application startup and middleware configuration.
- Dependency Injection registration.
- Authentication and authorization middleware.
- Static frontend assets such as CSS, JavaScript, images, and Bootstrap libraries.

Controllers delegate application operations to Business Logic services instead of implementing database queries and business rules directly.

### ⚙️ Business Logic Layer — `GymManagementSystem.BLL`

The Business Logic Layer coordinates application workflows and enforces the rules of the gym domain.

It contains:

- Services for Members, Trainers, Plans, Sessions, Memberships, Bookings, and Dashboard analytics.
- Service interfaces that define the operations exposed to the Presentation Layer.
- ViewModels used by MVC forms and pages.
- AutoMapper profiles for mapping between entities and ViewModels.
- The shared `Result` / `Result<T>` types for returning success and failure information.
- The reusable Attachment Service for member photo operations.
- Egypt time-zone conversion helpers for session scheduling.

### 🗄️ Data Access Layer — `GymManagementSystem.DAL`

The Data Access Layer is responsible for persistence and communication with SQL Server through Entity Framework Core.

It contains:

- Domain entities and enums.
- `GymDbContext`, which also integrates ASP.NET Core Identity.
- Fluent API entity configurations and database constraints.
- Generic and specialized repository implementations.
- Unit of Work implementation.
- EF Core Code-First migrations.
- Initial data seeders for gym data and Identity roles/users.

### 🔄 Request and Data Flow

```text
                       Browser
                          │
                          ▼
             Presentation Layer (MVC)
            Controllers • Razor Views
                          │
                          ▼
             Business Logic Layer (BLL)
       Services • ViewModels • AutoMapper
                Result Pattern • Rules
                          │
                          ▼
              Data Access Layer (DAL)
       Repositories • Unit of Work • EF Core
                          │
                          ▼
                    SQL Server
```

Dependency Injection wires the interfaces to their implementations at application startup. The Presentation Layer invokes service contracts, services coordinate repositories through the Unit of Work, and EF Core persists the resulting changes.

## 🧩 Design Patterns & Implementation Practices

| Pattern / Practice | How It Is Used |
| --- | --- |
| **Three-Tier Architecture** | Separates UI, business operations, and persistence into three projects. |
| **Repository Pattern** | Encapsulates common EF Core data-access operations behind repository interfaces. |
| **Generic Repository** | Provides reusable CRUD, lookup, filtering, existence, and count operations for entities. |
| **Specialized Repositories** | Adds query-specific operations for Sessions, Memberships, and Bookings. |
| **Unit of Work** | Coordinates repositories that share the same `GymDbContext` and centralizes `SaveChangesAsync`. |
| **Dependency Injection** | Registers services, repositories, Identity, and other dependencies through the built-in .NET container. |
| **AutoMapper** | Defines reusable mappings between database entities and MVC ViewModels. |
| **Result Pattern** | Returns success, not-found, validation, and failure results from business services. |
| **Fluent API** | Defines property constraints, relationships, indexes, defaults, and check constraints. |

## 🧱 Main Modules & Business Rules

### 👥 Member Management

Member records contain personal and contact information, date of birth, gender, an owned address, a profile photo, and a related health record.

- Email addresses and phone numbers must be unique.
- Phone numbers are validated against the supported Egyptian mobile prefixes: `010`, `011`, `012`, and `015`, followed by eight digits.
- A health record is collected during member registration.
- Profile photos are required by the current registration ViewModel and are validated by the Attachment Service. Supported extensions are `.jpg`, `.jpeg`, and `.png`, with a maximum file size of **5 MB**.
- The join date is populated automatically.
- A member cannot be deleted while they have an active membership or a future session booking.
- Deleting a member also triggers deletion of the associated stored photo.

### 🏋️ Trainer Management

Trainers share common personal information with members through the abstract `GymUser` base class and have a trainer specialty.

- Email and phone numbers must be unique.
- Each trainer has a specialty represented by `TrainerSpecialties`.
- Hire date is populated automatically.
- A trainer cannot be deleted while assigned to a future session.
- A trainer's specialty must match the category of a session they are assigned to teach.

Supported specialties/categories include `Cardio`, `Strength`, `Yoga`, `Boxing`, and `CrossFit`.

### 💳 Plan Management

Plans define the membership options offered by the gym.

- Each plan has a name, description, price, duration, and active status.
- Plan duration is constrained to **1–365 days**.
- Prices are stored as `decimal(10,2)`.
- Plans are deactivated through the `IsActive` flag rather than permanently deleted.
- A plan cannot be edited or deactivated while it has active memberships.
- Inactive plans cannot be assigned to new memberships.
- The initial plan catalog is loaded from `wwwroot/Files/plans.json` when the database has no plans.

### 📅 Session Management & Scheduling

A session represents a scheduled training class and includes a description, capacity, start and end times, trainer, and category.

- Session capacity must be between **1 and 25** participants.
- The end date/time must be later than the start date/time.
- A newly created session must start in the future.
- A valid trainer and category must be assigned.
- The trainer's specialty must match the session category.
- A session's status is computed from its schedule:
  - `Upcoming` — the start time has not been reached.
  - `Ongoing` — the current time is between the start and end times.
  - `Completed` — the end time has passed.
- Sessions cannot be deleted before they have ended, and sessions with bookings cannot be deleted.
- Sessions that have started or have bookings cannot be edited.

#### Egypt Local Time and UTC Storage

The session forms use `datetime-local` inputs, which submit a local date/time without a time-zone offset. The application interprets these values as **Egypt local time**, converts them to **UTC** before storing them, and converts UTC values back to Egypt local time when displaying sessions or populating the edit form.

This keeps time comparisons consistent on the server while preserving the expected local schedule for users. The time-zone helper uses the Windows time-zone identifier on Windows hosting and the IANA identifier on Linux environments.

> **Existing database note:** `DatabaseScripts/ConvertExistingSessionDatesToUtc.sql` is a one-time conversion script for session records that were previously stored as Egypt local wall-clock times. Back up the database before using it, and never execute it more than once against the same records. A fresh database does not need this conversion.

### 🪪 Membership Management

A membership connects a member to a plan for a defined period.

- The selected member and plan must exist.
- Only active plans can be assigned.
- A member cannot have more than one active membership at the same time.
- The membership end date is calculated using the plan duration.
- Membership status is computed from its end date: `Active` while the end date is in the future; otherwise `Expired`.
- Cancellation removes the member's active membership record.

### 🎟️ Booking & Attendance

Bookings form the relationship between members and sessions.

- A member must have an active membership to book a session.
- Only sessions that have not started can be booked.
- A member cannot book the same session more than once.
- A booking is rejected when the session has reached its capacity.
- A booking can only be cancelled before the session starts.
- New bookings are created with `IsAttended = false`.
- The ongoing-session workflow displays booked members and provides the attendance-recording action.

### 📊 Dashboard Analytics

The dashboard calculates its figures from the database, including:

- Total members.
- Total trainers.
- Active memberships.
- Upcoming sessions.
- Ongoing sessions.
- Completed sessions.

The session and membership analytics use UTC-based date comparisons, consistent with the application's session time-zone strategy.

## 🔐 Authentication & Authorization

Authentication is implemented using **ASP.NET Core Identity** with cookie-based sign-in, backed by the same EF Core database context used by the gym data.

The application defines two staff roles:

| Role | Access |
| --- | --- |
| `SuperAdmin` | Can access the Member and Trainer controllers, which explicitly require this role, along with areas protected by general authentication. |
| `Admin` | Can access areas protected by general authentication, but cannot access controllers that explicitly require the `SuperAdmin` role. |

The Identity integration uses `ApplicationUser`, `UserManager`, `SignInManager`, and `RoleManager`. Initial roles and staff accounts are provisioned by `IdentityDataSeeder`. The Home controller is explicitly configured for anonymous access; operational controllers apply their own authorization attributes.

### Demo Account Credentials


| Role | Email | Password |
| --- | --- | --- |
| `SuperAdmin` | `abdelrahmankishk@gmail.com` | `P@ssw0rd` |
| `Admin` | `youssef@gmail.com` | `P@ssw0rd` |


## 🗃️ Database Model

The data model is built around the gym's core entities and their relationships.

### Main Entities

| Entity | Responsibility |
| --- | --- |
| `GymUser` | Abstract base class for shared member and trainer properties. |
| `Member` | Represents a gym member and connects to health records, memberships, and bookings. |
| `Trainer` | Represents a trainer and their specialty; trainers can conduct multiple sessions. |
| `Address` | Owned value object containing building number, street, and city. |
| `HealthRecord` | Stores a member's height, weight, blood type, and notes. |
| `Plan` | Defines a membership plan, its duration, price, and active status. |
| `MemberShip` | Junction entity connecting a member with a plan and storing the membership end date. |
| `Category` | Defines a training-session category. |
| `Session` | Stores scheduled class information and its trainer/category relationships. |
| `Booking` | Junction entity connecting a member with a session and storing attendance state. |
| `ApplicationUser` | ASP.NET Core Identity user with first and last name fields. |

### Database Configuration

Entity configurations use EF Core Fluent API to define:

- Property lengths and SQL data types.
- Unique indexes for member/trainer email and phone values.
- Egyptian phone and email format check constraints.
- Plan duration and price precision.
- Session capacity and start/end date constraints.
- Relationships between members, health records, trainers, categories, sessions, memberships, and bookings.
- Database-generated default dates where configured.

Categories are seeded through `CategoryConfiguration`. Initial plans are seeded from JSON, and Identity roles/users are initialized through the Identity seeder.

## 🗂️ Project Structure

```text
Power-Fitness/
│
├── GymManagementSolution.sln
│
├── GymManagementSystem/                  # Presentation Layer (ASP.NET Core MVC)
│   ├── Controllers/
│   ├── MembersPhotos/
│   ├── Views/
│   │   ├── Account/
│   │   ├── Booking/
│   │   ├── Home/
│   │   ├── Members/
│   │   ├── Membership/
│   │   ├── Sessions/
│   │   ├── Trainer/
│   │   └── plans/
│   ├── wwwroot/
│   │   ├── css/
│   │   ├── images/
│   │   ├── js/
│   │   └── Files/plans.json
│   ├── Program.cs
│   └── ProgramExtensions.cs
│
├── GymManagementSystem.BLL/               # Business Logic Layer
│   ├── Common/
│   │   ├── EgyptTimeZone.cs
│   │   ├── Result.cs
│   │   └── ResultKind.cs
│   ├── Services/
│   │   ├── Attachment/
│   │   ├── Classes/
│   │   └── Interfaces/
│   ├── ViewModels/
│   └── MappingProfile.cs
│
├── GymManagementSystem.DAL/               # Data Access Layer
│   ├── Configurations/
│   ├── DataSeeder/
│   ├── DbContexts/
│   ├── Migrations/
│   ├── Models/
│   └── Repositories/
│       ├── Classes/
│       └── Interfaces/
│
└── DatabaseScripts/
    └── ConvertExistingSessionDatesToUtc.sql
```

## 🛠️ Technologies & Tools

| Technology | Purpose |
| --- | --- |
| **C# / .NET 9** | Main development platform and runtime target. |
| **ASP.NET Core MVC** | Server-side web application, routing, controllers, and Razor Views. |
| **Entity Framework Core 9** | ORM, LINQ queries, relationships, migrations, and persistence. |
| **Microsoft SQL Server** | Relational database. |
| **ASP.NET Core Identity** | Staff accounts, password hashing, roles, and cookie authentication. |
| **AutoMapper 17** | Entity and ViewModel mapping. |
| **Bootstrap 5** | Responsive layout and UI components. |
| **Bootstrap Icons** | Interface icons. |
| **HTML5 / CSS3 / JavaScript** | View markup, custom page styling, animations, and frontend interactions. |
| **Git & GitHub** | Source control and project repository. |
| **MonsterASP.NET** | Application hosting and deployment. |

## 🚀 Getting Started

### Prerequisites

- **.NET 9 SDK**
- **Microsoft SQL Server** (local or remote instance)
- Visual Studio 2022 or another .NET-compatible IDE
- Git

### 1. Clone the Repository

```bash
git clone https://github.com/Abdelrahmankishk/Power-Fitness.git
cd Power-Fitness
```

### 2. Configure the Database Connection

Configure the `DefaultConnection` connection string using **User Secrets** or an environment variable. Do not commit production connection strings, usernames, or passwords to source control.

For example, in PowerShell:

```powershell
$env:ConnectionStrings__DefaultConnection = "Server=.;Database=PowerFitnessDb;Trusted_Connection=True;TrustServerCertificate=True"
```

For SQL Server authentication, provide your own database username and password through a secure local configuration or hosting environment variable.

### 3. Restore Dependencies

```bash
dotnet restore GymManagementSolution.sln
```

### 4. Configure Local Identity Accounts

Before the first run, review `GymManagementSystem.DAL/DataSeeder/IdentityDataSeeder.cs` and configure your own local test accounts. Do not reuse production credentials or publish privileged account passwords.

### 5. Run the Application

```bash
dotnet run --project GymManagementSystem/GymManagementSystem.PL.csproj
```

At startup, the application checks for pending EF Core migrations, applies them, and runs the gym and Identity seeders. Make sure the configured SQL Server account has the required database permissions for this initialization.

Open the local URL displayed in the terminal after the application starts.

## 🚀 Deployment

The production application is hosted on **MonsterASP.NET**:

**[https://power-fitnesss.runasp.net](https://power-fitnesss.runasp.net)**


The SQL conversion script is located at `DatabaseScripts/ConvertExistingSessionDatesToUtc.sql`.
---

**Project:** Power Fitness — Gym Management System  
**Architecture:** Three-Tier (Presentation / Business Logic / Data Access)  
**Framework:** ASP.NET Core MVC on .NET 9  
**Deployment:** MonsterASP.NET
