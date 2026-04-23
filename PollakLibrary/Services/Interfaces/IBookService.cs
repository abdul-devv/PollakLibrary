using PollakLibrary.Api.DTOs.Book;

namespace PollakLibrary.Api.Services.Interfaces;

public interface IBookService
{
    Task<IEnumerable<BookResponseDto>> GetAllAsync();
    Task<BookResponseDto> GetByIdAsync(int id);
    Task<BookResponseDto> CreateAsync(CreateBookDto dto);
    Task<BookResponseDto> UpdateAsync(int id, UpdateBookDto dto);
    Task DeleteAsync(int id);
}
