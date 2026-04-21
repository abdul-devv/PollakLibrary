using Microsoft.EntityFrameworkCore;
using PollakLibrary.Api.Data;
using PollakLibrary.Api.Middleware;
var builder = WebApplication.CreateBuilder(args);
// 1. Add DB
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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
