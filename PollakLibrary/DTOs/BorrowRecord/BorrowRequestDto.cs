using System.ComponentModel.DataAnnotations;

namespace PollakLibrary.Api.DTOs.BorrowRecord;

public class BorrowRequestDto
{
    [Range(1, int.MaxValue, ErrorMessage = "BookId must be a valid Id.")]
    public int BookId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "MemberId must be a valid Id.")]
    public int MemberId { get; set; }
}
