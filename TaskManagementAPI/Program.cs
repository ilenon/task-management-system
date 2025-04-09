using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TaskManagementAPI.Data;
using TaskManagementAPI.Repositories;
using TaskManagementAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Adiciona o contexto do banco de dados
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registra o repositório e o serviço
builder.Services.AddScoped<ITaskRepository, TaskRepository>(); // Registra o repositório
builder.Services.AddScoped<ITaskService, TaskService>(); // Registra o serviço
builder.Services.AddScoped<IAuthService, AuthService>(); // Registra o serviço de autenticação

// Add services to the container.
builder.Services.AddControllers(); // Adiciona suporte para controllers

// Configurações de autenticação JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // Valida o emissor do token
            ValidateAudience = true, // Valida o público do token
            ValidateLifetime = true, // Valida o tempo de vida do token
            ValidateIssuerSigningKey = true, // Valida a chave de assinatura do token
            ValidIssuer = builder.Configuration["Jwt:Issuer"], // Emissor do token
            ValidAudience = builder.Configuration["Jwt:Audience"], // Público do token
            // Chave de assinatura do token
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });
builder.Services.AddAuthorization(); // Adiciona suporte para autorização
builder.Services.AddEndpointsApiExplorer(); // Adiciona suporte para explorar os endpoints
// Adiciona suporte para gerar a documentação do Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "TaskManagementAPI",
        Version = "v1"
    });

    // Adiciona o esquema de segurança JWT ao Swagger
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Insira o token JWT no formato: Bearer {token}"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>() // Não há escopos necessários para autenticação
        }
    });
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // Mapeia o OpenAPI
    app.UseSwagger(); // Ativa o Swager em ambiente de desenvolvimento
    app.UseSwaggerUI(); // Ativa a interface do Swagger UI
}

app.UseHttpsRedirection(); // Redireciona requisições HTTP para HTTPS
app.UseAuthentication(); // Ativa a autenticação
app.UseAuthorization(); // Ativa a autorização

app.MapControllers(); // Mapeia os controllers

app.Run();

