using Microsoft.AspNetCore.Mvc;
using PollakLibrary.Api.DTOs.BorrowRecord;
using PollakLibrary.Api.Services.Interfaces;

namespace PollakLibrary.Api.Controllers;

[ApiController]
[Route("api/borrow-records")]
[Produces("application/json")]
public class BorrowRecordsController : ControllerBase
{
    private readonly IBorrowService _borrowService;

    public BorrowRecordsController(IBorrowService borrowService)
    {
        _borrowService = borrowService;
    }

    ///Get all borrow records.
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BorrowRecordResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var records = await _borrowService.GetAllAsync();
        return Ok(records);
    }

    ///Get borrow history for a specific member.
    [HttpGet("member/{memberId:int}")]
    [ProducesResponseType(typeof(IEnumerable<BorrowRecordResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByMember(int memberId)
    {
        var records = await _borrowService.GetByMemberIdAsync(memberId);
        return Ok(records);
    }

    ///Borrow a book for a member.
    [HttpPost("borrow")]
    [ProducesResponseType(typeof(BorrowRecordResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Borrow([FromBody] BorrowRequestDto dto)
    {
        var record = await _borrowService.BorrowBookAsync(dto);
        return CreatedAtAction(nameof(GetAll), new { }, record);
    }

    ///Return a borrowed book by borrow record ID.
    [HttpPut("return/{recordId:int}")]
    [ProducesResponseType(typeof(BorrowRecordResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Return(int recordId)
    {
        var record = await _borrowService.ReturnBookAsync(recordId);
        return Ok(record);
    }
}
