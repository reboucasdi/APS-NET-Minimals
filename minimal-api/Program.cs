using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MinimalApi.DTOs;
using MinimalApi.Dominio.Interfaces;
using MinimalApi.Infraestrutura.Db;
using MinimalApi.Infraestrutura.Repositorios;
using MinimalApi.Servicos;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configuração do banco de dados
builder.Services.AddDbContext<DbContexto>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuração do JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Injeção de dependência
builder.Services.AddScoped<IVeiculoRepository, VeiculoRepository>();
builder.Services.AddSingleton<AuthService>();

var app = builder.Build();

// Endpoints de autenticação
app.MapPost("/login", (LoginDTO loginDTO, AuthService authService) =>
{
    if (authService.ValidarUsuario(loginDTO.Email, loginDTO.Senha))
    {
        var token = authService.GerarTokenJWT(loginDTO);
        return Results.Ok(new { token });
    }
    else
    {
        return Results.Unauthorized();
    }
});

// Endpoints de CRUD de veículos
app.MapGet("/veiculos", async (IVeiculoRepository repo) => await repo.GetAllAsync()).RequireAuthorization();
app.MapGet("/veiculos/{id}", async (int id, IVeiculoRepository repo) => await repo.GetByIdAsync(id) ?? Results.NotFound()).RequireAuthorization();
app.MapPost("/veiculos", async (Veiculo veiculo, IVeiculoRepository repo) =>
{
    await repo.AddAsync(veiculo);
    return Results.Created($"/veiculos/{veiculo.Id}", veiculo);
}).RequireAuthorization();
app.MapPut("/veiculos/{id}", async (int id, Veiculo veiculo, IVeiculoRepository repo) =>
{
    var existente = await repo.GetByIdAsync(id);
    if (existente == null) return Results.NotFound();

    existente.Placa = veiculo.Placa;
    existente.Modelo = veiculo.Modelo;
    existente.Ano = veiculo.Ano;
    existente.Cor = veiculo.Cor;

    await repo.UpdateAsync(existente);
    return Results.NoContent();
}).RequireAuthorization();
app.MapDelete("/veiculos/{id}", async (int id, IVeiculoRepository repo) =>
{
    await repo.DeleteAsync(id);
    return Results.NoContent();
}).RequireAuthorization();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
