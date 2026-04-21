namespace PollakLibrary.Api.Models;

public enum BorrowStatus
{
    Borrowed,
    Returned
}

public class BorrowRecord
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public int MemberId { get; set; }
    public DateTime BorrowDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public BorrowStatus Status { get; set; }

    public Book Book { get; set; } = null!;
    public Member Member { get; set; } = null!;
}