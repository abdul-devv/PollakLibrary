using Microsoft.EntityFrameworkCore;
using PollakLibrary.Api.Models;
namespace PollakLibrary.Api.Data;
public class LibraryDbContext : DbContext {
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<BorrowRecord> BorrowRecords => Set<BorrowRecord>();
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<BorrowRecord>().Property(b => b.Status).HasConversion<string>();
    }
}
