using Lesson3_CNLTWeb.Data;
using Lesson3_CNLTWeb.Models;
using Lesson3_CNLTWeb.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Lesson3_CNLTWeb.Controllers
{
    public class BookController : Controller
    {
        private readonly BookRepository _bookRepository;

        public BookController(BookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public IActionResult Index()
        {
            // Đã sửa thành GetAllBooks()
            return View(_bookRepository.GetAllBooks());
        }

        public IActionResult Detail(int id)
        {
            // Đã sửa thành GetBookById(id)
            var book = _bookRepository.GetBookById(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Book book)
        {
            // Kiểm tra tính hợp lệ của dữ liệu trước khi thêm
            if (ModelState.IsValid)
            {
                _bookRepository.AddBook(book); // Gọi đúng hàm AddBook() trong Repository
                return RedirectToAction(nameof(Index)); // Thêm xong thì quay về trang Index
            }
            return View(book);
        }
    }
}