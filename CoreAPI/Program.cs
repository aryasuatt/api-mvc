using CoreAPI.Context;
using CoreAPI.Services;
using Microsoft.EntityFrameworkCore;
using CoreAPI.Controllers;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Database connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services for ImageService, CartService, UserService, and IUserService
builder.Services.AddScoped<ImageService>();  // Add ImageService to DI container
builder.Services.AddScoped<CartService>();   // Add CartService to DI container
builder.Services.AddScoped<IUserService, UserService>();  // Add IUserService and UserService to DI container

// API Controllers
builder.Services.AddControllers();  // Ensure controllers are registered

// Swagger and OpenAPI configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Response caching
builder.Services.AddResponseCaching();

//// CORS configuration
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowSpecificOrigin", policy =>
//    {
//        policy.WithOrigins("http://127.0.0.1:5500")  // Allowed origins
//              .AllowAnyMethod() // Allowed HTTP methods (GET, POST, etc.)
//              .AllowAnyHeader(); // Allowed headers

//        // Uncomment below for open CORS policy
//        // policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
//    });
//});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()  // Tüm kaynaklardan gelen istekleri kabul eder
              .AllowAnyMethod()  // Tüm HTTP metodlarýna izin verir (GET, POST, PUT, DELETE vb.)
              .AllowAnyHeader(); // Tüm baþlýklara izin verir
    });
});




// Identity service configuration
builder.Services.AddIdentity<CoreAPI.Models.ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// JWT authentication configuration
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"]
        };
    });

// Authorization
builder.Services.AddAuthorization();

var app = builder.Build();

// Swagger and development environment configuration
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//// CORS middleware
//app.UseCors("AllowSpecificOrigin");

// CORS politikalarýný uygula
app.UseCors("AllowAllOrigins");

// HTTPS redirection and other middleware
app.UseHttpsRedirection();
app.UseResponseCaching();
app.UseAuthentication();
app.UseAuthorization();

// Map controllers
app.MapControllers();

app.Run();
