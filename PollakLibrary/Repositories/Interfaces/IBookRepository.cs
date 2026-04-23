using PollakLibrary.Api.Models;

namespace PollakLibrary.Api.Repositories.Interfaces;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAllAsync();
    Task<Book?> GetByIdAsync(int id);
    Task<Book> CreateAsync(Book book);
    Task<Book> UpdateAsync(Book book);
    Task DeleteAsync(Book book);
    Task<bool> IsbnExistsAsync(string isbn, int? excludeId = null);
}
