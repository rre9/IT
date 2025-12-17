using ITHelpDesk.Data;
using ITHelpDesk.Models;
using ITHelpDesk.Seed;
using ITHelpDesk.Services;
using ITHelpDesk.Services.Abstractions;
using ITHelpDesk.Services.Authorization;
using ITHelpDesk.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();

builder.Services.AddTransient<IUserValidator<ApplicationUser>, YubEmailDomainValidator>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("IsSupportOrAdmin", policy =>
        policy.RequireRole("Admin", "Support"));
    options.AddPolicy("TicketAccess", policy =>
        policy.Requirements.Add(new TicketAccessRequirement()));
});
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddScoped<ITicketAttachmentService, TicketAttachmentService>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddSingleton<DevConsoleEmailSender>();
builder.Services.AddTransient<IEmailSender, SmtpEmailSender>();
builder.Services.Configure<DepartmentOptions>(builder.Configuration.GetSection("Departments"));
builder.Services.AddScoped<IDepartmentProvider, DepartmentProvider>();
builder.Services.AddScoped<ITicketQueryService, TicketQueryService>();
builder.Services.AddScoped<IAuthorizationHandler, TicketAccessHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    await IdentitySeeder.SeedAsync(scope.ServiceProvider);
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
