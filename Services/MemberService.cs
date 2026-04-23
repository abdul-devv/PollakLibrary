using PollakLibrary.Api.DTOs.Member;
using PollakLibrary.Api.Models;
using PollakLibrary.Api.Repositories.Interfaces;
using PollakLibrary.Api.Services.Interfaces;

namespace PollakLibrary.Api.Services;

public class MemberService : IMemberService
{
    private readonly IMemberRepository _memberRepo;

    public MemberService(IMemberRepository memberRepo)
    {
        _memberRepo = memberRepo;
    }

    public async Task<IEnumerable<MemberResponseDto>> GetAllAsync()
    {
        var members = await _memberRepo.GetAllAsync();
        return members.Select(MapToDto);
    }

    public async Task<MemberResponseDto> GetByIdAsync(int id)
    {
        var member = await _memberRepo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Member with ID {id} not found.");
        return MapToDto(member);
    }

    public async Task<MemberResponseDto> CreateAsync(CreateMemberDto dto)
    {
        if (await _memberRepo.EmailExistsAsync(dto.Email))
            throw new InvalidOperationException($"A member with email '{dto.Email}' already exists.");

        var member = new Member
        {
            FullName = dto.FullName,
            Email = dto.Email,
            MembershipDate = DateTime.UtcNow
        };

        var created = await _memberRepo.CreateAsync(member);
        return MapToDto(created);
    }

    public async Task<MemberResponseDto> UpdateAsync(int id, UpdateMemberDto dto)
    {
        var member = await _memberRepo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Member with ID {id} not found.");

        if (await _memberRepo.EmailExistsAsync(dto.Email, excludeId: id))
            throw new InvalidOperationException($"Another member with email '{dto.Email}' already exists.");

        member.FullName = dto.FullName;
        member.Email = dto.Email;

        var updated = await _memberRepo.UpdateAsync(member);
        return MapToDto(updated);
    }

    public async Task DeleteAsync(int id)
    {
        var member = await _memberRepo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Member with ID {id} not found.");

        await _memberRepo.DeleteAsync(member);
    }

    private static MemberResponseDto MapToDto(Member m) => new()
    {
        Id = m.Id,
        FullName = m.FullName,
        Email = m.Email,
        MembershipDate = m.MembershipDate
    };
}
