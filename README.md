# 🧩 VMS.API Solution

**VMS.API** is a multi-project, modular .NET solution for managing and delivering Vehicle Management Services (VMS) or a similar enterprise service. It follows a clean architecture pattern, separating concerns between API, models, data access, conversion logic, common utilities, and Entity Framework infrastructure.

---

## 📚 Table of Contents

- [Overview](#overview)
- [Solution Structure](#solution-structure)
- [Features](#features)
- [Technology Stack](#technology-stack)

---

## 📖 Overview

This solution is designed for modular enterprise-scale application development. Each project in the solution serves a specific purpose, enabling better maintainability, testability, and scalability.

---

## 🏗 Solution Structure

VMS.API.sln
│
├── VMS.API/                  # Main Web API project (entry point)
├── VMS.Model/                # Domain models and DTOs
├── VMS.EntityFramework/      # EF DbContext and migrations
├── VMS.DataAccess/           # Repository and data access logic
├── VMS.Converter/            # Entity <-> DTO converters
└── VMS.Common/               # Shared constants, helpers, configs

✨ Features
✅ RESTful API backend (VMS.API)

✅ Clean separation of concerns

✅ Entity Framework Core for data access

✅ Custom model converters for clean data contracts

✅ Shared utility code in VMS.Common

✅ Scalable and testable architecture


🧰 Technology Stack
.NET SDK (version to be confirmed from .csproj)

C#

ASP.NET Core Web API

Entity Framework Core

Visual Studio 2022+

SQL Server (assumed, confirm in EF config)
