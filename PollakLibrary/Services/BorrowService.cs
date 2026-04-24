using PollakLibrary.Api.DTOs.BorrowRecord;
using PollakLibrary.Api.Models;
using PollakLibrary.Api.Repositories.Interfaces;
using PollakLibrary.Api.Services.Interfaces;

namespace PollakLibrary.Api.Services;

public class BorrowService : IBorrowService
{
    private readonly IBorrowRecordRepository _borrowRepo;
    private readonly IBookRepository _bookRepo;
    private readonly IMemberRepository _memberRepo;

    public BorrowService(
        IBorrowRecordRepository borrowRepo,
        IBookRepository bookRepo,
        IMemberRepository memberRepo)
    {
        _borrowRepo = borrowRepo;
        _bookRepo = bookRepo;
        _memberRepo = memberRepo;
    }

    // ── GET ALL ──────────────────────────────────────────────────────────────
    public async Task<IEnumerable<BorrowRecordResponseDto>> GetAllAsync()
    {
        var records = await _borrowRepo.GetAllAsync();
        return records.Select(MapToDto);
    }

    // ── GET BY MEMBER ────────────────────────────────────────────────────────
    public async Task<IEnumerable<BorrowRecordResponseDto>> GetByMemberIdAsync(int memberId)
    {
        // Verify member exists first
        var memberExists = await _memberRepo.GetByIdAsync(memberId);
        if (memberExists is null)
            throw new KeyNotFoundException($"Member with ID {memberId} not found.");

        var records = await _borrowRepo.GetByMemberIdAsync(memberId);
        return records.Select(MapToDto);
    }

    // ── BORROW ───────────────────────────────────────────────────────────────
    public async Task<BorrowRecordResponseDto> BorrowBookAsync(BorrowRequestDto dto)
    {
        // Verify book exists
        var book = await _bookRepo.GetByIdAsync(dto.BookId)
            ?? throw new KeyNotFoundException($"Book with ID {dto.BookId} not found.");

        // Verify member exists
        var memberExists = await _memberRepo.GetByIdAsync(dto.MemberId);
        if (memberExists is null)
            throw new KeyNotFoundException($"Member with ID {dto.MemberId} not found.");

        // Quick pre-check to give a friendly error before the atomic update
        if (book.AvailableCopies <= 0)
            throw new InvalidOperationException($"No available copies of '{book.Title}'.");

        // *** CONCURRENCY-SAFE atomic decrement ***
        // Uses: UPDATE Books SET AvailableCopies = AvailableCopies - 1
        //       WHERE Id = @id AND AvailableCopies > 0
        // If another request took the last copy between the check and here,
        // rowsAffected will be 0 → we return 409 Conflict. AvailableCopies can never go < 0.
        int rowsAffected = await _borrowRepo.TryDecrementAvailableCopiesAsync(dto.BookId);
        if (rowsAffected == 0)
            throw new InvalidOperationException($"No available copies of '{book.Title}'. The last copy was just borrowed.");

        var record = new BorrowRecord
        {
            BookId = dto.BookId,
            MemberId = dto.MemberId,
            BorrowDate = DateTime.UtcNow,
            Status = BorrowStatus.Borrowed
        };

        var created = await _borrowRepo.CreateAsync(record);

        // Reload with navigation properties for the response DTO
        var full = await _borrowRepo.GetByIdAsync(created.Id);
        return MapToDto(full!);
    }

    // ── RETURN ───────────────────────────────────────────────────────────────
    public async Task<BorrowRecordResponseDto> ReturnBookAsync(int recordId)
    {
        var record = await _borrowRepo.GetByIdAsync(recordId)
            ?? throw new KeyNotFoundException($"Borrow record with ID {recordId} not found.");

        if (record.Status == BorrowStatus.Returned)
            throw new InvalidOperationException("This book has already been returned.");

        record.Status = BorrowStatus.Returned;
        record.ReturnDate = DateTime.UtcNow;

        await _borrowRepo.UpdateAsync(record);
        await _borrowRepo.IncrementAvailableCopiesAsync(record.BookId);

        var full = await _borrowRepo.GetByIdAsync(record.Id);
        return MapToDto(full!);
    }

    // ── MAPPER ───────────────────────────────────────────────────────────────
    private static BorrowRecordResponseDto MapToDto(BorrowRecord br) => new()
    {
        Id = br.Id,
        BookId = br.BookId,
        BookTitle = br.Book?.Title ?? string.Empty,
        MemberId = br.MemberId,
        MemberName = br.Member?.FullName ?? string.Empty,
        BorrowDate = br.BorrowDate,
        ReturnDate = br.ReturnDate,
        Status = br.Status.ToString()
    };
}
