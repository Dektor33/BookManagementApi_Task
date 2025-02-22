using BookManagementApi_GO.BusinessLogic;
using BookManagementApi_GO.DataAcess;
using BookManagementApi_GO.Database.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder (args);

// Add DbContext and repository/services for product and auth
builder.Services.AddDbContext<AppDbContext> (options =>
{
    options.UseSqlServer (builder.Configuration.GetConnectionString ("DefaultConnection"));
});

builder.Services.AddScoped<IBookService , BookService> ();
builder.Services.AddScoped<IBookRepository , BookRepository> ();

builder.Services.AddScoped<IAuthRepository , AuthRepository> ();
builder.Services.AddScoped<IAuthService , AuthService> ();

// Add authentication and authorization
var secret = builder.Configuration.GetValue<string> ("Jwt:Secret");
builder.Services.AddAuthentication (JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer (options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true ,
            ValidateAudience = true ,
            ValidateLifetime = true ,
            ValidateIssuerSigningKey = true ,
            ValidIssuer = "doseHp" ,
            ValidAudience = "doseHp" ,
            IssuerSigningKey = new SymmetricSecurityKey (Encoding.UTF8.GetBytes (secret!))
        };
    });

builder.Services.AddAuthorization ();


// Configure Swagger for API
builder.Services.AddControllers ();
builder.Services.AddEndpointsApiExplorer ();
builder.Services.AddSwaggerGen (c =>
{
    c.AddSecurityDefinition ("Bearer" , new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header ,
        Description = "Please insert JWT with Bearer into field" ,
        Name = "Authorization" ,
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement (new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Add ProblemDetails service for error handling
builder.Services.AddProblemDetails ();

var app = builder.Build ();


// Configure HTTP request pipeline
app.UseExceptionHandler ();
app.UseAuthentication ();  // Ensure authentication middleware is called before authorization
app.UseAuthorization ();


// Map API routes and Blazor components
app.MapControllers ();

// Enable Swagger in development
if (app.Environment.IsDevelopment ())
{
    app.UseSwagger ();
    app.UseSwaggerUI ();
}

await app.RunAsync ();