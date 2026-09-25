namespace EFCoreDemo.Models;

/// <summary>
/// Lớp model (entity) đại diện cho bảng "Products" trong database.
/// Chỉ chứa dữ liệu + ràng buộc dữ liệu của bản thân thực thể.
/// Không chứa Console, không chứa truy vấn EF Core, không chứa business logic.
/// </summary>
public class Product
{
    public int Id { get; set; }

    /// <summary>
    /// Tên sản phẩm. Bắt buộc, EF Core hiểu là NOT NULL (nullable reference types đang bật).
    /// </summary>
    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int Quantity { get; set; }
}