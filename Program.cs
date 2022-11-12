using AdminPanel.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//получаем строку подключения из appsetiings.json
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//Добавляем сервис для подлючения к MySQL при помощи строки подключения
//в качестве типа для AddDbContext используем ApplicationContext
builder.Services.AddDbContext<ApplicationContext>(options => 
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

//подключаем identity тут также добавляем токины
builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ApplicationContext>()
    .AddDefaultTokenProviders();


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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
