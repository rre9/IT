# IT Help Desk System

A comprehensive, enterprise-grade IT support ticketing system built with ASP.NET Core 8.0 MVC.

## 🎯 Features

- **Role-Based Access Control**: Admin, Support, and Employee roles with appropriate permissions
- **Ticket Management**: Create, assign, track, and resolve IT support tickets
- **File Attachments**: Upload images and documents with tickets
- **Activity Logging**: Complete audit trail of all ticket actions
- **Email Notifications**: Automated email alerts for ticket assignments and status changes
- **Resource-Based Authorization**: Secure access control ensuring users can only view their authorized tickets
- **User Management**: Admin panel for managing users, roles, and account status
- **Modern UI**: Responsive design with Bootstrap 5 and Font Awesome icons

## 🚀 Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server (LocalDB works for development)
- Git

### Installation

1. Clone the repository:
```bash
git clone https://github.com/rre9/IT.git
cd IT/ITHelpDesk
```

2. Restore dependencies:
```bash
dotnet restore
```

3. Build the solution:
```bash
dotnet build
```

4. Apply database migrations:
```bash
dotnet ef database update --project ITHelpDesk/ITHelpDesk.csproj
```

5. Run the application:
```bash
dotnet run --project ITHelpDesk/ITHelpDesk.csproj
```

6. Browse to `https://localhost:5001` (or the URL shown in the console)

## 👤 Default Admin Account

At startup, the system automatically creates an admin account:

- **Email**: `yazan@yub.com.sa`
- **Password**: Configured via `appsettings.Development.json` → `Seed:AdminDefaultPassword`
- **Roles**: Admin, Support

Use this account to access the Admin Panel and manage users.

## ⚙️ Configuration

### Email Settings

Configure SMTP settings in `appsettings.Development.json`:

```json
{
  "EmailSettings": {
    "Host": "smtp.office365.com",
    "Port": 587,
    "UserName": "your-email@yub.com.sa",
    "Password": "your-password",
    "From": "it-helpdesk@yub.com.sa"
  }
}
```

If SMTP is not configured, the system uses `DevConsoleEmailSender` which logs emails to the console.

### Departments

Configure departments in `appsettings.Development.json`:

```json
{
  "Departments": {
    "Items": [
      "IT Operations",
      "Networking",
      "Security",
      "Infrastructure",
      "Applications"
    ]
  }
}
```

## 📁 Project Structure

```
ITHelpDesk/
├── Areas/Identity/          # Identity pages (Login, Register, etc.)
├── Controllers/            # MVC Controllers
├── Data/                   # Database context
├── Models/                 # Entity models
├── Services/               # Business logic services
│   ├── Abstractions/       # Service interfaces
│   └── Authorization/      # Authorization handlers
├── ViewModels/             # View models
├── Views/                  # Razor views
├── wwwroot/                # Static files
│   └── uploads/            # User-uploaded files
└── Migrations/             # EF Core migrations
```

## 🧪 Testing

Run the test suite:

```bash
dotnet test
```

The tests verify:
- Authorization (employees receive 403 when accessing admin-only areas)
- File upload validation rules
- Ticket status change logging

## 📊 Database

- **Development**: SQL Server LocalDB
- **Connection String**: `Server=(localdb)\mssqllocaldb;Database=ITHelpDesk;Trusted_Connection=True;MultipleActiveResultSets=true`
- **Location**: `C:\Users\{YourUsername}\ITHelpDesk.mdf`

## 📁 File Storage

- **Location**: `wwwroot/uploads/{TicketId}/`
- **Allowed Types**: JPG, PNG, PDF
- **Max Size**: 10 MB per file

## 🔒 Security Features

- Email domain validation (only @yub.com.sa emails allowed)
- Role-based access control (RBAC)
- Resource-based authorization (users can only see their tickets)
- CSRF protection
- SQL injection prevention (Entity Framework parameterized queries)
- Secure password hashing (ASP.NET Identity)

## 📝 Documentation

- [Presentation Scenario](ITHelpDesk/PRESENTATION_SCENARIO.md) - Professional presentation guide
- [Professional Assessment](ITHelpDesk/PROFESSIONAL_ASSESSMENT.md) - System evaluation
- [Data Storage](ITHelpDesk/DATA_STORAGE.md) - Database and file storage locations

## 🛠️ Technologies Used

- **Framework**: ASP.NET Core 8.0 MVC
- **ORM**: Entity Framework Core
- **Authentication**: ASP.NET Core Identity
- **Database**: SQL Server
- **Frontend**: Bootstrap 5, jQuery, Font Awesome
- **Email**: SMTP (configurable)

## 📄 License

This project is proprietary software developed for internal use.

## 👥 Contributors

- Initial development by the IT team

## 🔄 Future Enhancements

- Dashboard with analytics and charts
- SLA tracking
- Knowledge base integration
- REST API for external integrations
- Mobile app support

---

**For more information, see the [ITHelpDesk README](ITHelpDesk/README.md)**

