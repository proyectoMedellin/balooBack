using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SiecaAPI.Commons;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
// Swagger config
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new Microsoft.IdentityModel
            .Tokens.TokenValidationParameters
        {
            ValidateIssuer = bool.Parse(AppParamsTools.GetEnvironmentVariable("Jwt:ValidateIssuer")),
            ValidateAudience = bool.Parse(AppParamsTools.GetEnvironmentVariable("Jwt:ValidateAudience")),
            ValidateLifetime = bool.Parse(AppParamsTools.GetEnvironmentVariable("Jwt:ValidateLifetime")),
            ValidateIssuerSigningKey = bool.Parse(AppParamsTools.GetEnvironmentVariable("Jwt:ValidateIssuerSigningKey")),
            ValidIssuer = AppParamsTools.GetEnvironmentVariable("Jwt:Issuer"),
            ValidAudience = AppParamsTools.GetEnvironmentVariable("Jwt:Audience"),
            IssuerSigningKey = new SymmetricSecurityKey(
                 Encoding.UTF8.GetBytes(AppParamsTools.GetEnvironmentVariable("Jwt:Key")))
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();
app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
