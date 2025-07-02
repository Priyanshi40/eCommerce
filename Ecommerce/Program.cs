using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var conn = builder.Configuration.GetConnectionString("EcommerceDBConnection");
builder.Services.AddDbContext<E_CommerceContext>(q => q.UseNpgsql(conn));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                    .AddEntityFrameworkStores<E_CommerceContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/Login";

    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    // options.SlidingExpiration = true; 
});

builder.Services.AddControllersWithViews();

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

app.UseAuthorization();

app.MapAreaControllerRoute(
    name: "AdminArea",
    areaName: "Admin",
    pattern: "Products/{controller=Home}/{action=Index}/{id?}");

app.MapAreaControllerRoute(
    name: "UserArea",
    areaName: "User",
    pattern: "Services/{controller=Home}/{action=Index}/{id?}");

app.MapAreaControllerRoute(
    name: "VendorArea",
    areaName: "Vendor",
    pattern: "Services/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
