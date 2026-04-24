using Microsoft.EntityFrameworkCore;
using PollakLibrary.Api.Data;
using PollakLibrary.Api.Middleware;
var builder = WebApplication.CreateBuilder(args);
// 1. Add DB
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
builder.Services.AddMemoryCache(); 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register Repositories
builder.Services.AddScoped<PollakLibrary.Api.Repositories.Interfaces.IBookRepository, PollakLibrary.Api.Repositories.BookRepository>();
builder.Services.AddScoped<PollakLibrary.Api.Repositories.Interfaces.IMemberRepository, PollakLibrary.Api.Repositories.MemberRepository>();
builder.Services.AddScoped<PollakLibrary.Api.Repositories.Interfaces.IBorrowRecordRepository, PollakLibrary.Api.Repositories.BorrowRecordRepository>(); 

// Register Services
builder.Services.AddScoped<PollakLibrary.Api.Services.Interfaces.IBookService, PollakLibrary.Api.Services.BookService>();
builder.Services.AddScoped<PollakLibrary.Api.Services.Interfaces.IMemberService, PollakLibrary.Api.Services.MemberService>();
builder.Services.AddScoped<PollakLibrary.Api.Services.Interfaces.IBorrowService, PollakLibrary.Api.Services.BorrowService>();
var app = builder.Build();
// 2. Global Error Handling
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
// 3. Auto-Migrate DB on Start
using (var scope = app.Services.CreateScope()) {
    scope.ServiceProvider.GetRequiredService<LibraryDbContext>().Database.EnsureCreated();
}
if (app.Environment.IsDevelopment()) { app.UseSwagger(); app.UseSwaggerUI(); }
app.MapControllers();
app.Run();
