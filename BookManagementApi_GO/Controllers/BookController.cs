using BookManagementApi_GO.BusinessLogic;
using BookManagementApi_GO.Model.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookManagementApi_GO.Controllers
{
    [Authorize (Roles = "Admin,Reader")]
    [Route ("api/Books")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController ( IBookService bookService )
        {
            _bookService = bookService;
        }

        [HttpGet ("titles")]
        [AllowAnonymous] // Anyone can access this method
        public async Task<IActionResult> GetAllBooks ( [FromQuery] int pageNumber = 1 , [FromQuery] int pageSize = 10 )
        {
            var bookTitles = await _bookService.GetBookTitlesAsync (pageNumber , pageSize);
            return Ok (bookTitles);
        }

        [HttpGet ("{id}")]
        [AllowAnonymous] // Anyone can access this method
        public async Task<IActionResult> GetBookById ( int id )
        {
            var book = await _bookService.GetBookByIdAsync (id);
            if (book == null)
                return NotFound ();

            return Ok (book);
        }

        [HttpPost]
        [Authorize (Roles = "Admin")]
        public async Task<IActionResult> AddBook ( [FromBody] Book book )
        {
            try
            {
                await _bookService.AddBookAsync (book);
                return CreatedAtAction (nameof (GetBookById) , new { id = book.Id } , book);
            }
            catch (Exception ex)
            {
                return BadRequest (ex.Message);
            }
        }

        [HttpPost ("bulk")]
        [Authorize (Roles = "Admin")]
        public async Task<IActionResult> AddBooks ( [FromBody] IEnumerable<Book> books )
        {
            await _bookService.AddBooksAsync (books);
            return Ok ("Books added successfully.");
        }

        [HttpPut ("{id}")]
        public async Task<IActionResult> UpdateBook ( int id , [FromBody] Book book )
        {
            if (id != book.Id)
                return BadRequest ("Book ID mismatch");

            await _bookService.UpdateBookAsync (book);
            return NoContent ();
        }

        [HttpDelete ("{id}")]
        public async Task<IActionResult> DeleteBook ( int id ) //  Hard delete
        {
            await _bookService.DeleteBookAsync (id);
            return NoContent ();
        }

        [HttpDelete ("bulk")]
        public async Task<IActionResult> DeleteBooks ( [FromBody] IEnumerable<int> ids ) //  Bulk delete
        {
            await _bookService.DeleteBooksAsync (ids);
            return NoContent ();
        }
    }
}
