using Microsoft.EntityFrameworkCore;
using IF133.Tugas1.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// Daftarkan controller
builder.Services.AddControllers();

// Daftarkan DbContext dengan provider SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Konfigurasi Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Konfigurasi pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
