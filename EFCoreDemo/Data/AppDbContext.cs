using EFCoreDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCoreDemo.Data;

/// <summary>
/// Chịu trách nhiệm duy nhất: làm cầu nối giữa C# và PostgreSQL qua EF Core.
/// Chỉ chứa DbSet + cấu hình kết nối + cấu hình model (nếu có).
/// Không chứa Console UI, menu hay business logic.
/// </summary>
public class AppDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(
            "Host=localhost;" +
            "Port=5433;" +
            "Database=efcore_demo;" +
            "Username=postgres;" +
            "Password=123456");
    }

    // Ghi chú dạy học:
    // Lớp này KHÔNG có OnModelCreating, và cũng KHÔNG cần thêm.
    // Những gì bạn thấy trong Migrations/AppDbContextModelSnapshot.cs
    // (Entity("Product"), ToTable("Products"), HasKey("Id")...) chính là
    // model mặc định mà EF Core suy ra từ Product.cs.
    // Nếu thêm OnModelCreating để đổi tên bảng/cột/kiểu dữ liệu,
    // thì đó là thay đổi schema -> lúc đó mới cần migration mới.
    // Bước refactor này KHÔNG được làm việc đó.
}