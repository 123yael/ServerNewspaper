using BLL.Functions;
using DAL.Actions.Classes;
using DAL.Actions.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

IConfiguration _configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(p => p.AddPolicy("AlowAll", option =>
{
    option.AllowAnyMethod();
    option.AllowAnyHeader();
    option.AllowAnyOrigin();
}));

builder.Services.AddDbContext<NewspaperSystemContext>(y => y.UseSqlServer(_configuration["NewspaperSystemContextString"]));

builder.Services.AddScoped<IAdPlacementActions, AdPlacementActions>();
builder.Services.AddScoped<IAdSizeActions, AdSizeActions>();
builder.Services.AddScoped<ICustomerActions, CustomerActions>();
builder.Services.AddScoped<IDatesForOrderDetailActions, DatesForOrderDetailActions>();
builder.Services.AddScoped<INewspapersPublishedActions, NewspapersPublishedActions>();
builder.Services.AddScoped<IOrderActions, OrderActions>();
builder.Services.AddScoped<IOrderDetailActions, OrderDetailActions>();
builder.Services.AddScoped<IWordAdSubCategoryActions, WordAdSubCategoryActions>();




builder.Services.AddScoped<IFuncs, Funcs>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors("AlowAll");
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
