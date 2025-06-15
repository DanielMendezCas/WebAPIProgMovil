using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using WebAPIProgMovil.Endpoints;
using WebAPIProgMovil.Repositorios;
using WebAPIProgMovil.Servicios;
using WebAPIProgMovil.Utilidades;

var builder = WebApplication.CreateBuilder(args);

// Inicio de área de servicios
builder.Services.AddEndpointsApiExplorer(); // Permite que swagger pueda explorar los endpoints
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IRepositorioUsuarios, RepositorioUsuarios>();
builder.Services.AddScoped<IRepositorioPines, RepositorioPines>();
builder.Services.AddScoped<IAlmacenadorArchivos, AlmacenadorArchivos>();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddAuthentication().AddJwtBearer(opciones =>
    opciones.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        //IssuerSigningKey = Llaves.ObtenerLlave(builder.Configuration).First(),
        IssuerSigningKeys = Llaves.ObtenerTodasLasLlaves(builder.Configuration),
        ClockSkew = TimeSpan.Zero
    });
builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IUserStore<IdentityUser>, UsuarioStore>();
builder.Services.AddIdentityCore<IdentityUser>();
builder.Services.AddTransient<SignInManager<IdentityUser>>(); //Logear usuarios

// Fin de área de servicios
var app = builder.Build();
// Inicio de área de los middleware


if (builder.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapGroup("/pines").MapPines();
app.MapGroup("/usuarios").MapUsuarios();

// Fin de área de los middelware
app.Run();