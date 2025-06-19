using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using ProjectLibraryGroup2;
using ProjectMvcGroup2.Data;
using ProjectMvcGroup2.Models;
using ProjectMvcGroup2.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services
    .AddDefaultIdentity<AppUser>
    (
            options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
            }
    )
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<IAppUserRepo, AppUserRepo>();
builder.Services.AddTransient<IEquipmentRentalRepo, EquipmentRentalRepo>();
builder.Services.AddScoped<ILodgingRepo, LodgingRepo>();
builder.Services.AddTransient<ILiftTicketRepo, LiftTicketRepo>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddTransient<ILodgingEmailSender, LodgingEmailSender>();
builder.Services.AddTransient<IEquipmentRentalEmailSender, EquipmentRentalEmailSender>();
builder.Services.AddTransient<ILiftTicketEmailSender, LiftTicketEmailSender>();

var app = builder.Build();

var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    InitialDatabase.SeedDatabase(services);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred seeding the DB.");
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();