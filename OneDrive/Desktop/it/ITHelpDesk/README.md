# IT Help Desk – ASP.NET Core 8 MVC

## Getting started

1. Install the .NET 8 SDK and SQL Server (LocalDB works fine).
2. Restore and build the solution:
   ```bash
   dotnet build
   ```
3. Apply the latest Entity Framework Core migrations:
   ```bash
   dotnet ef database update --project ITHelpDesk/ITHelpDesk.csproj
   ```
4. Run the web app:
   ```bash
   dotnet run --project ITHelpDesk/ITHelpDesk.csproj
   ```
5. Browse to `https://localhost:5001` (or the URL printed in the console).

## Seeded admin account

At startup the identity seeder ensures the following roles and admin account exist:

| Email               | Password      | Roles             |
|---------------------|---------------|-------------------|
| `yazan@yub.com.sa`  | Provided via configuration (`Seed:AdminDefaultPassword`) | Admin, Support |

Use this account to reach the Admin Panel and bootstrap additional users.

## SMTP configuration

Email confirmation and password-reset email use `IEmailSender`. Provide Mailtrap (or any SMTP) credentials via `appsettings.Development.json` or user secrets:

```json
{
  "EmailSettings": {
    "Host": "smtp.mailtrap.io",
    "Port": 587,
    "UserName": "YOUR_MAILTRAP_USERNAME",
    "Password": "YOUR_MAILTRAP_PASSWORD",
    "From": "it-helpdesk@yub.com.sa"
  }
}
```

In non-configured environments, the fallback `DevConsoleEmailSender` logs the email body to the console.

## Departments

Departments displayed in filters and ticket creation are driven by configuration:

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

Update the list above to tailor the system to your organization.

## Tests

Run integration tests with:

```bash
dotnet test
```

The tests verify:
- Authorization (employees receive 403 when accessing the full ticket dashboard)
- File upload validation rules
- Ticket status changes log activity when performed by Admins
