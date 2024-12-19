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

// ��������� ����������� ������ � �������� �� ���������
app.UseDefaultFiles(); // ������������� index.html ��� ������� �������� �� ���������
app.UseStaticFiles();  // ��������� ������ � ����������� ������ (��������, wwwroot/index.html)

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// ��������� �������� �� ��������� (����� �������� index.html ������ Swagger)
app.MapFallbackToFile("/index.html");

app.Run();
