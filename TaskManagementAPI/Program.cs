var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(); // Adiciona suporte para controllers

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Adiciona suporte ao Swagger

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger(); // Ativa o Swager em ambiente de desenvolvimento
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Mapeia automaticamente os controlladores
app.MapControllers();

app.Run();

