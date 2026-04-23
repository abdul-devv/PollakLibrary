using PollakLibrary.Api.Models;

namespace PollakLibrary.Api.Repositories.Interfaces;

public interface IMemberRepository
{
    Task<IEnumerable<Member>> GetAllAsync();
    Task<Member?> GetByIdAsync(int id);
    Task<Member> CreateAsync(Member member);
    Task<Member> UpdateAsync(Member member);
    Task DeleteAsync(Member member);
    Task<bool> EmailExistsAsync(string email, int? excludeId = null);
}
