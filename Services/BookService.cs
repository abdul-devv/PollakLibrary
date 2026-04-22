using Microsoft.Extensions.Caching.Memory;
using PollakLibrary.Api.DTOs.Book;
using PollakLibrary.Api.Models;
using PollakLibrary.Api.Repositories.Interfaces;
using PollakLibrary.Api.Services.Interfaces;

namespace PollakLibrary.Api.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepo;
    private readonly IMemoryCache _cache;
    private const string AllBooksCacheKey = "all_books";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromSeconds(60);

    public BookService(IBookRepository bookRepo, IMemoryCache cache)
    {
        _bookRepo = bookRepo;
        _cache = cache;
    }

    // ── GET ALL (Cached) ─────────────────────────────────────────────────────
    public async Task<IEnumerable<BookResponseDto>> GetAllAsync()
    {
        if (_cache.TryGetValue(AllBooksCacheKey, out IEnumerable<BookResponseDto>? cached) && cached is not null)
            return cached;

        var books = await _bookRepo.GetAllAsync();
        var dtos = books.Select(MapToDto).ToList();

        _cache.Set(AllBooksCacheKey, dtos, CacheDuration);
        return dtos;
    }

    // ── GET BY ID ────────────────────────────────────────────────────────────
    public async Task<BookResponseDto> GetByIdAsync(int id)
    {
        var book = await _bookRepo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Book with ID {id} not found.");
        return MapToDto(book);
    }

    // ── CREATE ───────────────────────────────────────────────────────────────
    public async Task<BookResponseDto> CreateAsync(CreateBookDto dto)
    {
        // Business rule: AvailableCopies must not exceed TotalCopies
        if (dto.AvailableCopies > dto.TotalCopies)
            throw new InvalidOperationException("AvailableCopies cannot exceed TotalCopies.");

        // Business rule: ISBN must be unique
        if (await _bookRepo.IsbnExistsAsync(dto.ISBN))
            throw new InvalidOperationException($"A book with ISBN '{dto.ISBN}' already exists.");

        var book = new Book
        {
            Title = dto.Title,
            Author = dto.Author,
            ISBN = dto.ISBN,
            TotalCopies = dto.TotalCopies,
            AvailableCopies = dto.AvailableCopies
        };

        var created = await _bookRepo.CreateAsync(book);
        InvalidateCache();
        return MapToDto(created);
    }

    // ── UPDATE ───────────────────────────────────────────────────────────────
    public async Task<BookResponseDto> UpdateAsync(int id, UpdateBookDto dto)
    {
        var book = await _bookRepo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Book with ID {id} not found.");

        if (dto.AvailableCopies > dto.TotalCopies)
            throw new InvalidOperationException("AvailableCopies cannot exceed TotalCopies.");

        if (await _bookRepo.IsbnExistsAsync(dto.ISBN, excludeId: id))
            throw new InvalidOperationException($"Another book with ISBN '{dto.ISBN}' already exists.");

        book.Title = dto.Title;
        book.Author = dto.Author;
        book.ISBN = dto.ISBN;
        book.TotalCopies = dto.TotalCopies;
        book.AvailableCopies = dto.AvailableCopies;

        var updated = await _bookRepo.UpdateAsync(book);
        InvalidateCache();
        return MapToDto(updated);
    }

    // ── DELETE ───────────────────────────────────────────────────────────────
    public async Task DeleteAsync(int id)
    {
        var book = await _bookRepo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Book with ID {id} not found.");

        await _bookRepo.DeleteAsync(book);
        InvalidateCache();
    }

    // ── HELPERS ──────────────────────────────────────────────────────────────
    private void InvalidateCache() => _cache.Remove(AllBooksCacheKey);

    private static BookResponseDto MapToDto(Book b) => new()
    {
        Id = b.Id,
        Title = b.Title,
        Author = b.Author,
        ISBN = b.ISBN,
        TotalCopies = b.TotalCopies,
        AvailableCopies = b.AvailableCopies
    };
}
