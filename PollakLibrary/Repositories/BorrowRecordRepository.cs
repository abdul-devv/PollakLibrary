using Microsoft.EntityFrameworkCore;
using PollakLibrary.Api.Data;
using PollakLibrary.Api.Models;
using PollakLibrary.Api.Repositories.Interfaces;

namespace PollakLibrary.Api.Repositories;

public class BorrowRecordRepository : IBorrowRecordRepository
{
    private readonly LibraryDbContext _context;

    public BorrowRecordRepository(LibraryDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BorrowRecord>> GetAllAsync()
        => await _context.BorrowRecords
            .Include(br => br.Book)
            .Include(br => br.Member)
            .AsNoTracking()
            .ToListAsync();

    public async Task<IEnumerable<BorrowRecord>> GetByMemberIdAsync(int memberId)
        => await _context.BorrowRecords
            .Include(br => br.Book)
            .Include(br => br.Member)
            .Where(br => br.MemberId == memberId)
            .AsNoTracking()
            .ToListAsync();

    public async Task<BorrowRecord?> GetByIdAsync(int id)
        => await _context.BorrowRecords
            .Include(br => br.Book)
            .Include(br => br.Member)
            .FirstOrDefaultAsync(br => br.Id == id);

    public async Task<BorrowRecord> CreateAsync(BorrowRecord record)
    {
        _context.BorrowRecords.Add(record);
        await _context.SaveChangesAsync();
        return record;
    }

    public async Task<BorrowRecord> UpdateAsync(BorrowRecord record)
    {
        _context.BorrowRecords.Update(record);
        await _context.SaveChangesAsync();
        return record;
    }

    /// <summary>
    /// Atomic conditional decrement: only decrements if AvailableCopies > 0.
    /// Returns rows affected: 1 = success, 0 = no copies left (concurrent race lost).
    /// </summary>
    public async Task<int> TryDecrementAvailableCopiesAsync(int bookId)
        => await _context.Books
            .Where(b => b.Id == bookId && b.AvailableCopies > 0)
            .ExecuteUpdateAsync(setters =>
                setters.SetProperty(b => b.AvailableCopies, b => b.AvailableCopies - 1));

    /// <summary>
    /// Safely increments AvailableCopies (up to TotalCopies) on book return.
    /// </summary>
    public async Task IncrementAvailableCopiesAsync(int bookId)
        => await _context.Books
            .Where(b => b.Id == bookId && b.AvailableCopies < b.TotalCopies)
            .ExecuteUpdateAsync(setters =>
                setters.SetProperty(b => b.AvailableCopies, b => b.AvailableCopies + 1));
}
