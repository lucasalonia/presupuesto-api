using Microsoft.EntityFrameworkCore;
using presupuesto_api.Data;
using presupuesto_api.Repositories;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using presupuesto_api.Models;
using presupuesto_api.Services;

var builder = WebApplication.CreateBuilder(args);

//Lectura de configuraciones
var jwtKey = builder.Configuration["JwtSettings:Key"];
var jwtIssuer = builder.Configuration["JwtSettings:Issuer"];
var jwtAudience = builder.Configuration["JwtSettings:Audience"];
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


//Registro de servicios esenciales
builder.Services.AddControllers(); 

//Conexión a la base de datos
builder.Services.AddDbContext<DataContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

//Builder para tokens. Configura la autenticación JWT y define los parámetros de validación del token
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

// Configuración de opciones para JWT, permitiendo inyectar estas configuraciones en los servicios que lo requieran
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtSettings"));

// Dependencia de repositorios
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();
builder.Services.AddScoped<IPresupuestoRepositorio, PresupuestoRepositorio>();
builder.Services.AddScoped<IProyeccionRepositorio, ProyeccionRepositorio>();

//Dependencia de servicios
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IEncriptadorService, EncriptadorService>();



var app = builder.Build();

// Pipeline de la aplicación
app.UseHttpsRedirection();

app.UseAuthentication(); 
app.UseAuthorization();

//Mapea las rutas automáticas hacia los controladores
app.MapControllers(); 

app.Run();