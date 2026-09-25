using Microsoft.EntityFrameworkCore;
using VttLesson10EFDbFirst.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// Lấy chuỗi kết nói từ appsetting
var vttConnection = builder.Configuration.GetConnectionString("VttLesson10");
builder.Services.AddDbContext<VttLesson10EfdbContext>(x => x.UseSqlServer(vttConnection));
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=VttMembers}/{action=Index}/{id?}");

app.Run();
