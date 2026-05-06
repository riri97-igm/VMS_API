# VMS API — Visitor Management System (Backend)

A RESTful API built with **ASP.NET Core 8** and **Entity Framework Core** for managing visitors, appointments, staff, and check-in/check-out logs.

---

## Tech Stack

| Layer | Technology |
|-------|-----------|
| Framework | ASP.NET Core 8 (.NET 8) |
| ORM | Entity Framework Core 8 |
| Database | SQL Server (SQL Express) |
| Authentication | JWT Bearer Tokens |
| Password Hashing | HMACSHA512 |
| API Docs | Swagger / OpenAPI |
| Email | SMTP (Gmail) |
| Architecture | Clean Architecture (6 projects) |

---

## Project Structure

```
VMS.API/
├── VMS.API/               # Controllers, Services, Program.cs
├── VMS.Common/            # Enums (AppointmentStatus, AuditAction)
├── VMS.Converter/         # Entity ↔ DTO converters
├── VMS.DataAccess/        # Repository interfaces & implementations
├── VMS.EntityFramework/   # DbContext, Entities, Migrations, Seeder
└── VMS.Model/             # DTOs (Data Transfer Objects)
```

---

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8)
- SQL Server or SQL Server Express
- Visual Studio 2022 or VS Code

---

## Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/riri97-igm/VMS_API.git
cd VMS_API
```

### 2. Configure the database connection

Open `VMS.API/appsettings.json` and update the connection string:

```json
"ConnectionStrings": {
  "DbVMSConnectionString": "Server=YOUR_SERVER\\SQLEXPRESS;Database=DbVMS;User ID=sa;Password=YOUR_PASSWORD;Integrated Security=False;TrustServerCertificate=True;"
}
```

### 3. Configure JWT settings

```json
"Jwt": {
  "Key": "YOUR-SECRET-KEY-MIN-32-CHARACTERS",
  "Issuer": "VMS.API",
  "Audience": "VMS.UI"
}
```

### 4. Run database migrations

```bash
dotnet ef database update --project VMS.EntityFramework --startup-project VMS.API
```

### 5. Run the API

```bash
dotnet run --project VMS.API
```

The API will start on `http://localhost:5235`.  
Swagger UI is available at `http://localhost:5235/swagger`.

---

## Default Accounts (auto-seeded on first run)

| Role | Email | Password |
|------|-------|----------|
| Admin | `admin@vms.com` | `Admin@123` |
| Receptionist | `sarah@vms.com` | `Reception@123` |
| Staff | `alice@vms.com` | `Staff@123` |

> Accounts are seeded automatically when the Staffs table is empty.

---

## API Endpoints

### Authentication
| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| POST | `/api/Auth/login` | None | Login and get JWT token |
| POST | `/api/Auth/register` | Admin | Register new staff account |
| POST | `/api/Auth/change-password` | Any | Change own password |
| GET | `/api/Auth/me` | Any | Get current user info |

### Departments
| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/api/Department` | Any | Get all departments |
| GET | `/api/Department/{id}` | Any | Get department by ID |
| POST | `/api/Department` | Admin | Create department |
| PUT | `/api/Department/{id}` | Admin | Update department |
| DELETE | `/api/Department/{id}` | Admin | Delete department |

### Staff
| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/api/Staff` | Any | Get all staff |
| GET | `/api/Staff/{id}` | Any | Get staff by ID |
| POST | `/api/Staff` | Admin | Create staff |
| PUT | `/api/Staff/{id}` | Admin | Update staff |
| DELETE | `/api/Staff/{id}` | Admin | Delete staff |

### Roles
| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/api/Role` | Any | Get all roles |
| POST | `/api/Role` | Admin | Create role |
| PUT | `/api/Role/{id}` | Admin | Update role |
| DELETE | `/api/Role/{id}` | Admin | Delete role |

### Visitors
| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/api/Visitor` | Any | Get all visitors |
| GET | `/api/Visitor/{id}` | Any | Get visitor by ID |
| POST | `/api/Visitor` | Any | Register visitor |
| PUT | `/api/Visitor/{id}` | Any | Update visitor |
| DELETE | `/api/Visitor/{id}` | Any | Delete visitor |

### Appointments
| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/api/Appointment` | Any | Get all appointments |
| GET | `/api/Appointment/{id}` | Any | Get appointment by ID |
| GET | `/api/Appointment/stats` | Any | Get dashboard stats |
| POST | `/api/Appointment` | Any | Create appointment |
| PUT | `/api/Appointment/{id}` | Any | Update appointment (triggers email on status change) |
| DELETE | `/api/Appointment/{id}` | Any | Delete appointment |

### Visitor Log
| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/api/VisitorLog` | Any | Get all logs |
| GET | `/api/VisitorLog/today` | Any | Get today's logs |
| GET | `/api/VisitorLog/{id}` | Any | Get log by ID |
| GET | `/api/VisitorLog/visitor/{visitorId}` | Any | Get logs by visitor |
| POST | `/api/VisitorLog/checkin` | Any | Check in visitor (triggers email to host) |
| PUT | `/api/VisitorLog/{id}/checkout` | Any | Check out visitor |
| DELETE | `/api/VisitorLog/{id}` | Any | Delete log entry |

---

## Using Swagger with JWT

1. Open `http://localhost:5235/swagger`
2. Call `POST /api/Auth/login` with your credentials
3. Copy the `token` from the response
4. Click the **Authorize** button (🔒) at the top right
5. Enter: `Bearer YOUR_TOKEN_HERE`
6. Click **Authorize** — all requests will now include your token

---

## Email Notifications

The system sends automatic emails in two situations:

- **Visitor Check-in** — notifies the host staff when their visitor arrives
- **Appointment Approved/Rejected** — notifies the visitor of the decision

To enable email, configure `appsettings.json`:

```json
"Email": {
  "SmtpHost": "smtp.gmail.com",
  "SmtpPort": "587",
  "Username": "your-email@gmail.com",
  "Password": "your-gmail-app-password",
  "FromAddress": "your-email@gmail.com",
  "FromName": "VMS System"
}
```

> For Gmail, use an **App Password** (not your regular password). Enable 2FA first, then generate one at https://myaccount.google.com/apppasswords

---

## Role-Based Access

| Role | Permissions |
|------|------------|
| **Admin** | Full access — manage staff, departments, roles, visitors, appointments, logs |
| **Receptionist** | Manage visitors, appointments, check-in/check-out |
| **Staff** | View dashboard only |

---

## Database Schema

```
Departments ──< Staffs >── Roles
                  │
Visitors ────< Appointments
    │               │
    └────────< VisitorLogs >┘
```
