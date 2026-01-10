using Ajerly_Platform.Data;
using Microsoft.EntityFrameworkCore;
using Ajerly_Platform.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Add Identity
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

// Add Razor Pages so Identity UI page models work correctly
builder.Services.AddRazorPages();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Identity UI routes
app.MapRazorPages();

// Ensure test user in Development for quick login testing
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var email = "test@ajerly.local";
    var user = userManager.FindByEmailAsync(email).GetAwaiter().GetResult();
    if (user == null)
    {
        var newUser = new ApplicationUser { UserName = email, Email = email, FullName = "مثال" };
        var createResult = userManager.CreateAsync(newUser, "Test123!").GetAwaiter().GetResult();
    }
}

app.Run();

