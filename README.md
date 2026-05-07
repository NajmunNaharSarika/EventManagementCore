# 🎫 EventManagementCore

A full-featured **ASP.NET Core MVC** web application for managing clients and events — built under the **Aurum Events & Co.** brand. The system supports client registration with image upload, event assignment, role-based access control, and an elegant card-based UI.

---

## ✨ Features

- **Client Management** — Create, view, edit, and delete client records with profile photo upload
- **Event Management** — Full CRUD for events with active/inactive status tracking
- **Event Assignment** — Assign one or more events to each client through a dynamic partial view
- **Role-Based Authorization** — Fine-grained access control using ASP.NET Core Identity (`Admin`, `Manager`)
- **Role Administration** — Create roles and assign them to registered users via a dedicated panel
- **Running Events Widget** — A View Component that displays the current count of active events in the layout
- **Image Upload** — Client profile images are stored in the `wwwroot/Images` folder with randomized filenames

---

## 🛠️ Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core MVC (.NET 8) |
| ORM | Entity Framework Core |
| Auth & Identity | ASP.NET Core Identity |
| Database | SQL Server (via `ApplicationDbContext`) |
| Frontend | Razor Views, Bootstrap, custom CSS |
| UI Fonts | Cormorant Garamond, Montserrat |

---

## 📁 Project Structure

```
EventManagementCore/
├── Controllers/
│   ├── ClientsController.cs       # Client CRUD + image upload + event linking
│   ├── EventsController.cs        # Event CRUD
│   ├── HomeController.cs          # Home & error pages
│   └── RoleController.cs          # Role creation & user-role assignment
│
├── Models/
│   ├── Client.cs                  # Client entity
│   ├── Event.cs                   # Event entity
│   ├── EventService.cs            # Join table: Client ↔ Event
│   ├── ErrorViewModel.cs
│   └── ViewModels/
│       └── ClientVM.cs            # Client form view model (includes IFormFile)
│
├── ViewComponents/
│   └── RunningEventsViewComponent.cs   # Counts active events for sidebar/navbar
│
├── Views/
│   ├── Clients/
│   │   ├── Index.cshtml           # Card-based client registry
│   │   ├── Create.cshtml
│   │   ├── Edit.cshtml
│   │   ├── Delete.cshtml
│   │   └── _addNewEvent.cshtml    # Partial: event selector dropdown
│   ├── Events/
│   │   ├── Index.cshtml
│   │   ├── Create.cshtml
│   │   ├── Edit.cshtml
│   │   └── Delete.cshtml
│   └── Shared/
│       └── _Layout.cshtml
│
├── Data/
│   └── ApplicationDbContext.cs
│
└── wwwroot/
    └── Images/                    # Uploaded client profile photos
```

---

## 🗄️ Data Model

```
Client ──< EventService >── Event
```

- A **Client** can be enrolled in many **Events**
- An **Event** can have many **Clients**
- **EventService** is the many-to-many join entity

---

## 🔐 Authorization Roles

| Action | Required Role |
|---|---|
| Create a client | `Admin` |
| Edit a client | `Admin` or `Manager` |
| Delete a client | `Admin` or `Manager` |
| Manage roles | *(configurable via `[Authorize]` on `RoleController`)* |

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- SQL Server (LocalDB or full instance)
- Visual Studio 2022 / VS Code

### Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/NajmunNaharSarika/EventManagementCore
   cd EventManagementCore
   ```

2. **Configure the database connection** in `appsettings.json`
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=EventManagementCoreDb;Trusted_Connection=True;"
   }
   ```

3. **Apply migrations**
   ```bash
   dotnet ef database update
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```
   Navigate to `https://localhost:5001`

5. **Create an Admin user** by registering via the Identity UI, then use the **Role** panel (`/Role`) to create the `Admin` and `Manager` roles and assign them to users.

---

## 📸 Key Pages

| Page | Route | Description |
|---|---|---|
| Client Registry | `/Clients` | Card grid with avatar, details & booked events |
| Add Client | `/Clients/Create` | Form with image upload and event multi-select |
| Edit Client | `/Clients/Edit/{id}` | Pre-populated form, re-assigns events |
| Event List | `/Events` | Table of all events |
| Role Manager | `/Role` | Create roles & assign to users |

---

## 📌 Notes

- Profile images must be in a supported format (`.jpg`, `.png`, etc.); they are saved to `wwwroot/Images/` with a random filename to avoid collisions.
- The `RunningEventsViewComponent` can be rendered anywhere in the layout using `@await Component.InvokeAsync("RunningEvents")`.
- Deleting a client also removes all associated `EventService` records (cascade handled manually in the controller).

---
