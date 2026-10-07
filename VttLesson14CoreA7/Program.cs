using Microsoft.EntityFrameworkCore;
using VttLesson14CoreA7.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Cấu hình AppDbContext: Ưu tiên SQL Server (SQLEXPRESS02), tự động fallback InMemory nếu chưa bật SQL Server
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
bool canConnectSqlServer = false;

if (!string.IsNullOrEmpty(connectionString))
{
    try
    {
        using var testConn = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
        testConn.Open();
        canConnectSqlServer = true;
        testConn.Close();
    }
    catch
    {
        canConnectSqlServer = false;
    }
}

builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (canConnectSqlServer && !string.IsNullOrEmpty(connectionString))
    {
        options.UseSqlServer(connectionString);
    }
    else
    {
        options.UseInMemoryDatabase("VttLesson14CoreA7Db");
    }
});

var app = builder.Build();

// Khởi tạo CSDL và nạp dữ liệu mẫu ban đầu (Seeding Data)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        DbSeeder.Seed(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogWarning(ex, "Không thể seed CSDL SQL Server, chuyển sang chế độ dự phòng.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// BÀI 3 BƯỚC 4: Khai báo Route cho Area Admin trước route mặc định
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

// Route mặc định cho người dùng
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
