using System.Net.Mime;
using Microsoft.EntityFrameworkCore;
using SolarWatch.Controllers;
using SolarWatch.Data;
using SolarWatch.Repository;
using SolarWatchMvp.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
AddDbContext();

builder.Services.AddControllers(
    options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ISolarDataProvider, OpenSolarMapApi>();
builder.Services.AddSingleton<IJsonProcessor, JsonProcessor>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<ISunTimeRepository, SunTimeRepository>();
builder.Services.AddDbContext<CityApiContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
/*if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); //Added by default
    app.UseSwagger();
    app.UseSwaggerUI();
}*/

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
//Docker run command: docker run -d -p 8080:80 -e ASPNETCORE_ENVIRONMENT=Development solarwatchapp

// using static System.Net.Mime.MediaTypeNames;
app.UseStatusCodePages(MediaTypeNames.Text.Plain, "Status Code Page: {0}");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

void AddDbContext()
{
    builder.Services.AddDbContext<CityApiContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
}