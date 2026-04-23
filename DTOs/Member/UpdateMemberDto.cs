using System.ComponentModel.DataAnnotations;

namespace PollakLibrary.Api.DTOs.Member;

public class UpdateMemberDto
{
    [Required(ErrorMessage = "FullName is required.")]
    [StringLength(200, ErrorMessage = "FullName cannot exceed 200 characters.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Email must be a valid email address.")]
    [StringLength(200, ErrorMessage = "Email cannot exceed 200 characters.")]
    public string Email { get; set; } = string.Empty;
}
