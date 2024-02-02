using BookStoreAPI.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : ControllerBase
    {
        public BooksController(BookStoreDbContext bookStoreDbContext)
        {
            BookStoreDbContext = bookStoreDbContext;
        }

        public BookStoreDbContext BookStoreDbContext { get; }

        [HttpGet(Name = "SearchBooks")]
        public IEnumerable<Book> SearchBooks([FromQuery] SearchColumn? searchColumn, [FromQuery] string? searchValue)
        {
            if(searchColumn == null || searchValue == null)
            {
                return BookStoreDbContext.Books.ToList();
            }   

            switch (searchColumn)
            {
                case SearchColumn.Author:
                    return BookStoreDbContext.BooksByAuthor(searchValue);
                case SearchColumn.ISBN:
                    return BookStoreDbContext.BooksByISBN(searchValue);
                case SearchColumn.AvailableCopies:
                    return BookStoreDbContext.BooksByAvailableCopies(searchValue);
                case SearchColumn.TotalCopies:
                    return BookStoreDbContext.BooksByTotalCopies(searchValue);
                default:
                    return BookStoreDbContext.Books.ToList();
            }
        }
    }

    public enum SearchColumn
    {
        Author,
        ISBN,
        AvailableCopies,
        TotalCopies
    }
}