using Microsoft.EntityFrameworkCore;
using PollakLibrary.Api.Data;
using PollakLibrary.Api.Models;
using PollakLibrary.Api.Repositories.Interfaces;

namespace PollakLibrary.Api.Repositories;

public class MemberRepository : IMemberRepository
{
    private readonly LibraryDbContext _context;

    public MemberRepository(LibraryDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Member>> GetAllAsync()
        => await _context.Members.AsNoTracking().ToListAsync();

    public async Task<Member?> GetByIdAsync(int id)
        => await _context.Members.FindAsync(id);

    public async Task<Member> CreateAsync(Member member)
    {
        _context.Members.Add(member);
        await _context.SaveChangesAsync();
        return member;
    }

    public async Task<Member> UpdateAsync(Member member)
    {
        _context.Members.Update(member);
        await _context.SaveChangesAsync();
        return member;
    }

    public async Task DeleteAsync(Member member)
    {
        _context.Members.Remove(member);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> EmailExistsAsync(string email, int? excludeId = null)
        => await _context.Members.AnyAsync(m =>
            m.Email == email && (excludeId == null || m.Id != excludeId));
}
