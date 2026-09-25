using EFCoreDemo.Data;
using EFCoreDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCoreDemo.Repositories;

/// <summary>
/// Tầng truy cập dữ liệu (data access).
/// Là tầng DUY NHẤT (ngoài Program.cs) được phép chạm vào AppDbContext.
///
/// Trách nhiệm: nhận AppDbContext từ bên ngoài vào và thực hiện các thao tác
/// đọc/ghi database cho Product.
///
/// KHÔNG được chứa: Console.ReadLine(), Console.WriteLine(), menu, UI logic,
/// hay quy tắc nghiệp vụ (validate) — những thứ đó thuộc tầng khác.
///
/// Không cần interface IProductRepository ở bước này: chỉ có 1 implementation
/// duy nhất, thêm interface lúc này là over-engineering.
/// </summary>
public class ProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Lấy toàn bộ sản phẩm.
    /// </summary>
    public List<Product> GetAll()
    {
        return _context.Products
            .OrderBy(p => p.Id)
            .ToList();
    }

    /// <summary>
    /// Lấy 1 sản phẩm theo Id. Trả về null nếu không tìm thấy.
    /// </summary>
    public Product? GetById(int id)
    {
        return _context.Products
            .FirstOrDefault(p => p.Id == id);
    }

    /// <summary>
    /// Tìm sản phẩm theo tên (tìm gần đúng, không phân biệt hoa/thường).
    /// </summary>
    public List<Product> Search(string keyword)
    {
        // Chuẩn hóa về lowercase để PostgreSQL dịch thành ILIKE.
        string pattern = (keyword ?? string.Empty).ToLower();

        return _context.Products
            .Where(p => p.Name.ToLower().Contains(pattern))
            .OrderBy(p => p.Id)
            .ToList();
    }

    /// <summary>
    /// Thêm sản phẩm mới. Trả về chính entity đã được thêm (đã có Id do DB sinh).
    /// </summary>
    public Product Add(Product product)
    {
        _context.Products.Add(product);
        _context.SaveChanges();

        return product;
    }

    /// <summary>
    /// Cập nhật sản phẩm đã tồn tại (entity này đang được EF Core theo dõi).
    /// Trả về false nếu sản phẩm không còn trong database.
    /// </summary>
    public bool Update(Product product)
    {
        bool exists = _context.Products.Any(p => p.Id == product.Id);

        if (!exists)
        {
            return false;
        }

        _context.SaveChanges();
        return true;
    }

    /// <summary>
    /// Xóa sản phẩm theo Id. Trả về false nếu không tìm thấy.
    /// </summary>
    public bool Delete(int id)
    {
        Product? product = GetById(id);

        if (product == null)
        {
            return false;
        }

        _context.Products.Remove(product);
        _context.SaveChanges();

        return true;
    }
}