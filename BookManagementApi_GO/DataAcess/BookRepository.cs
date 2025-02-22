using BookManagementApi_GO.Database.Data;
using BookManagementApi_GO.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookManagementApi_GO.DataAcess
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _context;

        public BookRepository ( AppDbContext context )
        {
            _context = context;
        }

        public async Task<IEnumerable<string>> GetBookTitlesAsync ( int pageNumber , int pageSize )
        {
            int currentYear = DateTime.UtcNow.Year;

            return await _context.Books
                .OrderBy (b => (b.Views * 0.5) + ((currentYear - b.PublicationYear) * 2)) // Popularity Score
                .Select (b => b.Title)
                .Skip ((pageNumber - 1) * pageSize)
                .Take (pageSize)
                .ToListAsync ();
        }

        public async Task<Book> GetBookByIdAsync ( int id )
        {
            var book = await _context.Books.FindAsync (id);
            if (book != null)
            {
                book.Views++; // Increase book views count
                await _context.SaveChangesAsync ();
            }
            return book;
        }

        public async Task AddBookAsync ( Book book )
        {
            _context.Books.Add (book);
            await _context.SaveChangesAsync ();
        }

        public async Task AddBooksAsync ( IEnumerable<Book> books )
        {
            _context.Books.AddRange (books);
            await _context.SaveChangesAsync ();
        }

        public async Task UpdateBookAsync ( Book book )
        {
            _context.Books.Update (book);
            await _context.SaveChangesAsync ();
        }

        public async Task DeleteBookAsync ( int id )
        {
            var book = await _context.Books.FindAsync (id);
            if (book != null)
            {
                _context.Books.Remove (book);
                await _context.SaveChangesAsync ();
            }
        }

        public async Task DeleteBooksAsync ( IEnumerable<int> ids )
        {
            var books = await _context.Books.Where (b => ids.Contains (b.Id)).ToListAsync ();
            if (books.Any ())
            {
                _context.Books.RemoveRange (books);
                await _context.SaveChangesAsync ();
            }
        }

        public async Task<bool> BookExistsAsync ( string title )
        {
            return await _context.Books.AnyAsync (b => b.Title == title);
        }
    }
}
