using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ServiClientes.Application.Profiles;
using ServiClientes.Application.Services;
using ServiClientes.Application.Validators;
using ServiClientes.Data;
using ServiClientes.Infraestructure.Repository;
using ServiClientes.Model;
using ServiClientes.Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    sqlServerOptionsAction: sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
        maxRetryCount: 5,
        maxRetryDelay: TimeSpan.FromSeconds(30),
        errorNumbersToAdd: null
        );
    }
    ));

// Cargar configuración desde appsettings.json
builder.Services.Configure<PaginationSettings>(builder.Configuration.GetSection("Pagination"));

// Cargar configuracion de automapper
builder.Services.AddAutoMapper(typeof(Program));


// Cargar Validadores
//builder.Services.AddScoped<ClienteValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ClienteValidator>();


// Cargar configuracion de dependencias
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();  

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
