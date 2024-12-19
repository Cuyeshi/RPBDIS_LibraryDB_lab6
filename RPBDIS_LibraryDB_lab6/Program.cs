using Microsoft.EntityFrameworkCore;
using RPBDIS_LibraryDB_lab6.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllers();



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Настройка статических файлов и страницы по умолчанию
app.UseDefaultFiles(); // Устанавливает index.html как главную страницу по умолчанию
app.UseStaticFiles();  // Разрешает доступ к статическим файлам (например, wwwroot/index.html)

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Настройка маршрута по умолчанию (чтобы открылся index.html вместо Swagger)
app.MapFallbackToFile("/index.html");

app.Run();
