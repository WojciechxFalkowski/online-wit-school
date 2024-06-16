using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TSQLV6.Models;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<UniversityDbContext>(options => options.UseSqlServer(
              builder.Configuration.GetConnectionString("DefaultConnection"),
               options => options.CommandTimeout(3600))
           );

// Add IHttpContextAccessor
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Add services to the container
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation(); // AddRazorRuntimeCompilation() -> MUST HAVE

// Dodajemy usługi uwierzytelniania
builder.Services.AddAuthentication("MyCookieAuth").AddCookie("MyCookieAuth", options =>
{
    options.Cookie.Name = "auth_token"; // Zmieniona nazwa cookie na "auth_token"
    options.LoginPath = "/Users/Login";
    options.AccessDeniedPath = "/Home/AccessDenied";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Dodajemy middleware uwierzytelniania i autoryzacji
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
