using PollakLibrary.Api.DTOs.BorrowRecord;

namespace PollakLibrary.Api.Services.Interfaces;

public interface IBorrowService
{
    Task<IEnumerable<BorrowRecordResponseDto>> GetAllAsync();
    Task<IEnumerable<BorrowRecordResponseDto>> GetByMemberIdAsync(int memberId);
    Task<BorrowRecordResponseDto> BorrowBookAsync(BorrowRequestDto dto);
    Task<BorrowRecordResponseDto> ReturnBookAsync(int recordId);
}
