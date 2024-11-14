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
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()  // Allow all origins (use this only for development)
              .AllowAnyMethod()  // Allow all HTTP methods (GET, POST, PUT, DELETE, etc.)
              .AllowAnyHeader(); // Allow all headers
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
        options.RequireHttpsMetadata = false; // Set to true in production
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

// CORS middleware - apply the "AllowAllOrigins" policy
app.UseCors("AllowAllOrigins");  // Enable CORS for all origins, methods, and headers

// HTTPS redirection and other middleware
app.UseHttpsRedirection(); // Redirect HTTP to HTTPS
app.UseResponseCaching();  // Enable response caching
app.UseAuthentication();   // Enable authentication middleware
app.UseAuthorization();    // Enable authorization middleware

// Map controllers
app.MapControllers();

app.Run();
