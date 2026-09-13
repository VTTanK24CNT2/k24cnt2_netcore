using Microsoft.AspNetCore.Mvc;
using VttLab06.Models;

namespace VttLab06.ViewComponents
{
    public class BookViewComponent : ViewComponent
    {
        protected Book bookModel = new Book();

        public IViewComponentResult Invoke()
        {
            // Lấy danh sách sách (hoặc lọc các sách nổi bật / popular)
            var books = bookModel.GetBookList();
            return View(books);
        }
    }
}
