using DATN.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers()
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null; // Không sử dụng quy tắc đặt tên camelCase mặc định của c#
    options.JsonSerializerOptions.Converters.Add(new JsonDateTimeConverter("yyyy-MM-dd HH:mm:ss")); // config format thời gian
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// config cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
               builder => builder.AllowAnyOrigin()
                      .AllowAnyMethod()
                             .AllowAnyHeader());
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Setup DbContext for me
builder.Services.AddDbContext<QuanTriHeThongContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Server=(local)\\SQLSV2019EXP;Database=QuanTriHeThong;Trusted_Connection=True;TrustServerCertificate=True;Integrated Security=True;"));
});


var app = builder.Build();


// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}                      \

app.UseStaticFiles();

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
