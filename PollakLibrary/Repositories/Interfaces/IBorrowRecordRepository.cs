using PollakLibrary.Api.Models;

namespace PollakLibrary.Api.Repositories.Interfaces;

public interface IBorrowRecordRepository
{
    Task<IEnumerable<BorrowRecord>> GetAllAsync();
    Task<IEnumerable<BorrowRecord>> GetByMemberIdAsync(int memberId);
    Task<BorrowRecord?> GetByIdAsync(int id);
    Task<BorrowRecord> CreateAsync(BorrowRecord record);
    Task<BorrowRecord> UpdateAsync(BorrowRecord record);

    /// <summary>
    /// Atomically decrements AvailableCopies WHERE AvailableCopies > 0.
    /// Returns the number of rows affected (0 = no copies available).
    /// </summary>
    Task<int> TryDecrementAvailableCopiesAsync(int bookId);

    /// <summary>
    /// Atomically increments AvailableCopies on return.
    /// </summary>
    Task IncrementAvailableCopiesAsync(int bookId);
}
