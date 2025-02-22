using BookManagementApi_GO.DataAcess;
using BookManagementApi_GO.Model.Entities;

namespace BookManagementApi_GO.BusinessLogic
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService ( IBookRepository bookRepository )
        {
            _bookRepository = bookRepository;
        }

        public async Task<IEnumerable<string>> GetBookTitlesAsync ( int pageNumber , int pageSize )
        {
            return await _bookRepository.GetBookTitlesAsync (pageNumber , pageSize);
        }

        public async Task<Book?> GetBookByIdAsync ( int id )
        {
            return await _bookRepository.GetBookByIdAsync (id);
        }

        public async Task AddBookAsync ( Book book )
        {
            var exists = await _bookRepository.BookExistsAsync (book.Title);
            if (exists)
                throw new Exception ("A book with the same title already exists.");

            await _bookRepository.AddBookAsync (book);
        }

        public async Task AddBooksAsync ( IEnumerable<Book> books )
        {
            // Extract the titles from the input books
            var newTitles = books.Select (b => b.Title).ToHashSet ();

            // Fetch all existing book titles in one query
            var existingTitles = new HashSet<string> (await _bookRepository.GetBookTitlesAsync (1 , int.MaxValue));

            // Filter out books that already exist
            var uniqueBooks = books.Where (b => !existingTitles.Contains (b.Title)).ToList ();

            if (!uniqueBooks.Any ())
                throw new InvalidOperationException ("All provided books have duplicate titles.");

            await _bookRepository.AddBooksAsync (uniqueBooks);
        }

        public async Task UpdateBookAsync ( Book book )
        {
            await _bookRepository.UpdateBookAsync (book);
        }

        public async Task DeleteBookAsync ( int id ) //  Hard delete
        {
            await _bookRepository.DeleteBookAsync (id);
        }

        public async Task DeleteBooksAsync ( IEnumerable<int> ids ) //  Bulk delete
        {
            await _bookRepository.DeleteBooksAsync (ids);
        }
    }
}
