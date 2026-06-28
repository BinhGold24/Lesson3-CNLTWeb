using System.Net.Sockets;
using Lesson3_CNLTWeb.Data;
using Lesson3_CNLTWeb.Middleware;
using Lesson3_CNLTWeb.Repositories; 
using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 1. SỬA TÊN CHUỖI KẾT NỐI thành "DefaultConnection"
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// 2. SỬA TÊN DBCONTEXT thành ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<BookRepository>();

var app = builder.Build();

// 3. ĐÃ BỎ PHẦN DBINITIALIZER VÌ DATABASE ĐÃ ĐƯỢC TẠO BẰNG SQL RỒI
// (Chỉ giữ lại phần catch lỗi để bạn tham khảo nếu sau này cần)
using (var scope = app.Services.CreateScope())
{
    try
    {
        // Kiểm tra xem có kết nối được database không
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.CanConnect();
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("DatabaseCheck");
        logger.LogError(ex, "Không thể kết nối database. Kiểm tra lại chuỗi kết nối hoặc SQL Server đang chạy.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseRouting();
app.UseAuthorization();
app.UseMiddleware<RequestLoggingMiddleware>();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

try
{
    app.Run();
}
catch (IOException ex) when (ex.InnerException is AddressInUseException or SocketException)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Không thể khởi động web: cổng localhost đang được sử dụng.");
    Console.WriteLine("Dừng instance cũ (Ctrl+C) hoặc đóng terminal đang chạy ứng dụng, rồi chạy lại dotnet run.");
    Console.ResetColor();
    throw;
}