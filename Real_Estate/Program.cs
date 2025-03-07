using Real_Estate.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Real_Estate.Models;
using Real_Estate.Repository.Appointments;
using Real_Estate.Repository.EstateProperties;
using Real_Estate.Repository.OwnerSchedules;
using Real_Estate.Repository.PropertyCategoryRepositories;
using Real_Estate.Repository.PropertyCategories;
using Real_Estate.Repository.SalesOrRents;
using Real_Estate.Repository.Users;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<RealEDbContext>();

builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IEstatePropertyRepository, EstatePropertyRepository>();
builder.Services.AddScoped<IOwnerScheduleRepository, OwnerScheduleRepository>();
builder.Services.AddScoped<IPropertyCategoryRepository, PropertyCategoryRepository>();
builder.Services.AddScoped<ISaleOrRentRepository, SaleOrRentRepository>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();

builder.Services.AddControllersWithViews();
builder.Services.AddIdentity<ApplicationUser , IdentityRole>()
    .AddEntityFrameworkStores<RealEDbContext>();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 1;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
app.Automigrate();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Properties}/{action=Properties}/{id?}");


//REDbInitializer.Seed(app);
app.Run();


