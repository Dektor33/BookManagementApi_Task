using BookManagementApi_GO.Model.Entities;

namespace BookManagementApi_GO.BusinessLogic
{
    public interface IBookService
    {
        Task<IEnumerable<string>> GetBookTitlesAsync ( int pageNumber , int pageSize );
        Task<Book?> GetBookByIdAsync ( int id );
        Task AddBookAsync ( Book book );
        Task AddBooksAsync ( IEnumerable<Book> books );
        Task UpdateBookAsync ( Book book );
        Task DeleteBookAsync ( int id ); // 🔹 Replace soft delete
        Task DeleteBooksAsync ( IEnumerable<int> ids ); // 🔹 Add bulk delete
    }
}
