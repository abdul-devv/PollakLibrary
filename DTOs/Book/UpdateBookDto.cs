using System.ComponentModel.DataAnnotations;

namespace PollakLibrary.Api.DTOs.Book;

public class UpdateBookDto
{
    [Required(ErrorMessage = "Title is required.")]
    [StringLength(300, ErrorMessage = "Title cannot exceed 300 characters.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Author is required.")]
    [StringLength(200, ErrorMessage = "Author cannot exceed 200 characters.")]
    public string Author { get; set; } = string.Empty;

    [Required(ErrorMessage = "ISBN is required.")]
    [StringLength(20, ErrorMessage = "ISBN cannot exceed 20 characters.")]
    public string ISBN { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "TotalCopies must be greater than 0.")]
    public int TotalCopies { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "AvailableCopies must be 0 or greater.")]
    public int AvailableCopies { get; set; }
}
