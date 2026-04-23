using Microsoft.AspNetCore.Mvc;
using PollakLibrary.Api.DTOs.Member;
using PollakLibrary.Api.Services.Interfaces;

namespace PollakLibrary.Api.Controllers;

[ApiController]
[Route("api/members")]
[Produces("application/json")]
public class MembersController : ControllerBase
{
    private readonly IMemberService _memberService;

    public MembersController(IMemberService memberService)
    {
        _memberService = memberService;
    }

    ///Get all library members.
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<MemberResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var members = await _memberService.GetAllAsync();
        return Ok(members);
    }

    ///Get a single member by ID.
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(MemberResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var member = await _memberService.GetByIdAsync(id);
        return Ok(member);
    }

    ///Register a new library member.
    [HttpPost]
    [ProducesResponseType(typeof(MemberResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateMemberDto dto)
    {
        var created = await _memberService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    ///Update an existing member.
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(MemberResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateMemberDto dto)
    {
        var updated = await _memberService.UpdateAsync(id, dto);
        return Ok(updated);
    }

    ///Delete a member by ID.
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await _memberService.DeleteAsync(id);
        return NoContent();
    }
}
