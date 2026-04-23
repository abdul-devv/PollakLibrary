using PollakLibrary.Api.DTOs.Member;

namespace PollakLibrary.Api.Services.Interfaces;

public interface IMemberService
{
    Task<IEnumerable<MemberResponseDto>> GetAllAsync();
    Task<MemberResponseDto> GetByIdAsync(int id);
    Task<MemberResponseDto> CreateAsync(CreateMemberDto dto);
    Task<MemberResponseDto> UpdateAsync(int id, UpdateMemberDto dto);
    Task DeleteAsync(int id);
}
