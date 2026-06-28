using Lesson3_CNLTWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace Lesson3_CNLTWeb.Data
{
    /// <summary>
    /// Thực hiện các thao tác CRUD với bảng Book qua Entity Framework Core.
    /// </summary>
    public class BookRepository
    {
        private readonly AppDbContext _context;

        public BookRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Book> GetAll()
        {
            return _context.Books
                .OrderBy(b => b.Id)
                .ToList(); // SELECT * FROM Books ORDER BY Id
        }

        public Book? GetById(int id)
        {
            return _context.Books.Find(id); // SELECT * FROM Books WHERE Id = @id
        }

        public void Add(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
        }
        // UPDATE: Cập nhật thông tin sách
        public void Update(Book book)
        {
            _context.Books.Update(book);
            _context.SaveChanges(); // Lưu thay đổi xuống SQL Server
        }

        // DELETE: Xóa sách
        public void Delete(int id)
        {
            var book = _context.Books.Find(id); // Tìm sách theo ID
            if (book != null)
            {
                _context.Books.Remove(book); // Xóa khỏi ORM
                _context.SaveChanges();      // Cập nhật lệnh xóa xuống SQL Server
            }
        }

        public bool Delete(int id)
        {
            var book = _context.Books.Find(id);
            if (book == null)
            {
                return false;
            }

            _context.Books.Remove(book); // DELETE FROM Books WHERE Id = @id
            _context.SaveChanges();
            return true;
        }
    }
}
