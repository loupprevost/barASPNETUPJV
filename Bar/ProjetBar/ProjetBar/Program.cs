using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjetBar.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
// Configuration du service d'authentification
builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true; 
    options.Password.RequireLowercase = true; 
    options.Password.RequireNonAlphanumeric = true; 
    options.Password.RequireUppercase = true; 
    options.Password.RequiredLength = 6; 
    options.Password.RequiredUniqueChars = 1; 

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5; 
    options.Lockout.AllowedForNewUsers = true; 

    // User settings.
    options.User.AllowedUserNameCharacters = 
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+"; 
    options.User.RequireUniqueEmail = false; 
}); 

builder.Services.ConfigureApplicationCookie(options =>
{ 
    // Cookie settings
    options.Cookie.HttpOnly = true; 
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5); 
    options.LoginPath = "/Identity/Account/Login"; 
    options.AccessDeniedPath = "/Identity/Account/AccessDenied"; 
    options.SlidingExpiration = true; 
});

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
