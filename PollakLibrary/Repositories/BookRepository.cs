using Microsoft.EntityFrameworkCore;
using PollakLibrary.Api.Data;
using PollakLibrary.Api.Models;
using PollakLibrary.Api.Repositories.Interfaces;

namespace PollakLibrary.Api.Repositories;

public class BookRepository : IBookRepository
{
    private readonly LibraryDbContext _context;

    public BookRepository(LibraryDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
        => await _context.Books.AsNoTracking().ToListAsync();

    public async Task<Book?> GetByIdAsync(int id)
        => await _context.Books.FindAsync(id);

    public async Task<Book> CreateAsync(Book book)
    {
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task<Book> UpdateAsync(Book book)
    {
        _context.Books.Update(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task DeleteAsync(Book book)
    {
        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsbnExistsAsync(string isbn, int? excludeId = null)
        => await _context.Books.AnyAsync(b =>
            b.ISBN == isbn && (excludeId == null || b.Id != excludeId));
}
