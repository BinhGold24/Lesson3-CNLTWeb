namespace Lesson3_CNLTWeb.Data
{
    /// <summary>
    /// Tự động tạo database BookManagement và bảng Book khi ứng dụng khởi động.
    /// </summary>
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}
